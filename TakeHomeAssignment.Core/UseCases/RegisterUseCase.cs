using System.Net.Http.Json;
using System.Text.Json;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.Gateways.Interfaces;
using TakeHomeAssignment.Presenters.Interfaces;
using TakeHomeAssignment.UseCases.Interfaces;

namespace TakeHomeAssignment.UseCases
{
    public class RegisterUseCase : IRegisterUseCase
    {
        private ISendRegisterRequestGateway _sendRegisterRequestGateway;
        private IRegisterPresenter _registerPresenter;
        private IErrorPresenter _errorPresenter;

        public RegisterUseCase(ISendRegisterRequestGateway sendRegisterRequestGateway, IRegisterPresenter registerPresenter, IErrorPresenter errorPresenter)
        {
            _sendRegisterRequestGateway = sendRegisterRequestGateway;
            _registerPresenter = registerPresenter;
            _errorPresenter = errorPresenter;
        }

        public async Task Execute()
        {
            try
            {
                var response = await _sendRegisterRequestGateway.ExecuteAsync();
                var isSuccess = response.IsSuccessStatusCode;

                if (isSuccess)
                {
                    var raw = await response.Content.ReadAsStringAsync();
                    var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                    if (registerResponse != null)
                    {
                        _registerPresenter.Present(new RegisterResultMessage
                        {
                            Id = registerResponse.UserId
                        });
                    }
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _errorPresenter.Present(
                        string.IsNullOrWhiteSpace(errorMessage) ?
                        "Encountered Unknown Error" :
                        errorMessage);
                }
            }
            catch (JsonException) 
            {
                _errorPresenter.Present("User ID was not an int.");
            }
            catch (TaskCanceledException) 
            {
                _errorPresenter.Present("The server request timed out.");
            }
        }
    }
}
