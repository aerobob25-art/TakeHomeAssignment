using TakeHomeAssignment.Core.Controllers.Interfaces;
using TakeHomeAssignment.Core.UseCases.Interfaces;

namespace TakeHomeAssignment.Core.Controllers
{
    public class RegisterController : IRegisterController
    {
        private IRegisterUseCase _registerUseCase;

        public RegisterController(IRegisterUseCase registerUseCase)
        {
            _registerUseCase = registerUseCase;
        }

        public void Execute(CancellationToken cancellation)
        {
            _registerUseCase.Execute(cancellation);
        }
    }
}
