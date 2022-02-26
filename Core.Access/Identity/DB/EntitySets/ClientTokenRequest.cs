using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Access.Identity.DB.EntitySets
{
    public class ClientTokenRequest : IEntity
    {
        [Key]
        public long Id { get; set; }

        public string ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public bool IsActive { get; set; }
    }
}
