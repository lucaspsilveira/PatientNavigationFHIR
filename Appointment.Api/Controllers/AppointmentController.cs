using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Mvc;

namespace Appointment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly FhirClient _client;

        public AppointmentController(ILogger<AppointmentController> logger)
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
        public async Task<ObjectResult> Post([FromBody] Hl7.Fhir.Model.Appointment appointment)
        {
            var response = await _client.CreateAsync(appointment);
            return Ok(response);
        }
    }
}