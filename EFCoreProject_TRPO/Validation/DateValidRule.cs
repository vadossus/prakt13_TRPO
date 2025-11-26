using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCoreProject_TRPO.Validation
{
    public class DateValidRule : BasicValidRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Now)
                    return ValidationResult(false, "Дата создания не может быть в будущем");

                return ValidationResult(true, string.Empty);
            }

            return ValidationResult(false, "Неверный формат даты");
        }
    }
}
