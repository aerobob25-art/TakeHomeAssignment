using System.Net;
using TakeHomeAssignment.Core.Gateways.Interfaces;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.Core.UseCases.Interfaces;

namespace TakeHomeAssignment.Core.UseCases
{
    public class LogInUseCase : ILogInUseCase
    {
        private ISendLogInRequestGateway _sendLogInRequestGateway;
        private ILogInPresenter _logInPresenter;
        private IErrorPresenter _errorPresenter;

        public LogInUseCase(ISendLogInRequestGateway sendLogInRequestGateway, ILogInPresenter logInPresenter, IErrorPresenter errorPresenter)
        {
            _sendLogInRequestGateway = sendLogInRequestGateway;
            _logInPresenter = logInPresenter;
            _errorPresenter = errorPresenter;
        }

        public async Task Execute(long userId)
        {
            try
            {
                var response = await _sendLogInRequestGateway.ExecuteAsync(userId);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        _logInPresenter.Present(new LogInResultMessage()
                        {
                            ClientState = ClientStates.LoggedIn
                        });
                        break;
                    case HttpStatusCode.NotFound:
                        _errorPresenter.Present(new ErrorMessage()
                        {
                            Error = "User ID not found",
                            ClientState = ClientStates.Unregistered
                        });
                        break;
                    case HttpStatusCode.BadRequest:
                        _errorPresenter.Present(new ErrorMessage()
                        {
                            Error = "Invalid User ID.",
                            ClientState = ClientStates.NoChange
                        });
                        break;
                    case HttpStatusCode.InternalServerError:
                        _errorPresenter.Present(new ErrorMessage()
                        {
                            Error = "Login Failed.",
                            ClientState = ClientStates.NoChange
                        });
                        break;
                    default:
                        _errorPresenter.Present(new ErrorMessage()
                        {
                            Error = "Unknown Error Occurred.",
                            ClientState = ClientStates.NoChange
                        });
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                _errorPresenter.Present(new ErrorMessage()
                {
                    Error = "The server request timed out.",
                    ClientState = ClientStates.NoChange
                });
            }
            catch (HttpRequestException)
            {
                _errorPresenter.Present(new ErrorMessage()
                {
                    Error = "Unknown error has occurred.",
                    ClientState = ClientStates.NoChange
                });
            }
        }
    }
}
