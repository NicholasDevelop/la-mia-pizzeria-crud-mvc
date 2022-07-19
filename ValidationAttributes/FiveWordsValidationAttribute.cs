using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationAttributes
{
    internal class FiveWordsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            string fieldValue = (string)value;

            if(fieldValue == null || fieldValue.Trim().IndexOf(" ") <= 4)
            {
                return new ValidationResult("Il campo deve contenere almeno 5 parole");
            }

            return ValidationResult.Success;
        }
    }
}