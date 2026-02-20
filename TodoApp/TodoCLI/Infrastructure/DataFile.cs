using TodoCLI.Models;

namespace TodoCLI.Infrastructure;


public class DataFile
{
    public int AvailableId { get; set; }
    public List<Todo> Todos { get; set; } = new List<Todo>();
}