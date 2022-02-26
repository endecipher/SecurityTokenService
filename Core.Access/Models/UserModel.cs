using Core.Access.Models.CustomAttributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Access.Models
{
    public class UserModel : BaseModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(1, 10)]
        public int SecurityLevel { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        [DateValidation]
        public DateTime DateOfBirth { get; set; }
    }
}
