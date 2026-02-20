
namespace TodoCLI.Models;


public class Todo
{
    public int Id { get; set; }
    public string Description { get; set; }
    public Status Status { get; set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; set; }
}