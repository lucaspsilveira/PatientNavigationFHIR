using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using PatientNavigation.Common.KakfaHelpers;

namespace Medications.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MedicationController : ControllerBase
    {
        private readonly ILogger<MedicationController> _logger;
        private readonly EventsHelper _eventsHelper;
        private readonly string _topicName;

        public MedicationController(ILogger<MedicationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            var boostrapServer = configuration.GetValue<string>("KafkaConfig:Servers");
            _eventsHelper = new EventsHelper(boostrapServer);

            _topicName = configuration.GetValue<string>("KafkaConfig:MedicationTopicName");
        }

        [HttpPost]
        public async Task<ObjectResult> Post(string medication)
        {
            var parser = new FhirJsonParser();
            var resource = parser.Parse<Hl7.Fhir.Model.Medication>(medication);
            await _eventsHelper.Produce(_topicName, resource.ToJson());
            return Ok("");
        }
    }
}
