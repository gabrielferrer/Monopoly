using System;
using System.Globalization;
using System.Windows;

namespace Monopoly.UI
{
    internal class BoolNotConverter : MarkupExtensionConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue ? !boolValue : DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue ? !boolValue : DependencyProperty.UnsetValue;
        }
    }
}
