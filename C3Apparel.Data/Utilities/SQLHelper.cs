namespace C3Apparel.Data.Utilities
{
    public static class SQLHelper
    {
        public static string SqlString(this string sSql)
        {
            if (string.IsNullOrEmpty(sSql))
            {
                return "";
            }
            return sSql.Replace("'", "''");
        }
    }
}