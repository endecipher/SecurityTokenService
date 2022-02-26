using System.Collections.Generic;

namespace Core.Access.Models
{
    /// <summary>
    /// Base View Model class
    /// </summary>
    public abstract class BaseModel
    {
        public string ReturnUrl { get; set; }

        public List<string> Errors { get; set; } = new List<string>();
    }
}
