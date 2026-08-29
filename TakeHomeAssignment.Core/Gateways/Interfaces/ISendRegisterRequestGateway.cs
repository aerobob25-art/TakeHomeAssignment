namespace TakeHomeAssignment.Gateways.Interfaces
{
    public interface ISendRegisterRequestGateway
    {
        Task<HttpResponseMessage> ExecuteAsync();
    }
}
