using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Access.Models.CustomAttributes
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
                    ErrorMessage = Resource.DateOfBirthTooYoung;
                    return false;
                }

                if (date < DateTime.Now.AddYears(-50))
                {
                    ErrorMessage = Resource.DateOfBirthTooOld;
                    return false;
                }
            }
            else
            {
                ErrorMessage = Resource.DateOfBirthIncorrect;
                return false;
            }

            return true;
        }
    }
}
