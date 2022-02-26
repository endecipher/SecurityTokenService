using Microsoft.AspNetCore.Authorization;
using System;

namespace Core.UserClient.Policies.SecurityLevelRequirement
{
    public class SecurityLevelRequirement : IAuthorizationRequirement
    {
        private const string HighestLevel = "10";

        public string Level { get; set; }

        public SecurityLevelRequirement(string level)
        {
            Level = level;
        }

        public static bool IsAcceptable(string ClaimLevel, string RequirementLevel)
        {
            return Convert.ToInt32(ClaimLevel) >= Convert.ToInt32(RequirementLevel);
        }

        public static SecurityLevelRequirement Highest()
        {
            return new SecurityLevelRequirement(HighestLevel);
        }

        public static bool IsHighest(string level)
        {
            return level.Equals(HighestLevel);
        }
    }
}
