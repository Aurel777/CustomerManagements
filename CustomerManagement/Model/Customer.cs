namespace CustomerManagement.Model
{
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table(Name = "Customers")]
    public class Customer 
    {
        EntityRef<Customer> customer;

        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [Description("Id"), Column(IsPrimaryKey = true, IsDbGenerated = true, Name = "Id", DbType = "int")]
        public int Id { get; set; }

        [Description("FirstName"), Column(Name = "FirstName", DbType = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Description("LastName"), Column(Name = "LastName", DbType = "nvarchar(50)")]
        public string LastName { get; set; }

        [Description("Address"), Column(Name = "Address", DbType = "nvarchar(50)")]
        public string Address { get; set; }

        [Description("PostCode"), Column(Name = "PostCode", DbType = "nvarchar(50)")]
        public string PostCode { get; set; }

        [Description("Country"), Column(Name = "Country", DbType = "nvarchar(50)")]
        public string Country { get; set; }

        [Description("PhoneNumber"), Column(Name = "PhoneNumber", DbType = "nvarchar(50)")]
        public string PhoneNumber { get; set; }

        [Description("Email"), Column(Name = "Email", DbType = "nvarchar(50)")]
        public string Email { get; set; }

        public override string ToString() =>  $"{FirstName} {LastName}";
    }
}