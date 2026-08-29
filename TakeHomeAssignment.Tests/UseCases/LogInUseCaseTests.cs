using Moq;
using System.Net;
using TakeHomeAssignment.Core.Gateways.Interfaces;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.Core.UseCases;

namespace TakeHomeAssignment.Tests.UseCases
{
    [TestFixture]
    public class LogInUseCaseTests
    {
        private LogInUseCase _useCase;
        private Mock<ISendLogInRequestGateway> _gateway;
        private Mock<ILogInPresenter> _presenter;
        private Mock<IErrorPresenter> _errorPresenter;

        [SetUp]
        public void SetUp()
        {
            _gateway = new Mock<ISendLogInRequestGateway>();
            _presenter = new Mock<ILogInPresenter>();
            _errorPresenter = new Mock<IErrorPresenter>();
            _useCase = new LogInUseCase(
                _gateway.Object,
                _presenter.Object,
                _errorPresenter.Object);
        }

        [Test]
        public async Task Execute_WhenStatusIsOk_PresentsLoginResult()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.OK);
            _gateway.Setup(gateway => gateway.ExecuteAsync(1)).ReturnsAsync(response);

            await _useCase.Execute(1);

            _presenter.Verify(
                presenter => presenter.Present(It.IsAny<LogInResultMessage>()),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenStatusIsNotFound_PresentsUserNotFoundError()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.NotFound);
            _gateway.Setup(gateway => gateway.ExecuteAsync(1)).ReturnsAsync(response);

            await _useCase.Execute(1);

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "User ID not found" &&
                        message.ClientState == ClientStates.Unregistered)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenStatusIsBadRequest_PresentsInvalidUserIdError()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _gateway.Setup(gateway => gateway.ExecuteAsync(1)).ReturnsAsync(response);

            await _useCase.Execute(1);

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "Invalid User ID." &&
                        message.ClientState == ClientStates.NoChange)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenStatusIsInternalServerError_PresentsLoginFailedError()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            _gateway.Setup(gateway => gateway.ExecuteAsync(1)).ReturnsAsync(response);

            await _useCase.Execute(1);

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "Login Failed." &&
                        message.ClientState == ClientStates.NoChange)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenStatusIsUnexpected_PresentsUnknownError()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            _gateway.Setup(gateway => gateway.ExecuteAsync(1)).ReturnsAsync(response);

            await _useCase.Execute(1);

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "Unknown Error Occurred." &&
                        message.ClientState == ClientStates.NoChange)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenRequestTimesOut_PresentsTimeoutError()
        {
            _gateway
                .Setup(gateway => gateway.ExecuteAsync(1))
                .ThrowsAsync(new TaskCanceledException());

            await _useCase.Execute(1);

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "The server request timed out." &&
                        message.ClientState == ClientStates.NoChange)),
                Times.Once);
        }

    }
}
