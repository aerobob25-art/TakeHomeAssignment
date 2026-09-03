namespace TakeHomeAssignment.Core.Gateways.Interfaces
{
    public interface ISendRegisterRequestGateway
    {
        Task<HttpResponseMessage> ExecuteAsync();
    }
}
