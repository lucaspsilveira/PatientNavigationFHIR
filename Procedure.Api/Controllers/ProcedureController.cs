using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace Procedure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcedureController : ControllerBase
    {
        private readonly ILogger<ProcedureController> _logger;
        private readonly FhirClient _client;

        public ProcedureController(ILogger<ProcedureController> logger)
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
        public async Task<ObjectResult> Post([FromBody] Hl7.Fhir.Model.Procedure procedure)
        {
            var response = await _client.CreateAsync(procedure);
            return Ok(response.ToJson());
        }

    }
}