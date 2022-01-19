using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using PatientNavigation.Common.KakfaHelpers;
using System.Configuration;

namespace Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly FhirClient _client;
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
            await _eventsHelper.Produce(_topicName, patient.ToJson());
            return Ok("");
        }
    }
}