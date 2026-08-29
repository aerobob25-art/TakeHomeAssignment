using System.Net.Http.Json;
using System.Text.Json;
using TakeHomeAssignment.Core.Gateways.Interfaces;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.Core.UseCases.Interfaces;

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
                    var registerResponse = await response.Content.ReadFromJsonAsync<RegisterResponse>();
                    if (registerResponse == null)
                    {
                        throw new HttpRequestException();
                    }

                    _registerPresenter.Present(new RegisterResultMessage
                    {
                        Id = registerResponse.UserId,
                        ClientState = ClientStates.Registered
                    });
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    _errorPresenter.Present(new ErrorMessage()
                    {
                        Error = string.IsNullOrWhiteSpace(errorMessage) ? "Encountered Unknown Error" : errorMessage,
                        ClientState = ClientStates.Unregistered
                    });
                }
            }
            catch (JsonException) 
            {
                _errorPresenter.Present(new ErrorMessage()
                {
                    Error = "User ID was not valid.",
                    ClientState = ClientStates.Unregistered
                });
            }
            catch (TaskCanceledException) 
            {
                _errorPresenter.Present(new ErrorMessage() 
                {
                    Error = "The server request timed out.",
                    ClientState = ClientStates.Unregistered
                });
            }
            catch (HttpRequestException)
            {
                _errorPresenter.Present(new ErrorMessage()
                {
                    Error = "Unknown error has occurred.",
                    ClientState = ClientStates.Unregistered
                });
            }
        }
    }
}
