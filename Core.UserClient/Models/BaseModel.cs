using System.Collections.Generic;

namespace Core.UserClient.Models
{
    public abstract class BaseModel
    {
        public string ReturnUrl { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
