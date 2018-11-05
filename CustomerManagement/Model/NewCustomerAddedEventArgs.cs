namespace CustomerManagement.Model
{
    using System;

    public class NewCustomerAddedEventArgs : EventArgs
    {
        public NewCustomerAddedEventArgs(Customer customer) => NewCustomer = customer;
        
        public Customer NewCustomer { get; }
    }
}