namespace TakeHomeAssignment.Core.Messages
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Error = message;
        }

        public string Error { get; set; } = string.Empty;
    }
}
