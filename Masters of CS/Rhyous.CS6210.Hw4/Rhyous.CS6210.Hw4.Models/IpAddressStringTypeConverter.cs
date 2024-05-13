using System;
using System.ComponentModel;
using System.Globalization;

namespace Rhyous.CS6210.Hw4.Models
{
    public class IpAddressStringTypeConverter : TypeConverter
    {
        #region TypeConverter
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return value.ToString();
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is string)
                return (IpAddress)value.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion
}
