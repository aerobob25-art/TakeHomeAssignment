using System.Net.Http.Json;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.Gateways.Interfaces;
using TakeHomeAssignment.Presenters.Interfaces;
using TakeHomeAssignment.UseCases.Interfaces;
using System.Net;

namespace TakeHomeAssignment.UseCases
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
                        _logInPresenter.Present(new LogInResultMessage());
                        break;
                    case HttpStatusCode.NotFound:
                        _errorPresenter.Present("User not found.");
                        break;
                    case HttpStatusCode.BadRequest:
                        _errorPresenter.Present("Invalid User ID.");
                        break;
                    case HttpStatusCode.InternalServerError:
                        _errorPresenter.Present("Login Failed.");
                        break;
                    default:
                        _errorPresenter.Present("Unknown Error Occurred.");
                        break;
                }
            }
            catch (TaskCanceledException)
            {
                _errorPresenter.Present("The server request timed out.");
            }
        }
    }
}
