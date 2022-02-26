using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Access.Identity.DB.EntitySets
{
    public class Client : IEntity
    {
        [Key]
        public string ClientId { get; set; }

        public string Password { get; set; }

        public string FriendlyName { get; set; }

        public static Func<string, string, string, string> Concatenate = (id, name, pw) => $"{id}{name}{pw}";
    }
}
