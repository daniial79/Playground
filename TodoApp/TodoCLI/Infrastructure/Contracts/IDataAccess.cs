using TodoCLI.Models;

namespace TodoCLI.Infrastructure.Contracts;


public interface IDataAccess
{
    public List<Todo> GetAllTodos();
    public void CreateTodo(string description);
    public Todo? GetTodo(int id);
    public int? DeleteTodo(int id);
    public int? UpdateTodo(int id, string description);
    public int? ChangeTodoStatus(int id, Status newStatus);
}