using CommunityToolkit.Mvvm.Messaging;
using Moq;
using TakeHomeAssignment.Core.Controllers.Interfaces;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;
using TakeHomeAssignment.Core.ViewModels;

namespace TakeHomeAssignment.Core.Tests.ViewModels
{
    [TestFixture]
    public class MainWindowViewModelTests
    {
        MainWindowViewModel _viewModel;
        Mock<ILogInController> _logInController;
        Mock<IRegisterController> _registerController;
        Mock<IMessenger> _messenger;

        [SetUp]
        public void Setup()
        {
            _logInController = new Mock<ILogInController>();
            _registerController = new Mock<IRegisterController>();
            _messenger = new Mock<IMessenger>();
            _viewModel = new MainWindowViewModel(_logInController.Object, _registerController.Object, _messenger.Object);
        }

        [Test]
        public void Constructor_InitializesUnregistered()
        {
            Assert.Multiple(() =>
            {
                Assert.That(_viewModel.UserId, Is.Empty);
                Assert.That(_viewModel.ClientState, Is.EqualTo("Unregistered"));
                Assert.That(_viewModel.HasError, Is.False);
                Assert.That(_viewModel.ErrorMessage, Is.Empty);
                Assert.That(_viewModel.LogInCommand.CanExecute(null), Is.True);
                Assert.That(_viewModel.RegisterCommand.CanExecute(null), Is.True);
            });
        }

        [Test]
        public void LogInCommand_WithValidUserId_CallsLogInController()
        {
            _viewModel.UserId = "123456";
            _viewModel.LogInCommand.Execute(null);

            _logInController.Verify(m => m.Execute(123456));
        }

        [TestCase("")]
        [TestCase("  ")]
        [TestCase("abc")]
        [TestCase("123abc")]
        public void LogInCommand_WithInvalidUserId_DisplaysError(string userId)
        {
            _viewModel.UserId = userId;
            _viewModel.LogInCommand.Execute(null);

            Assert.Multiple(() =>
            {
                Assert.That(_viewModel.HasError, Is.True);
                Assert.That(_viewModel.ErrorMessage, Is.EqualTo("Please enter a valid User ID."));
            });
        }

        [Test]
        public void LogInCommand_WithInvalidUserId_DoesNotCallLogInController()
        {
            _viewModel.UserId = "abc";
            _viewModel.LogInCommand.Execute(null);

            _logInController.Verify(m => m.Execute(It.IsAny<long>()), Times.Never);
        }

        [Test]
        public void LogInCommand_WithCurrentErrorState_ClearsErrorState()
        {
            _viewModel.UserId = "123";
            _viewModel.HasError = true;
            _viewModel.ErrorMessage = "Error";

            _viewModel.LogInCommand.Execute(null);

            Assert.Multiple(() =>
            {
                Assert.That(_viewModel.HasError, Is.False);
                Assert.That(_viewModel.ErrorMessage, Is.Empty);
            });
        }

        [Test]
        public void CanLogIn_WhenWorking_ReturnsFalse()
        {
            _viewModel.UserId = "123";
            _viewModel.LogInCommand.Execute(null);

            Assert.That(_viewModel.LogInCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void ReceiveLogInMessage_SetsClientState()
        {
            _viewModel.Receive(new LogInResultMessage()
            {
                ClientState = ClientStates.LoggedIn
            });

            Assert.That(_viewModel.ClientState, Is.EqualTo("Logged In"));
        }

        [Test]
        public void RegisterCommand_WhenExecuted_CallsRegisterController()
        {
            _viewModel.RegisterCommand.Execute(null);

            _registerController.Verify(m => m.Execute(default), Times.Once);
        }

        [Test]
        public void CanRegister_WhenWorking_ReturnsFalse()
        {
            _viewModel.RegisterCommand.Execute(null);

            Assert.That(_viewModel.RegisterCommand.CanExecute(null), Is.False);
        }

        [Test]
        public void ReceiveRegisterMessage_SetsRegisteredState()
        {
            _viewModel.Receive(new RegisterResultMessage()
            {
                Id = 123,
                ClientState = ClientStates.Registered
            });

            Assert.That(_viewModel.ClientState, Is.EqualTo("Registered"));
        }

        [Test]
        public void ReceiveRegisterMessage_SetsUserId()
        {
            _viewModel.Receive(new RegisterResultMessage()
            {
                Id = 123,
                ClientState = ClientStates.Registered
            });

            Assert.That(_viewModel.UserId, Is.EqualTo("123"));
        }

        [Test]
        public void ReceiveErrorMessage_SetsErrorState()
        {
            _viewModel.Receive(new ErrorMessage()
            {
                Error = "Error",
                ClientState = ClientStates.Unregistered
            });

            Assert.Multiple(() =>
            {
                Assert.That(_viewModel.HasError, Is.True);
                Assert.That(_viewModel.ErrorMessage, Is.EqualTo("Error"));
            });
        }

        [Test]
        public void RecieveErrorMessage_WithNoChangeClientState_ClientStateRemainsTheSame()
        {
            _viewModel.ClientState = "Registered";

            _viewModel.Receive(new ErrorMessage()
            {
                Error = "Error",
                ClientState = ClientStates.NoChange
            });

            Assert.That(_viewModel.ClientState, Is.EqualTo("Registered"));
        }

        [Test]
        public void RegisterCommand_WithCurrentErrorState_ClearsErrorState()
        {
            _viewModel.HasError = true;
            _viewModel.ErrorMessage = "Error";

            _viewModel.RegisterCommand.Execute(null);

            Assert.Multiple(() =>
            {
                Assert.That(_viewModel.HasError, Is.False);
                Assert.That(_viewModel.ErrorMessage, Is.Empty);
            });
        }
    }
}
