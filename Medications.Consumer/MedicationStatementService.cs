using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace Medications.Consumer
{
    public class MedicationStatementService : IMedicationStatementService
    {
        private FhirClient _client;
        private ILogger<MedicationStatementService> _logger;

        public MedicationStatementService(ILogger<MedicationStatementService> logger, IConfiguration configuration)
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

        public async Task InsertMedicationStatement(string medicationStatementPayload)
        {
            var parser = new FhirJsonParser();
            var resource = parser.Parse<Hl7.Fhir.Model.MedicationStatement>(medicationStatementPayload);
            var result = await _client.CreateAsync(resource).ConfigureAwait(false);
            if (result != null)
                _logger.LogInformation($"Medication Statement created on FHIR Server with Id: {result.Id}");
            else
                _logger.LogError($"Medication Statement not created on FHIR Server.");
        }
    }
}
