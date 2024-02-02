using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Monopoly
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    public abstract class MarkupExtensionConverterBase : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }

    [MarkupExtensionReturnType(typeof(IMultiValueConverter))]
    public abstract class MarkupExtensionMultiConverterBase : MarkupExtension, IMultiValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
