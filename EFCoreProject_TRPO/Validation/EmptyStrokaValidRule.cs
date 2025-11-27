using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Globalization;

namespace EFCoreProject_TRPO.Validation
{
    public class EmptyStrokaValidRule : BasicValidRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty((string?)value))
            {
                return ValidationResult(false, "Заполните поле");
            }

            return ValidationResult(true, string.Empty);
        }
    }
}
