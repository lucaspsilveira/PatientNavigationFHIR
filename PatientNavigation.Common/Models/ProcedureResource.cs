using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PatientNavigation.Common.Models
{
    public class ProcedureResource
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public Hl7.Fhir.Model.Procedure? Procedure { get; set; }
        public string? Status { get; set; }
        public DateTime? LastUpdated { get; set; }
    }
}