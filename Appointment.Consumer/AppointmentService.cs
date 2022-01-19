using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace Appointment.Consumer
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<AppointmentService> _logger;
        private readonly FhirClient _client;

        public AppointmentService(ILogger<AppointmentService> logger, IConfiguration configuration)
        {
            _logger = logger;
            var settings = new FhirClientSettings
            {
                Timeout = 30,
                PreferredFormat = ResourceFormat.Json,
                VerifyFhirVersion = true,
                PreferredReturn = Prefer.RespondAsync
            };

            _client = new FhirClient(configuration.GetValue<string>("FhirServerUrl"), settings);
        }

        public async Task InsertAppointment(string appointmentPayload)
        {
            var parser = new FhirJsonParser(new ParserSettings
            {
                AllowUnrecognizedEnums = true
            });
            var resource = parser.Parse<Hl7.Fhir.Model.Appointment>(appointmentPayload);
            var result = await _client.CreateAsync(resource).ConfigureAwait(false);
            if (result != null)
                _logger.LogInformation($"Appointment created on FHIR Server with Id: {result.Id}");
            else
                _logger.LogError($"Appointment not created on FHIR Server.");
        }
    }
}
