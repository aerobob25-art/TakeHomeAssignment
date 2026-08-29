using TakeHomeAssignment.Core.Models;

namespace TakeHomeAssignment.Core.Messages
{
    public class ErrorMessage
    {
        public string Error { get; set; } = string.Empty;

        public ClientStates ClientState { get; set; }
    }
}
