namespace CustomerManagement.Database.LinqToSql
{
    using System.Data.Linq;
    using System.Data.Linq.Mapping;
    using System.Configuration;


    [Database]
    public class LinqDataBase : DataContext
    {
        public Table<Customer> Customers;

        public LinqDataBase(string databaseName) : base(ConfigurationManager.ConnectionStrings[databaseName]?.ConnectionString) { }
    }
}
