using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using PatientNavigation.Common.KakfaHelpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
JsonConvert.DefaultSettings = (() =>
{
    var settings = new JsonSerializerSettings();
    settings.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
    return settings;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var bootstrapServer = builder.Configuration.GetValue<string>("KafkaConfig:Servers");
var medicationTopicName = builder.Configuration.GetValue<string>("KafkaConfig:MedicationTopicName");
var medicationStatementTopicName = builder.Configuration.GetValue<string>("KafkaConfig:MedicationStatementTopicName");

await AdminKafkaHelper.CreateTopicAsync(bootstrapServer, medicationTopicName).ConfigureAwait(false);
await AdminKafkaHelper.CreateTopicAsync(bootstrapServer, medicationStatementTopicName).ConfigureAwait(false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
