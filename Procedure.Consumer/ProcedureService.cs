using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace Procedure.Consumer
{
    public class ProcedureService : IProcedureService
    {
        private readonly ILogger<ProcedureService> _logger;
        private readonly FhirClient _client;

        public ProcedureService(ILogger<ProcedureService> logger, IConfiguration configuration)
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

        public async Task InsertProcedure(string procedurePayload)
        {
            var parser = new FhirJsonParser();
            var resource = parser.Parse<Hl7.Fhir.Model.Procedure>(procedurePayload);
            var result = await _client.UpdateAsync(resource).ConfigureAwait(false);
            if (result != null)
                _logger.LogInformation($"Procedure created/updated on FHIR Server with Id: {result.Id}");
            else
                _logger.LogError($"Procedure not created/updated on FHIR Server.");
        }
    }
}
