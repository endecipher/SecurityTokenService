using Microsoft.AspNetCore.Authorization;
using System;

namespace Core.UserClient.Policies.AdultRequirement
{
    public class AdultRequirement : IAuthorizationRequirement
    {
        public AdultRequirement() { }

        public static bool IsAdult(string dateOfBirth)
        {
            var dob = DateTime.Parse(dateOfBirth);
            var today = DateTime.Today;
            int age = DateTime.Today.Year - DateTime.Parse(dateOfBirth).Year;
            if (dob > today.AddYears(-age))
                age--;

            return age > 18;
        }
    }
}
