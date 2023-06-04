using System;

namespace C3Apparel.Data.Extensions
{
    public static class ObjectExtensions
    {
        public static int ToInt(this object obj)
        {
            if (obj is int integer)
                return integer;
            int result;
            return int.TryParse(Convert.ToString(obj), out result) ? result : 0;
        }

        public static double ToDouble(this object obj)
        {
            if (obj is double number)
                return number;
            double result;
            return double.TryParse(Convert.ToString(obj), out result) ? result : 0;
        }

        public static string ToSafeString(this object obj)
        {
            if (obj is string str)
                return str;
            if (obj == null)
                return String.Empty;
            return  Convert.ToString(obj);
        }

        public static DateTime ToDateTime(this object obj)
        {
            if (obj == null)
                return DateTime.MinValue;
            if (obj is DateTime dateTime)
                return dateTime;
            try
            {
                return  DateTime.Parse(obj.ToString());
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        
        
        public static bool ToBool(this object obj)
        {
            if (obj == null  || obj is DBNull)
                return false;

            bool result = false;
            bool.TryParse(obj.ToString(), out result);

            return result;
        }

        public static object ToSQLBit(this bool? obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }

            return obj.Value.ToInt();
        }
        
        public static object ToSQLBit(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return DBNull.Value;
            }

            return str.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0;
        }

        public static object ToSQLInt(this int? obj)
        {
            if (obj == null)
            {
                return DBNull.Value;
            }

            return obj;
        }
        
    }
}