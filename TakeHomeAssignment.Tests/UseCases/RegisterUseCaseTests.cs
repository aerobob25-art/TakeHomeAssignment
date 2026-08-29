using Moq;
using System.Net;
using System.Text;
using TakeHomeAssignment.Core.Gateways.Interfaces;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.UseCases;

namespace TakeHomeAssignment.Tests.UseCases
{
    [TestFixture]
    public class RegisterUseCaseTests
    {
        private Mock<ISendRegisterRequestGateway> _gateway;
        private Mock<IRegisterPresenter> _registerPresenter;
        private Mock<IErrorPresenter> _errorPresenter;
        private RegisterUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _gateway = new Mock<ISendRegisterRequestGateway>();
            _registerPresenter = new Mock<IRegisterPresenter>();
            _errorPresenter = new Mock<IErrorPresenter>();
            _useCase = new RegisterUseCase(
                _gateway.Object,
                _registerPresenter.Object,
                _errorPresenter.Object);
        }

        [Test]
        public async Task Execute_WhenResponseIsSuccessful_PresentsRegisteredUserId()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"user_id\":42}",
                    Encoding.UTF8,
                    "application/json")
            };
            _gateway.Setup(gateway => gateway.ExecuteAsync()).ReturnsAsync(response);

            await _useCase.Execute();

            _registerPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<RegisterResultMessage>(message =>
                        message.Id == 42 &&
                        message.ClientState == ClientStates.Registered)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenResponseIsUnsuccessful_PresentsResponseError()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Registration failed")
            };
            _gateway.Setup(gateway => gateway.ExecuteAsync()).ReturnsAsync(response);

            await _useCase.Execute();

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "Registration failed" &&
                        message.ClientState == ClientStates.Unregistered)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenResponseContainsInvalidUserId_PresentsInvalidUserIdError()
        {
            using var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    "{\"user_id\":\"not-an-int\"}",
                    Encoding.UTF8,
                    "application/json")
            };
            _gateway.Setup(gateway => gateway.ExecuteAsync()).ReturnsAsync(response);

            await _useCase.Execute();

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "User ID was not valid." &&
                        message.ClientState == ClientStates.Unregistered)),
                Times.Once);
        }

        [Test]
        public async Task Execute_WhenRequestIsCanceled_PresentsTimeoutError()
        {
            _gateway
                .Setup(gateway => gateway.ExecuteAsync())
                .ThrowsAsync(new TaskCanceledException());

            await _useCase.Execute();

            _errorPresenter.Verify(
                presenter => presenter.Present(
                    It.Is<ErrorMessage>(message =>
                        message.Error == "The server request timed out." &&
                        message.ClientState == ClientStates.Unregistered)),
                Times.Once);
        }
    }
}
