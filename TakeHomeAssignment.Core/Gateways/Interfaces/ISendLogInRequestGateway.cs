namespace TakeHomeAssignment.Gateways.Interfaces
{
    public interface ISendLogInRequestGateway
    {
        Task<HttpResponseMessage> ExecuteAsync(long userId);
    }
}
