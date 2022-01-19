
namespace Procedure.Consumer
{
    public interface IProcedureService
    {
        Task InsertProcedure(string procedurePayload);
    }
}