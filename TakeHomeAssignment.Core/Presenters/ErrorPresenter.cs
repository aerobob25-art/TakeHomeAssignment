using CommunityToolkit.Mvvm.Messaging;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Presenters.Interfaces;

namespace TakeHomeAssignment.Core.Presenters
{
    public class ErrorPresenter : IErrorPresenter
    {
        IMessenger _messenger;

        public ErrorPresenter(IMessenger messenger)
        {
            _messenger = messenger;
        }
        public void Present(ErrorMessage message)
        {
            _messenger.Send(message);
        }
    }
}
