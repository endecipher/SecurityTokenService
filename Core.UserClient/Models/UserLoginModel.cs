using System.ComponentModel.DataAnnotations;

namespace Core.UserClient.Models
{
    public class UserLoginModel : BaseModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
