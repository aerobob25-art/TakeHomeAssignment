using Moq;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Gateways.Interfaces;
using TakeHomeAssignment.Presenters.Interfaces;
using TakeHomeAssignment.UseCases;

namespace TakeHomeAssignment.Tests.UseCases
{
    [TestFixture]
    public class LogInUseCaseTests
    {
        private LogInUseCase _useCase;
        private Mock<ISendLogInRequestGateway> _gateway;
        private Mock<ILogInPresenter> _presenter;

        [SetUp]
        public void SetUp()
        {
            _gateway = new Mock<ISendLogInRequestGateway>();
            _presenter = new Mock<ILogInPresenter>();
            _useCase = new LogInUseCase(_gateway.Object, _presenter.Object);
        }

        [Test]
        public void UseCase_Executes_SendsLogInRequest()
        {
            _useCase.Execute(1);
            _gateway.Verify(m => m.ExecuteAsync(1), Times.Once());
        }

        [Test]
        public void UseCase_Executes_PresentsResults()
        {
            _useCase.Execute(1);
            _presenter.Verify(m => m.Present(It.IsAny<LogInResultMessage>()), Times.Once());
        }

    }
}
