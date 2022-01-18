using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Medications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicationController : ControllerBase
    {
        private readonly ILogger<MedicationController> _logger;
        private readonly FhirClient _client;

        public MedicationController(ILogger<MedicationController> logger)
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
        public async Task<ObjectResult> Post(string medication)
        {
            var parser = new FhirJsonParser();
            var resource = parser.Parse<Hl7.Fhir.Model.Medication>(medication);
            var response = await _client.CreateAsync(resource);
            return Ok(response.ToJson());
        }



    }
}
