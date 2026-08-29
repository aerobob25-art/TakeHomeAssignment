using TakeHomeAssignment.Core.Messages;

namespace TakeHomeAssignment.Core.Presenters.Interfaces
{
    public interface IErrorPresenter
    {
        public void Present(ErrorMessage message);
    }
}
