using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using TakeHomeAssignment.Core.Controllers.Interfaces;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Models;

namespace TakeHomeAssignment.Core.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient,
        IRecipient<RegisterResultMessage>,
        IRecipient<LogInResultMessage>,
        IRecipient<ErrorMessage>
    {
        private ILogInController _logInController;
        private IRegisterController _registerController;
        private bool _isWorking;

        public MainWindowViewModel(ILogInController logInController, IRegisterController registerController, IMessenger messenger)
            : base(messenger)
        {
            IsActive = true;
            _logInController = logInController;
            _registerController = registerController;

            LogInCommand = new RelayCommand(LogIn, CanLogIn);
            RegisterCommand = new RelayCommand(Register, CanRegister);
        }

        private void Register()
        {
            ClearErrorState();
            SetWorking(true);
            _registerController.Execute();
        }

        private void LogIn()
        {
            ClearErrorState();
            if(string.IsNullOrWhiteSpace(UserId) || !long.TryParse(UserId, out var userId))
            {
                HasError = true;
                ErrorMessage = "Please enter a valid User ID.";
                return;
            }

            SetWorking(true);
            _logInController.Execute(userId);
        }

        private void SetWorking(bool isWorking)
        {
            _isWorking = isWorking;
            LogInCommand.NotifyCanExecuteChanged();
            RegisterCommand.NotifyCanExecuteChanged();
        }

        private bool CanLogIn()
        {
            return !_isWorking;
        }

        private bool CanRegister()
        {
            return !_isWorking;
        }

        private void ClearErrorState()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }

        public void Receive(RegisterResultMessage message)
        {
            UserId = message.Id.ToString();
            ClientState = GetClientStateText(message.ClientState);
            SetWorking(false);
        }

        public void Receive(ErrorMessage message)
        {
            HasError = true;
            ErrorMessage = message.Error;
            ClientState = GetClientStateText(message.ClientState);
            SetWorking(false);
        }

        public void Receive(LogInResultMessage message)
        {
            ClientState = GetClientStateText(message.ClientState);
            SetWorking(false);
        }

        public IRelayCommand LogInCommand { get; }
        
        public IRelayCommand RegisterCommand { get; }


        private string _userId = string.Empty;
        public string UserId 
        { 
            get => _userId; 
            set => SetProperty(ref _userId, value); 
        }

        private string _clientState = "Unregistered";
        public string ClientState
        {
            get => _clientState;
            set => SetProperty(ref _clientState, value);
        }

        private bool _hasError = false;
        public bool HasError 
        { 
            get => _hasError; 
            set => SetProperty(ref _hasError, value); 
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage 
        { 
            get => _errorMessage; 
            set => SetProperty(ref _errorMessage, value); 
        }

        private string GetClientStateText(ClientStates state)
        {
            var stateText = ClientState;

            switch (state)
            {
                case ClientStates.NoChange:
                    break;
                case ClientStates.Registered:
                    stateText = "Registered";
                    break;
                case ClientStates.LoggedIn:
                    stateText = "Logged In";
                    break;
                default:
                    stateText = "Unregistered";
                    break;
            }
                
            return stateText;
        }
    }
}
