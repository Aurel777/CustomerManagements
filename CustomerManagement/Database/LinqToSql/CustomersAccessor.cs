namespace CustomerManagement.Database.LinqToSql
{
    using System.Collections.Generic;
    using System.Linq;

    public static class CustomersAccessor
    {
        public static IEnumerable<Customer> GetCustomers()
        {
            var dataBase = new LinqDataBase("Customers");
            return from customer in dataBase.Customers select customer;
        }

        public static void Insert(string firstName, string lastName, string address, string postCode, string country, string phoneNumber, string email)
        {
            var dataBase = new LinqDataBase("Customers");
            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                PostCode = postCode,
                Country = country,
                PhoneNumber = phoneNumber,
                Email = email
            };
            dataBase.Customers.InsertOnSubmit(customer);
            dataBase.SubmitChanges();
        }

    }
}
