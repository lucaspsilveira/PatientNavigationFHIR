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
            //var result = await _client.CreateAsync(patient).ConfigureAwait(false);
            await _eventsHelper.Produce(_topicName, patient.ToJson());
            return Ok("");
        }
    }
}