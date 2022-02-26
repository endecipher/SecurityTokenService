using System.ComponentModel.DataAnnotations;

namespace Core.Access.Models
{
    public class OAuthModel : BaseModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string State { get; set; }
    }
}
