using TakeHomeAssignment.Core.Controllers.Interfaces;
using TakeHomeAssignment.Core.UseCases.Interfaces;

namespace TakeHomeAssignment.Core.Controllers
{
    public class LogInController : ILogInController
    {
        private ILogInUseCase _logInUseCase;

        public LogInController(ILogInUseCase logInUseCase)
        {
            _logInUseCase = logInUseCase;
        }

        public void Execute(long userId)
        {
            _logInUseCase.Execute(userId);
        }
    }
}
