﻿#pragma warning disable 1591

using System;
using System.Globalization;
using System.Windows.Data;

namespace Reloaded.WPF.Converters
{
    /// <summary>
    /// Returns "true" if the string is not empty or null, else false.
    /// </summary>
    public class IsTextNotEmptyConverter : IValueConverter
    {
        public static IsTextNotEmptyConverter Instance = new IsTextNotEmptyConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                if (String.IsNullOrEmpty((string)value))
                    return false;

                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
