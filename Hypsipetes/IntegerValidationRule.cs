using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Hypsipetes
{
    public class IntegerValidationRule : ValidationRule
    {
        public bool IsArrowNegative { set; get; }
        public bool IsArrowZero { set; get; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var text = value as string;
            int intValue;

            if (!int.TryParse(text, out intValue))
                return new ValidationResult(false, "value is numeric");

            if (!IsArrowNegative && intValue < 0)
                return new ValidationResult(false, "not negative");

            if(!IsArrowZero && intValue == 0)
                return new ValidationResult(false, "not zero");

            return new ValidationResult(true, null);
        }
    }
}
