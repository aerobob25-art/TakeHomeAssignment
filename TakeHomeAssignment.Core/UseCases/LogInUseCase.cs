using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Gateways.Interfaces;
using TakeHomeAssignment.Presenters.Interfaces;
using TakeHomeAssignment.UseCases.Interfaces;

namespace TakeHomeAssignment.UseCases
{
    public class LogInUseCase : ILogInUseCase
    {
        private ISendLogInRequestGateway _sendLogInRequestGateway;
        private ILogInPresenter _logInPresenter;

        public LogInUseCase(ISendLogInRequestGateway sendLogInRequestGateway, ILogInPresenter logInPresenter)
        {
            _sendLogInRequestGateway = sendLogInRequestGateway;
            _logInPresenter = logInPresenter;
        }

        public void Execute(int userId)
        {
            try
            {
                var response = _sendLogInRequestGateway.ExecuteAsync(userId);
                //Do checking here
                _logInPresenter.Present(new LogInResultMessage()
                {

                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
