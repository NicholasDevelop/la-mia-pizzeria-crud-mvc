using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.ValidationAttributes
{
    internal class FiveWordsValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string description = (string)value;
            int counterSpaces = 0;

            if(description != null)
            {
                foreach(char c in description.Trim())
                {
                    if(c.Equals(' '))
                    {
                        counterSpaces++;
                    }
                }
            }

            if(counterSpaces < 4)
            {
                return new ValidationResult("La descrizione deve avere almeno 5 parole");
            }

            return ValidationResult.Success;
        }
    }
}