using CommunityToolkit.Mvvm.Messaging;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Presenters.Interfaces;

namespace TakeHomeAssignment.Presenters
{
    public class RegisterPresenter : IRegisterPresenter
    {
        private IMessenger _messenger;

        public RegisterPresenter(IMessenger messenger)
        {
            _messenger = messenger;
        }
        public void Present(RegisterResultMessage message)
        {
            _messenger.Send(message);
        }
    }
}
