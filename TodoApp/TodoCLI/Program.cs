using TodoCLI.Application;
using TodoCLI.Infrastructure;
using TodoCLI.Infrastructure.Contracts;

namespace TodoCLI;

class Program
{
    static void Main(string[] args)
    {
        // Create and Inject Depedencies
        IDataAccess dataAccess = new DataAccess();
        Processor processor = new(dataAccess);
        UI ui = new UI(processor);

        ui.Run(args);
    }
}
