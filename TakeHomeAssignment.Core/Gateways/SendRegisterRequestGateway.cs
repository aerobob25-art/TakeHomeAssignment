using TakeHomeAssignment.Core.Gateways.Interfaces;

namespace TakeHomeAssignment.Core.Gateways
{
    public class SendRegisterRequestGateway : ISendRegisterRequestGateway
    {
        private HttpClient _httpClient;

        public SendRegisterRequestGateway(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> ExecuteAsync()
        {
            return await _httpClient.PostAsync("/register", null);
        }
    }
}
