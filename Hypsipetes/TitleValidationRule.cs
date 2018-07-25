using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Hypsipetes
{
    class TitleValidationRule : ValidationRule
    {
        public int MaxTitleLength { set; get; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            var text = value as string;

            if (string.IsNullOrWhiteSpace(text))
                return new ValidationResult(false, "Not empty or whitespace");

            if (text.Length > MaxTitleLength)
                return new ValidationResult(false, string.Format("Title must less than {0}", MaxTitleLength));

            if (text.Length == 0)
                return new ValidationResult(false, "Not empty");

            return new ValidationResult(true, null);
        }
    }
}
