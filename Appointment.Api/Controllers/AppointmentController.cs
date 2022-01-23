using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using PatientNavigation.Common.KakfaHelpers;

namespace Appointment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        private readonly EventsHelper _eventsHelper;
        private readonly string _topicName;

        public AppointmentController(ILogger<AppointmentController> logger, IConfiguration configuration)
        {
            _logger = logger;
            var boostrapServer = configuration.GetValue<string>("KafkaConfig:Servers");
            _eventsHelper = new EventsHelper(boostrapServer);

            _topicName = configuration.GetValue<string>("KafkaConfig:TopicName");
        }

        [HttpPost]
        public async Task<ObjectResult> Post([FromBody] Hl7.Fhir.Model.Appointment appointment)
        {
            appointment.Id = Guid.NewGuid().ToString();
            await _eventsHelper.Produce(_topicName, appointment.ToJson());
            return Ok(new {Id = appointment.Id});
        }

        [HttpPut("{appointmentId}")]
        public async Task<ObjectResult> Put([FromRoute] string appointmentId, [FromBody] Hl7.Fhir.Model.Appointment appointment)
        {
            await _eventsHelper.Produce(_topicName, appointment.ToJson());
            return Ok("");
        }
    }
}