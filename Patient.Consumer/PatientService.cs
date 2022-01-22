﻿using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace Patient.Consumer
{
    public class PatientService : IPatientService
    {
        private readonly FhirClient _client;

        private ILogger<PatientService> _logger { get; }
        public PatientService(ILogger<PatientService> logger, IConfiguration configuration)
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

        public async Task InsertPatient(string patientPayload)
        {
            var parser = new FhirJsonParser();
            var resource = parser.Parse<Hl7.Fhir.Model.Patient>(patientPayload);
            var result = await _client.UpdateAsync(resource).ConfigureAwait(false);
            if (result != null)
                _logger.LogInformation($"Patient created/updated on FHIR Server with Id: {result.Id}");
            else
                _logger.LogError($"Patient not created/updated on FHIR Server.");
        }

    }
}