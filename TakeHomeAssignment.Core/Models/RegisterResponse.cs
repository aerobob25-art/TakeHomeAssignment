using System.Text.Json.Serialization;

namespace TakeHomeAssignment.Core.Models
{
    public class RegisterResponse
    {
        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
