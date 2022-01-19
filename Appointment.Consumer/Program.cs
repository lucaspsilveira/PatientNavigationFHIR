using Appointment.Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IAppointmentService, AppointmentService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
