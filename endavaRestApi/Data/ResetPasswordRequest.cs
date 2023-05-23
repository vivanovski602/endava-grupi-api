using System.Text.Json.Serialization;

namespace endavaRestApi.Data
{
    public class ResetPasswordRequest : User
    {
        [JsonIgnore] // Hide id
        public int Id { get; set; }
        public string NewPassword { get; set; }
    }
}