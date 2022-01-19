using Procedure.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IProcedureService, ProcedureService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
