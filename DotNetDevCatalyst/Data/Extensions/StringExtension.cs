

namespace DevCatalyst.Data.Extensions
{
    public static class StringExtension
    {

        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }
        #region Sql

        public static string AddPrefix(this string columns, string prefix, bool removeIdentity = false, string? excludedColumns = null)
        {
            var columnsArr = columns.Replace(" ", "").Split(',');
            if (removeIdentity)
                columnsArr = columnsArr.Where(x => excludedColumns == null || !excludedColumns.Split(',').Contains(x)).ToArray();
            return string.Join(", ", columnsArr.Select(col => prefix + col));
        }
        public static string AddBraces(this string columns, string? excludedColumns = null)
        {
            var columnsArr = columns.Replace(" ", "").Split(',').Where(x => excludedColumns == null || !excludedColumns.Split(',').Contains(x));

            return string.Join(", ", columnsArr.Select(col => string.Format("[{0}]", col)));
        }
        public static string GenerateUpdateQuery(this string columns, string table, string key, string excludedColumns)
        {
            var columnsArr = columns.Replace(" ", "").Split(',').Where(x => !excludedColumns.Split(',').Contains(x));
            return string.Format("UPDATE [{0}] SET {1} WHERE {2}=@{2};", table, string.Join(", ", columnsArr.Select(col => string.Format("[{0}]=@{0}", col))), key);
        }
        public static string GenerateInsertQuery(this string columns, string table, string excludedColumns)
        {
            return string.Format("INSERT INTO [{0}] ({1}) VALUES({2});", table, columns.AddBraces(excludedColumns), columns.AddPrefix("@", true, excludedColumns));
        }

        #endregion
    }
    
}

