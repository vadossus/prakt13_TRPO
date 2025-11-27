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
    public class PasswordValidRule : BasicValidRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is string password)
            {
                if (string.IsNullOrWhiteSpace(password))
                    return ValidationResult(false, "Пароль обязателен для заполнения");

                if (password.Length < 8)
                    return ValidationResult(false, "Пароль должен содержать минимум 8 символов");

                if (!Regex.IsMatch(password, @"[0-9]"))
                    return ValidationResult(false, "Пароль должен содержать цифры");

                if (!Regex.IsMatch(password, @"[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]"))
                    return ValidationResult(false, "Пароль должен содержать специальные символы");

                if (!Regex.IsMatch(password, @"[a-z]"))
                {
                    return ValidationResult(false, "Буквы нижнего регистра должны присутствовать");
                }

                if (!Regex.IsMatch(password, @"[A-Z]"))
                {
                    return ValidationResult(false, "Буквы вернего регистра должны присутствовать");
                }

                return ValidationResult(true, string.Empty);
            }

            return ValidationResult(false, "Неверный формат пароля");
        }
    }
}

