using Core.UserClient.Data.DB;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.UserClient.Data.Domain
{
    public class ClientUser : IEntity
    {
        [Key]
        [DataType(DataType.Text)]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [Range(1,10)]
        public int SecurityLevel { get; set; }

        public static Func<string, string, string> Concatenate = (id, pw) => $"{id}{pw}";
    }
}
