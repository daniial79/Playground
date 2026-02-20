using TodoCLI.Models;

namespace TodoCLI.Application.Responses;


public class ReadResponse : Response
{
    public List<Todo> Todos { get; set; }
}