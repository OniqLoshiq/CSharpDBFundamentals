using System;
using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem.Data.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NonUnicodeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string errorMsgNull = "Value cannot be bull";

            if(value == null)
            {
                return new ValidationResult(errorMsgNull);
            }

            string text = value.ToString();

            string errorMsgUnicode = "Value cannot contain unicode characters!";

            for (int i = 0; i < text.Length; i++)
            {
                if(text[i] > 255)
                {
                    return new ValidationResult(errorMsgUnicode);
                }
            }

            return ValidationResult.Success;
        }
    }
}
