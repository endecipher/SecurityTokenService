using System;
using System.ComponentModel.DataAnnotations;

namespace Core.UserClient.Models
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public DateValidationAttribute()
        {

        }

        public override bool IsValid(object value)
        {
            if (value != null && value is DateTime?)
            {
                var date = value as DateTime?;

                if (date > DateTime.Now.AddYears(-12))
                {
                    ErrorMessage = "DateOfBirthTooYoung";
                    return false;
                }

                if (date < DateTime.Now.AddYears(-50))
                {
                    ErrorMessage = "DateOfBirthTooOld";
                    return false;
                }
            }
            else
            {
                ErrorMessage = "DateOfBirthIncorrect";
                return false;
            }

            return true;
        }
    }
}
