using System.Data;

namespace C3Apparel.Data.Sql
{
    public static class DataHelper
    {
        public static bool IsEmpty(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}