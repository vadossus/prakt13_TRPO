using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace EFCoreProject_TRPO.Validation
{
    public abstract class BasicValidRule : ValidationRule
    {
        public string PropertyName { get; set; } = string.Empty;

        protected ValidationResult ValidationResult(bool isValid, string errorMessage)
        {
            return new ValidationResult(isValid, errorMessage);
        }
    }
}
