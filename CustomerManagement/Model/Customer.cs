namespace CustomerManagement.Model
{
    using System.ComponentModel;
    
    public class Customer
    {
        public Customer(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        [Description("Id")] public int Id { get; set; }

        [Description("FirstName")] public string FirstName { get; set; }

        [Description("LastName")] public string LastName { get; set; }

        [Description("Address")] public string Address { get; set; }

        [Description("PostCode")] public string PostCode { get; set; }

        [Description("Country")] public string Country { get; set; }

        [Description("PhoneNumber")] public string PhoneNumber { get; set; }

        [Description("Email")] public string Email { get; set; }

        public override string ToString() => $"{FirstName} {LastName}";
    }
}