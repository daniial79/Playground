using TodoCLI.Application;
using TodoCLI.Models;
using TodoCLI.Application.Responses;

namespace TodoCLI;

public class UI
{
    private readonly Processor _processor;

    public UI(Processor processor)
    {
        _processor = processor;
    }

    private void ShowReadResponse(ReadResponse response)
    {
        WriteLine(response.Message);
        WriteLine(new string('=', 40));

        foreach (Todo todo in response.Todos)
        {
            WriteLine($"ID         : {todo.Id}");
            WriteLine($"Description: {todo.Description}");
            WriteLine($"Status     : {todo.Status}");
            WriteLine($"Created    : {todo.CreatedAt:yyyy-MM-dd HH:mm}");
            WriteLine($"Updated    : {todo.UpdatedAt:yyyy-MM-dd HH:mm}");
            WriteLine(new string('-', 40));
            WriteLine();
        }
    }

    private void ShowWriteResponse(WriteResponse response)
    {
        WriteLine(response.Message);
    }


    private bool CommandIsValid(string command)
    {
        string[] validCommands = [
            "list", "create", "update", "get",
            "delete", "mark-done", "mark-pause",
            "mark-inprogress"];

        return validCommands.Contains(command);
    }

    public void Run(string[] args)
    {
        if (args.Length < 1)
        {
            WriteLine("No command provided.");
            WriteLine("usage: todo [command] <arguments>?");
            return;
        }

        string command = args[0].ToLower().Trim();
        string[] arguments = args[1..];

        if (!CommandIsValid(command))
        {
            WriteLine("Invalid command.");
            return;
        }

        if (command == "list")
        {
            HandleList(arguments);
        }
        else if (command == "create")
        {
            HandleCreate(arguments);
        }
        else if (command == "get")
        {
            HandleGet(arguments);
        }
        else if (command == "delete")
        {
            HandleGet(arguments);
        }
        else if (command == "update")
        {
            HandleUpdate(arguments);
        }
        else if (new string[] { "mark-done", "mark-inprogress", "mark-todo", "mark-done"}.Contains(command))
        {
            HandleChangeStatus(command, arguments);
        }
    }

    private void HandleList(string[] arguments)
    {
        if (arguments.Length > 0)
        {
            string filter = arguments[0].Replace("-", "").ToLower();
            bool success = Enum.TryParse(filter, true, out Status status);

            if (!success)
            {
                WriteLine("invalid argument for list command.");
                return;
            }

            ReadResponse filteredTodos = _processor.GetAllTodos(status);
            ShowReadResponse(filteredTodos);
        }
        else
        {
            ReadResponse todos = _processor.GetAllTodos();
            ShowReadResponse(todos);
        }
    }

    private void HandleCreate(string[] arguments)
    {
        if (arguments.Length < 1)
        {
            WriteLine("you have to provide todo descriptin while creating one.");
            return;
        }

        string description = arguments[0].Trim();

        if (string.IsNullOrWhiteSpace(description))
        {
            WriteLine("description can not be empty or white space only.");
            return;
        }

        WriteResponse response = _processor.CreateTodo(description);
        ShowWriteResponse(response);
    }

    private void HandleGet(string[] argumments)
    {
        if (argumments.Length < 1)
        {
            WriteLine("you have to provide todo id.");
            return;
        }

        if (!int.TryParse(argumments[0], out int todoId))
        {
            WriteLine("invalid todo id. id must be an non-negative integer.");
            return;
        }

        ReadResponse response = _processor.GetTodo(todoId);
        ShowReadResponse(response);
    }

    private void HandleDelete(string[] arguments)
    {
        if (arguments.Length < 1)
        {
            WriteLine("you have to provide todo id.");
            return;
        }

        if (!int.TryParse(arguments[0], out int todoId))
        {
            WriteLine("invalid todo id. id must be an non-negative integer.");
            return;
        }

        WriteResponse response = _processor.DeleteTodo(todoId);
        ShowWriteResponse(response);
    }

    private void HandleUpdate(string[] arguments)
    {
        if (arguments.Length < 2)
        {
            WriteLine("id and new description must be provided in order to update todo.");
            return;
        }

        if (!int.TryParse(arguments[0], out int todoId))
        {
            WriteLine("invalid todo id. id must be an non-negative integer.");
            return;
        }

        string description = arguments[1];

        WriteResponse response = _processor.UpdateTodo(todoId, description);
        ShowWriteResponse(response);
    }

    public void HandleChangeStatus(string command, string[] arguments)
    {
        if (arguments.Length < 1)
        {
            WriteLine("you have to provide todo id.");
            return;
        }

        if (!int.TryParse(arguments[0], out int todoId))
        {
            WriteLine("invalid todo id. id must be an non-negative integer.");
            return;
        }

        WriteResponse response = _processor.ChangeTodoStatus(todoId, command);
        ShowWriteResponse(response);
    }

}