using Kysect.Tamgly.ConsoleClient.Commands;
using Spectre.Console.Cli;

namespace Kysect.Tamgly.ConsoleClient;

public class ProofOfConcept
{
    public void Execute()
    {
        var app = new CommandApp();
        app.Configure(config =>
        {
            config.AddCommand<CreateWorkItemCommand>("create-wi");
        });

        while (true)
        {
            string? readLine = Console.ReadLine() ?? throw new ArgumentException();
            app.Run(readLine.Split());
        }
    }
}