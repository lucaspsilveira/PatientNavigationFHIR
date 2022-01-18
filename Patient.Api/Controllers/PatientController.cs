using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly FhirClient _client;

        public PatientController(ILogger<PatientController> logger)
        {
            _logger = logger;
            var settings = new FhirClientSettings
            {
                Timeout = 30,
                PreferredFormat = ResourceFormat.Json,
                VerifyFhirVersion = true,
                PreferredReturn = Prefer.RespondAsync
            };

            _client = new FhirClient("http://hapi.fhir.org/baseR4", settings);
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody] Hl7.Fhir.Model.Patient patient)
        {
            var result = await _client.CreateAsync(patient).ConfigureAwait(false);
            return Ok(result.ToJson());
        }
    }
}