namespace CustomerManagement.View
{
    #region Using Statements

    using System;
    using System.Windows;
    using Model;
    using Database;

    #endregion

    public partial class AddCustomerScreen : Window
    {
        #region Private Fields

        readonly DataBaseEngine dbEngine;

        #endregion
        
        #region  Events

        public event EventHandler<NewCustomerAddedEventArgs> NewCustomerAdded;

        #endregion

        #region Constructor

        public AddCustomerScreen(DataBaseEngine engine)
        {
            InitializeComponent();
            dbEngine = engine;
        }

        #endregion

        #region Events

        void saveButton_Click(object sender, RoutedEventArgs e) => AddNewCustomer();

        #endregion

        #region Methods

        void AddNewCustomer()
        {
            // TODO : this doesn't fix the issue. 
            // e.g : I'm a dumbass would throw an exception because of the single quote
            var newCustomer = new Customer(FirstNameBox.Text.Replace("'","\\'"), LastNameBox.Text.Replace("'", "\\'"))
            {
                Address = AddressBox.Text.Replace("'", "\\'"),
                PostCode = PostcodeBox.Text.Replace("'", "\\'"),
                Country = CountryBox.Text.Replace("'", "\\'"),
                PhoneNumber = PhoneNumberBox.Text.Replace("'", "\\'"),
                Email = EmailBox.Text.Replace("'", "\\'") 
            };

            // Database has now IDENTITY on its primary key, no need to increment the Id anymore.
            //var id = (int)dbEngine.ExecuteScalar(Queries.CountQuery("Customers")) + 1; 
            //var query = $@"INSERT INTO Customers (Id, FirstName, LastName, Address, Postcode, Country, PhoneNumber, Email) VALUES ({id}, '{FirstNameBox.Text}', '{LastNameBox.Text}', '{AddressBox.Text}',  '{PostcodeBox.Text}', '{CountryBox.Text}', '{PhoneNumberBox.Text}', '{EmailBox.Text}')";
            var query = $@"INSERT INTO Customers (FirstName, LastName, Address, Postcode, Country, PhoneNumber, Email) VALUES ('{FirstNameBox.Text}', '{LastNameBox.Text}', '{AddressBox.Text}',  '{PostcodeBox.Text}', '{CountryBox.Text}', '{PhoneNumberBox.Text}', '{EmailBox.Text}')";

            if (dbEngine.Add(query) > 0)
                OnNewCustomerAdded(new NewCustomerAddedEventArgs(newCustomer));
        }

        protected virtual void OnNewCustomerAdded(NewCustomerAddedEventArgs e)
        {
            var handler = NewCustomerAdded;
            handler?.Invoke(this, e);
        }

        #endregion
    }
}