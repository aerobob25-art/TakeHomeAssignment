using CommunityToolkit.Mvvm.Messaging;
using TakeHomeAssignment.Core.Messages;
using TakeHomeAssignment.Core.Presenters.Interfaces;

namespace TakeHomeAssignment.Core.Presenters
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
