namespace CustomerManagement.Database
{
    public static class Queries
    {
        public static string CountQuery(string tableName) => $"SELECT COUNT(*) FROM {tableName}";

        public static string DeleteQuery(string tableName, string whereClause) =>
            $"DELETE FROM {tableName} WHERE {whereClause}";

        public static string GetColumnNamesFromTable(string tableName) => 
            $@"SELECT COLUMN_NAME
            FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = '{tableName}'  
            ORDER BY ORDINAL_POSITION ASC";

        public static string InsertIntoTable(string tableName) =>
            $@"INSERT INTO (Id, FirstName, LastName, Address, Postcode, Country, PhoneNumber, Email)
               VALUES (@Id, @Firstname, @Lastname, @Address, @Postcode, @Country, @PhoneNumber, @Email)";
    }
}
