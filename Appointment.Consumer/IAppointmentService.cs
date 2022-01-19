
namespace Appointment.Consumer
{
    public interface IAppointmentService
    {
        Task InsertAppointment(string appointmentPayload);
    }
}