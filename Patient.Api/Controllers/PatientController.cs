using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using PatientNavigation.Common.KakfaHelpers;
using PatientNavigation.Common.Repositories;

namespace Patient.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly ILogger<PatientController> _logger;
        private readonly IPatientRepository _patientRepository;
        private readonly string _topicName;
        private readonly EventsHelper _eventsHelper;

        public PatientController(ILogger<PatientController> logger, IConfiguration configuration, IPatientRepository patientRepository)
        {
            _logger = logger;
            var boostrapServer = configuration.GetValue<string>("KafkaConfig:Servers");
            _eventsHelper = new EventsHelper(boostrapServer);

            _patientRepository = patientRepository;
            _topicName = configuration.GetValue<string>("KafkaConfig:TopicName");
        }

        [HttpPost]
        public async Task<ObjectResult> PostAsync([FromBody] Hl7.Fhir.Model.Patient patient)
        {
            patient.Id = Guid.NewGuid().ToString();
            var result = _patientRepository.Create(new PatientNavigation.Common.Models.PatientResource
            {
                Patient = patient,
                LastUpdated = DateTime.UtcNow,
                Status = "PENDING"
            });

            await _eventsHelper.Produce(_topicName, patient.ToJson());
            return Ok(new { Id = patient.Id });
        }

        [HttpPut("{patientId}")]
        public async Task<ObjectResult> PostAsync([FromRoute] string patientId, [FromBody] Hl7.Fhir.Model.Patient patient)
        {
            await _eventsHelper.Produce(_topicName, patient.ToJson());
            return Ok("");
        }

        [HttpGet("{patientId}")]
        public ObjectResult GetAsync([FromRoute] string patientId)
        {
            var result = _patientRepository.Get(patientId);
            return Ok(result.Patient.ToJson());
        }
    }
}