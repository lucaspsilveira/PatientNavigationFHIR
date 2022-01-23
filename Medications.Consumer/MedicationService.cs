using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using PatientNavigation.Common.Models;
using PatientNavigation.Common.Repositories;

namespace Medications.Consumer
{
    public class MedicationService : IMedicationService
    {
        private FhirClient _client;
        private ILogger<MedicationService> _logger;
        private readonly IMedicationRepository _medicationRepository;

        public MedicationService(
            ILogger<MedicationService> logger,
            IConfiguration configuration,
            IMedicationRepository medicationRepository)
            
        {
            _logger = logger;
            _medicationRepository = medicationRepository;
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
            var result = await _client.UpdateAsync(resource).ConfigureAwait(false);
            var medicationResource = new MedicationResource {
                Medication = resource,
                LastUpdated = DateTime.UtcNow
            };
            if (result != null)
            {
                _logger.LogInformation($"Medication created/updated on FHIR Server with Id: {result.Id}");
                medicationResource.Status = "CREATED";
            }
            else
            {
                _logger.LogError($"Medication not created/updated on FHIR Server.");
                medicationResource.Status = "FAILED";
            }
            _medicationRepository.Update(resource.Id, medicationResource);
        }
    }
}
