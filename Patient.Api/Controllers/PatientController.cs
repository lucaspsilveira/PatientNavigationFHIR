using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using PatientNavigation.Common.KakfaHelpers;

namespace Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly string _topicName;
        private readonly EventsHelper _eventsHelper;

        public PatientController(ILogger<PatientController> logger, IConfiguration configuration)
        {
            _logger = logger;

            var boostrapServer = configuration.GetValue<string>("KafkaConfig:Servers");
            _eventsHelper = new EventsHelper(boostrapServer);

            _topicName = configuration.GetValue<string>("KafkaConfig:TopicName");
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody] Hl7.Fhir.Model.Patient patient)
        {
            patient.Id = Guid.NewGuid().ToString();
            await _eventsHelper.Produce(_topicName, patient.ToJson());
            return Ok(new { Id = patient.Id });
        }

        [HttpPut("{patientId}")]
        public async Task<ObjectResult> PostAsync([FromRoute] int patientId, [FromBody] Hl7.Fhir.Model.Patient patient)
        {
            await _eventsHelper.Produce(_topicName, patient.ToJson());
            return Ok("");
        }
    }
}