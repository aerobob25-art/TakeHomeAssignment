using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Windows.Input;
using TakeHomeAssignment.Controllers.Interfaces;
using TakeHomeAssignment.Core.Messages;

namespace TakeHomeAssignment.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient,
        IRecipient<RegisterResultMessage>,
        IRecipient<ErrorMessage>
    {
        private ILogInController _logInController;
        private IRegisterController _registerController;

        public MainWindowViewModel(ILogInController logInController, IRegisterController registerController, IMessenger messenger)
            : base(messenger)
        {
            IsActive = true;
            _logInController = logInController;
            _registerController = registerController;

            LogInCommand = new RelayCommand(LogIn);
            RegisterCommand = new RelayCommand(Register);
        }

        private void Register()
        {
            ClearErrorState();
            _registerController.Execute();
        }

        private void LogIn()
        {
            _logInController.Execute(UserId);
        }

        private void ClearErrorState()
        {
            HasError = false;
            ErrorMessage = string.Empty;
        }

        public void Receive(RegisterResultMessage message)
        {
            UserId = message.Id;
            ClientState = "Registered";
        }

        public void Receive(ErrorMessage message)
        {
            HasError = true;
            ErrorMessage = message.Error;
        }

        public ICommand LogInCommand { get; }
        
        public ICommand RegisterCommand { get; }


        private int _userId;
        public int UserId 
        { 
            get => _userId; 
            set => SetProperty(ref _userId, value); 
        }

        public string ClientState { get; set; } = "Unregistered";

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
    }
}
