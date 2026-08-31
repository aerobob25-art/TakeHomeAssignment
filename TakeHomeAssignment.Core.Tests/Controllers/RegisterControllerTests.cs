using Moq;
using TakeHomeAssignment.Core.Controllers;
using TakeHomeAssignment.Core.UseCases.Interfaces;

namespace TakeHomeAssignment.Tests.Controllers
{
    [TestFixture]
    public class RegisterControllerTests
    {
        private RegisterController _controller;
        private Mock<IRegisterUseCase> _useCase;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _useCase = new Mock<IRegisterUseCase>();
            _controller = new RegisterController(_useCase.Object);
        }

        [Test]
        public void Controller_Executes_CallsUseCase()
        {
            _controller.Execute(default);
            _useCase.Verify(m => m.Execute(default), Times.Once);
        }
    }
}
