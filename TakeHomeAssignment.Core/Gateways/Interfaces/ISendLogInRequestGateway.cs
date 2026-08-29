namespace TakeHomeAssignment.Core.Gateways.Interfaces
{
    public interface ISendLogInRequestGateway
    {
        Task<HttpResponseMessage> ExecuteAsync(long userId);
    }
}
