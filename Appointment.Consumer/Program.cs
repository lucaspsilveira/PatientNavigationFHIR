using Appointment.Consumer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddTransient<IAppointmentService, AppointmentService>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
