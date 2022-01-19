using Patient.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IPatientService, PatientService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
