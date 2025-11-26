using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCoreProject_TRPO.Validation
{
    public class EmailValidRule : BasicValidRule
    {
        private static readonly Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string email)
            {
                if (string.IsNullOrWhiteSpace(email))
                    return ValidationResult(false, "Email обязателен для заполнения");

                if (!emailRegex.IsMatch(email))
                    return ValidationResult(false, "Email должен содержать символ @ и корректный домен");

                return ValidationResult(true, string.Empty);
            }

            return ValidationResult(false, "Неверный формат email");
        }
    }
}
