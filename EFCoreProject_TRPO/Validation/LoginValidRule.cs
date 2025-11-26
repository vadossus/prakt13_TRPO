using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace EFCoreProject_TRPO.Validation
{
    public class LoginValidRule : BasicValidRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string login)
            {
                if (string.IsNullOrWhiteSpace(login))
                    return ValidationResult(false, "Логин обязателен для заполнения");

                if (login.Length < 5)
                    return ValidationResult(false, "Логин должен содержать минимум 5 символов");

                return ValidationResult(true, string.Empty);
            }

            return ValidationResult(false, "Неверный формат логина");
        }
    }
}
