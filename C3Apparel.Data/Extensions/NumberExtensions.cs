using System;
using System.Globalization;

namespace C3Apparel.Data.Extensions
{
    public static class NumberExtensions
    {
        public static int ToInteger(this string str)
        {
            int i = 0;
            Int32.TryParse(str, NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out i);

            return i;
        }
        
        public static decimal ToDecimal(this string str)
        {
            decimal i = 0;
            Decimal.TryParse(str, NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out i);

            return i;
        }
        public static decimal ToDecimal(this object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            decimal i = 0;
            Decimal.TryParse(obj.ToString(), NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, out i);

            return i;
        }
        
        public static decimal RoundUp(this decimal number, int numberOfDecimal)
        {
            var unit = 1;
            for (int i = 0; i < numberOfDecimal; i++)
            {
                unit = unit * 10;
            }

            return Math.Ceiling(number * unit) / unit;
        }
    }
}