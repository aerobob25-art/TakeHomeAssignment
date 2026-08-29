using TakeHomeAssignment.Gateways.Interfaces;

namespace TakeHomeAssignment.Gateways
{
    public class SendLogInRequestGateway : ISendLogInRequestGateway
    {
        private HttpClient _httpClient;

        public SendLogInRequestGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(long userId)
        {
            return await _httpClient.PostAsync($"/login?user_id={userId}", null);
        }
    }
}
