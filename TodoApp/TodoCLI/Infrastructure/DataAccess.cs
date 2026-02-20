using System.Text.Json;
using System.Text.Json.Serialization;
using TodoCLI.Infrastructure.Contracts;
using TodoCLI.Models;
using static TodoCLI.Logger.Logger;

namespace TodoCLI.Infrastructure;

public class DataAccess : IDataAccess
{
    private const string FilePath = "data.json";
    private JsonSerializerOptions Options { get; } = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() },
        WriteIndented = true
    };

    private DataFile ReadDataFile()
    {
        if (!File.Exists(FilePath))
        {
            throw new FileNotFoundException("data file not found");
        }

        string json = File.ReadAllText(FilePath);

        DataFile? data = JsonSerializer.Deserialize<DataFile>(json, Options);
        
        if (data == null)
        {
            throw new JsonException("Failed to read data.");
        }

        return data;
    }
    
    private void WriteDataFile(DataFile data)
    {
        string json = JsonSerializer.Serialize(data, Options);

        string tempFilePath = FilePath + ".tmp";

        File.WriteAllText(tempFilePath, json);

        File.Move(tempFilePath, FilePath, overwrite: true);
    }

    public List<Todo> GetAllTodos()
    {
        try
        {
            DataFile data = ReadDataFile();
            List<Todo> todos = data.Todos.ToList();
            return todos;
        }
        catch (Exception ex)
        {
            Log(ex, $"failed to get todos list.");
            throw;
        }
    }

    public void CreateTodo(string description)
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                var emptyData = new DataFile
                {
                    AvailableId = 1,
                    Todos = new List<Todo>()
                };

                WriteDataFile(emptyData);
            }

            DataFile data = ReadDataFile();

            var newTodo = new Todo
            {
                Id = data.AvailableId,
                Description = description,
                Status = Status.Todo,
                CreatedAt = DateTimeOffset.Now,
                UpdatedAt = DateTimeOffset.Now
            };

            data.Todos.Add(newTodo);
            data.AvailableId++;

            WriteDataFile(data);
        }
        catch(Exception ex)
        {
            Log(ex, $"failed to create todo with description of {description}");
            throw;
        }
    }

    public Todo? GetTodo(int id)
    {
        try
        {
            DataFile data = ReadDataFile();

            Todo? targetTodo = data.Todos.FirstOrDefault(t => t.Id == id);

            return targetTodo;
        }
        catch(Exception ex)
        {
            Log(ex, $"failed to get todo with id of {id}.");
            throw;
        }
    }

    public int? DeleteTodo(int id)
    {
        try
        {
            DataFile data = ReadDataFile();

            Todo? targetTodo = data.Todos.FirstOrDefault(t => t.Id == id);

            if (targetTodo == null)
            {
                return null;
            }

            data.Todos.Remove(targetTodo);

            WriteDataFile(data);

            return targetTodo.Id;
        }
        catch(Exception ex)
        {
            Log(ex, $"failed to delete todo with id of {id}.");
            throw;
        }
    }

    public int? UpdateTodo(int id, string description)
    {
        try
        {
            DataFile data = ReadDataFile();

            Todo? targetTodo = data.Todos.FirstOrDefault(t => t.Id == id);


            if (targetTodo == null)
            {
                return null;
            }

            targetTodo.Description = description;
            targetTodo.UpdatedAt = DateTimeOffset.Now;

            WriteDataFile(data);

            return targetTodo.Id;
        }
        catch(Exception ex)
        {
            Log(ex, $"failed to update todo with id of {id}.");
            throw;
        }
    }

    public int? ChangeTodoStatus(int id, Status newStatus)
    {
        try
        {
            DataFile data = ReadDataFile();

            Todo? targetTodo = data.Todos.FirstOrDefault(t => t.Id == id);

            if (targetTodo == null)
            {
                return null;
            }

            targetTodo.Status = newStatus;
            targetTodo.UpdatedAt = DateTimeOffset.Now;

            WriteDataFile(data);

            return targetTodo.Id;
        }
        catch(Exception ex)
        {
            Log(ex, $"failed to change todo status with id of {id}.");
            throw;
        }
    }
}