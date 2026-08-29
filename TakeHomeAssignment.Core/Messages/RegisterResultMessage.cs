using TakeHomeAssignment.Core.Models;

namespace TakeHomeAssignment.Core.Messages
{
    public class RegisterResultMessage
    {
        public long Id { get; set; }

        public ClientStates ClientState { get; set; }
    }
}
