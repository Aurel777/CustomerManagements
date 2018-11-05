namespace CustomerManagement.Database.LinqToSql
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqCustomersAccessor
    {
        public static IEnumerable<Customer> GetCustomers()
        {
            var dataBase = new LinqDataBase("Customers");
            return from customer in dataBase.Customers select customer;
        }
    }
}
