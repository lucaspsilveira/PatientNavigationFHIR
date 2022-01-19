using Confluent.Kafka;

namespace Medications.Consumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMedicationService _medicationService;
        private readonly string _medicationTopicName;
        private readonly string _medicationStatementTopicName;
        private readonly IMedicationStatementService _medicationStatementService;
        private readonly string[] _topicNames = new string[2];

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IMedicationService medicationService, IMedicationStatementService medicationStatementService)
        {
            _logger = logger;
            _configuration = configuration;
            _medicationService = medicationService;
            _medicationTopicName = _configuration.GetValue<string>("KafkaConfig:MedicationTopicName");
            _medicationStatementTopicName = _configuration.GetValue<string>("KafkaConfig:MedicationStatementTopicName");
            _medicationStatementService = medicationStatementService;
            _topicNames[0] = _medicationTopicName;
            _topicNames[1] = _medicationStatementTopicName;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration.GetValue<string>("KafkaConfig:Servers"),
                GroupId = "medications",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            while (!stoppingToken.IsCancellationRequested)
            {
                using var consumer = new ConsumerBuilder<Null, string>(config).Build();
                consumer.Subscribe(_topicNames);

                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    _logger.LogInformation($"Consumed event: {consumeResult?.Message.Value}");
                    
                    if (consumeResult?.Message?.Value is null)
                        continue;
                    
                    if (string.Equals(consumeResult.Topic, _medicationTopicName))
                        await _medicationService.InsertMedication(consumeResult.Message.Value);
                    if (string.Equals(consumeResult.Topic, _medicationStatementTopicName))
                        await _medicationStatementService.InsertMedicationStatement(consumeResult.Message.Value);
                }

                consumer.Close();
                await Task.Delay(1000);
            }
        }
    }
}