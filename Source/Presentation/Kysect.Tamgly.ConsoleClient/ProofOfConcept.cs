using Kysect.Tamgly.ConsoleClient.Commands;
using Kysect.Tamgly.ConsoleClient.Infrastructure;
using Kysect.Tamgly.Core;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Kysect.Tamgly.ConsoleClient;

public class ProofOfConcept
{
    public void Execute()
    {
        var workItemManager = new WorkItemManager();

        var registrations = new ServiceCollection();
        registrations.AddSingleton(workItemManager);

        var registrar = new TypeRegistrar(registrations);
        var app = new CommandApp(registrar);
        app.Configure(config =>
        {
            config.AddCommand<CreateWorkItemCommand>("create-wi");
            config.AddCommand<GetWorkItemCommand>("get-wi");
        });

        while (true)
        {
            string readLine = Console.ReadLine() ?? throw new ArgumentException();
            app.Run(readLine.Split());
        }
    }
}