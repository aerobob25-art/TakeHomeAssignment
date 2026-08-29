using TakeHomeAssignment.Core.Messages;

namespace TakeHomeAssignment.Presenters.Interfaces
{
    public interface IRegisterPresenter
    {
        void Present(RegisterResultMessage registerResultMessage);
    }
}