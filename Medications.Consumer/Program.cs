using Medications.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IMedicationService, MedicationService>();
        services.AddTransient<IMedicationStatementService, MedicationStatementService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
