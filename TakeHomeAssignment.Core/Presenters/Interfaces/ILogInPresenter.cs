using TakeHomeAssignment.Core.Messages;

namespace TakeHomeAssignment.Presenters.Interfaces
{
    public interface ILogInPresenter
    {
        void Present(LogInResultMessage message);
    }
}