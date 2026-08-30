namespace TakeHomeAssignment.Core.UseCases.Interfaces
{
    public interface ILogInUseCase
    {
        Task Execute(long? userId);
    }
}