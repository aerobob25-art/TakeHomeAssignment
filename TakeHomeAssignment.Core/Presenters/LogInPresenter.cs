using CommunityToolkit.Mvvm.Messaging;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Presenters.Interfaces;

namespace TakeHomeAssignment.Presenters
{
    public class LogInPresenter : ILogInPresenter
    {
        private IMessenger _messenger;

        public LogInPresenter(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public void Present(LogInResultMessage message)
        {
            _messenger.Send(message);
        }
    }
}
