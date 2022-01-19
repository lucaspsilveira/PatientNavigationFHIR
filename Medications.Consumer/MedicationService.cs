using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace Medications.Consumer
{
    public class MedicationService : IMedicationService
    {
        private FhirClient _client;
        private ILogger<MedicationService> _logger;

        public MedicationService(ILogger<MedicationService> logger, IConfiguration configuration)
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

        public async Task InsertMedication(string medicationPayload)
        {
            var parser = new FhirJsonParser();
            var resource = parser.Parse<Hl7.Fhir.Model.Medication>(medicationPayload);
            var result = await _client.CreateAsync(resource).ConfigureAwait(false);
            if (result != null)
                _logger.LogInformation($"Medication  created on FHIR Server with Id: {result.Id}");
            else
                _logger.LogError($"Medication  not created on FHIR Server.");
        }
    }
}
