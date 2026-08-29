using TakeHomeAssignment.Controllers.Interfaces;
using TakeHomeAssignment.UseCases.Interfaces;

namespace TakeHomeAssignment.Controllers
{
    public class LogInController : ILogInController
    {
        private ILogInUseCase _logInUseCase;

        public LogInController(ILogInUseCase logInUseCase)
        {
            _logInUseCase = logInUseCase;
        }

        public void Execute(int userId)
        {
            _logInUseCase.Execute(userId);
        }
    }
}
