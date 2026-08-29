namespace TakeHomeAssignment.UseCases.Interfaces
{
    public interface ILogInUseCase
    {
        Task Execute(long userId);
    }
}