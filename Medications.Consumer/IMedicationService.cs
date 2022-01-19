namespace Medications.Consumer
{
    public interface IMedicationService
    {
        Task InsertMedication(string medicationPayload);
    }
}