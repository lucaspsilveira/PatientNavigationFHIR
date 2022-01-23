using Hl7.Fhir.Serialization;
using Microsoft.AspNetCore.Mvc;
using PatientNavigation.Common.KakfaHelpers;

namespace Procedure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProcedureController : ControllerBase
    {
        private readonly ILogger<ProcedureController> _logger;
        private readonly EventsHelper _eventsHelper;
        private readonly string _topicName;

        public ProcedureController(ILogger<ProcedureController> logger, IConfiguration configuration)
        {
            _logger = logger;

            var boostrapServer = configuration.GetValue<string>("KafkaConfig:Servers");
            _eventsHelper = new EventsHelper(boostrapServer);

            _topicName = configuration.GetValue<string>("KafkaConfig:TopicName");
        }

        [HttpPost]
        public async Task<ObjectResult> Post([FromBody] Hl7.Fhir.Model.Procedure procedure)
        {
            procedure.Id = Guid.NewGuid().ToString();
            await _eventsHelper.Produce(_topicName, procedure.ToJson());
            return Ok(new {Id = procedure.Id });
        }

        [HttpPut("{procedureId}")]
        public async Task<ObjectResult> Put([FromRoute] string procedureId, [FromBody] Hl7.Fhir.Model.Procedure procedure)
        {
            await _eventsHelper.Produce(_topicName, procedure.ToJson());
            return Ok("");
        }
    }
}