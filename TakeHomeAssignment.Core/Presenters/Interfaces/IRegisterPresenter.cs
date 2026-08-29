using TakeHomeAssignment.Core.Messages;

namespace TakeHomeAssignment.Core.Presenters.Interfaces
{
    public interface IRegisterPresenter
    {
        void Present(RegisterResultMessage registerResultMessage);
    }
}