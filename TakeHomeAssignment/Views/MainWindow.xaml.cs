using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Net.Http;
using TakeHomeAssignment.Core.Controllers;
using TakeHomeAssignment.Core.Controllers.Interfaces;
using TakeHomeAssignment.Core.Gateways;
using TakeHomeAssignment.Core.Gateways.Interfaces;
using TakeHomeAssignment.Core.Presenters;
using TakeHomeAssignment.Core.Presenters.Interfaces;
using TakeHomeAssignment.Core.UseCases.Interfaces;
using TakeHomeAssignment.UseCases;
using TakeHomeAssignment.ViewModels;
using WinRT.Interop;

namespace TakeHomeAssignment
{
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            var windowId = Win32Interop.GetWindowIdFromWindow(WindowNative.GetWindowHandle(this));
            var appWindow = AppWindow.GetFromWindowId(windowId);

            appWindow.Resize(new Windows.Graphics.SizeInt32(600, 400));

            this.Root.DataContext = CreateMainViewModel();
        }

        public MainWindowViewModel CreateMainViewModel()
        {

            IMessenger messenger = new WeakReferenceMessenger();
            HttpClient httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:8080"),
                Timeout = TimeSpan.FromSeconds(10)
            };

            ISendRegisterRequestGateway sendRegisterRequestGateway = new SendRegisterRequestGateway(httpClient);
            ISendLogInRequestGateway sendLogInRequestGateway = new SendLogInRequestGateway(httpClient);

            IRegisterPresenter registerPresenter = new RegisterPresenter(messenger);
            ILogInPresenter logInPresenter = new LogInPresenter(messenger);
            IErrorPresenter errorPresenter = new ErrorPresenter(messenger);

            IRegisterUseCase registerUseCase = new RegisterUseCase(sendRegisterRequestGateway, registerPresenter, errorPresenter);
            ILogInUseCase logInUseCase = new LogInUseCase(sendLogInRequestGateway, logInPresenter, errorPresenter);

            ILogInController logInController = new LogInController(logInUseCase);
            IRegisterController registerController = new RegisterController(registerUseCase);

            return new MainWindowViewModel(logInController, registerController, messenger);
        }
    }
}
