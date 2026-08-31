using Moq;
using TakeHomeAssignment.Core.Controllers;
using TakeHomeAssignment.Core.UseCases.Interfaces;

namespace TakeHomeAssignment.Tests.Controllers
{
    [TestFixture]
    public class LogInControllerTests
    {
        private LogInController _controller;
        private Mock<ILogInUseCase> _mockUseCase;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _mockUseCase = new Mock<ILogInUseCase>();
            _controller = new LogInController(_mockUseCase.Object);
        }

        [Test]
        public void Controller_Executes_CallsUseCase()
        {
            _controller.Execute(1);
            _mockUseCase.Verify(m => m.Execute(1), Times.Once());
        }
    }
}
