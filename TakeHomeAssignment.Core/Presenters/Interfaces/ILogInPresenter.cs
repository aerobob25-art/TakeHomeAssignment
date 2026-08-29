using TakeHomeAssignment.Core.Messages;

namespace TakeHomeAssignment.Core.Presenters.Interfaces
{
    public interface ILogInPresenter
    {
        void Present(LogInResultMessage message);
    }
}