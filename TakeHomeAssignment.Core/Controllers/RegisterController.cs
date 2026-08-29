using TakeHomeAssignment.Controllers.Interfaces;
using TakeHomeAssignment.UseCases;
using TakeHomeAssignment.UseCases.Interfaces;

namespace TakeHomeAssignment.Controllers
{
    public class RegisterController : IRegisterController
    {
        private IRegisterUseCase _registerUseCase;

        public RegisterController(IRegisterUseCase registerUseCase)
        {
            _registerUseCase = registerUseCase;
        }

        public void Execute()
        {
            _registerUseCase.Execute();
        }
    }
}
