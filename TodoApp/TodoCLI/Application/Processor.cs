using System.Text.Json;
using TodoCLI.Application.Responses;
using TodoCLI.Infrastructure.Contracts;
using TodoCLI.Models;

namespace TodoCLI.Application;


public class Processor
{
    private readonly IDataAccess _dataAccess;

    public Processor(IDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    private string MapExceptionToMessage(Exception ex, string fallback)
    {
        return ex switch
        {
            FileNotFoundException => "data file not found.",
            JsonException => "data file is corrupted.",
            _ => fallback
        };
    }

    private Status GetStatusBasedOnCommand(string statusCommand)
    {
        return statusCommand switch
        {
            "mark-done" => Status.Done,
            "mark-pause" => Status.Paused,
            "mark-todo" => Status.Todo,
            "mark-inprogress" => Status.InProgress
        };
    }

    public ReadResponse GetAllTodos(Status? status = null)
    {
        try
        {
            List<Todo> todos = _dataAccess.GetAllTodos();

            if (status != null)
            {
                todos = todos.Where(t => t.Status == status).ToList();
            }

            return new ReadResponse
            {
                Todos = todos,
                Message = "Todos List: ",
            };
        }
        catch (Exception ex)
        {
            return new ReadResponse
            {
                Todos = [],
                Message = MapExceptionToMessage(ex, "failed to get todos list.")
            };
        }
    }

    public WriteResponse CreateTodo(string description)
    {
        try
        {
            _dataAccess.CreateTodo(description);
            return new WriteResponse
            {
                Message = "Todo created successfully."
            };
        }
        catch (Exception ex)
        {
            WriteResponse response = new WriteResponse
            {
                Message = MapExceptionToMessage(ex, "failed to create todo.")
            };

            return response;
        }
    }

    public ReadResponse GetTodo(int id)
    {
        try
        {
            Todo? todo = _dataAccess.GetTodo(id);
            if (todo == null)
            {
                return new ReadResponse
                {
                    Todos = [],
                    Message = "There is no todo with such id."
                };
            }

            return new ReadResponse
            {
                Todos = [todo],
                Message = "Todo: "
            };
        }
        catch (Exception ex)
        {
            return new ReadResponse
            {
                Message = MapExceptionToMessage(ex, "failed to get todo.")
            };
        }
    }

    public WriteResponse DeleteTodo(int id)
    {
        try
        {
            int? deletedTodoId = _dataAccess.DeleteTodo(id);

            if (deletedTodoId == null)
            {
                return new WriteResponse
                {
                    Message = "there is no todo with such id."
                };
            }

            return new WriteResponse
            {
                Message = $"todo with id of {deletedTodoId} is deleted."
            };
        }
        catch (Exception ex)
        {
            return new WriteResponse
            {
                Message = MapExceptionToMessage(ex, "failed to delete todo.")
            };
        }
    }

    public WriteResponse UpdateTodo(int id, string description)
    {
        try
        {
            int? updatedTodoId = _dataAccess.UpdateTodo(id, description);

            if (updatedTodoId == null)
            {
                return new WriteResponse
                {
                    Message = "there is no todo with such id."
                };
            }

            return new WriteResponse
            {
                Message = $"todo with id of {updatedTodoId} is updated."
            };
        }
        catch (Exception ex)
        {
            return new WriteResponse
            {
                Message = MapExceptionToMessage(ex, "failed to update todo")
            };
        }
    }

    public WriteResponse ChangeTodoStatus(int id, string statusCommand)
    {
        try
        {
            Status newStatus = GetStatusBasedOnCommand(statusCommand);

            int? updatedTodoId = _dataAccess.ChangeTodoStatus(id, newStatus);

            if (updatedTodoId == null)
            {
                return new WriteResponse
                {
                    Message = "there is no todo with such id."
                };
            }

            return new WriteResponse
            {
                Message = $"todo status with id of {updatedTodoId} is changed."
            };
        }
        catch(Exception ex)
        {
            return new WriteResponse
            {
                Message = MapExceptionToMessage(ex, "failed to change todo status.")
            };
        }
    }
}