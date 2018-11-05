namespace CustomerManagement.View
{
    #region Using Statements

    using System;
    using System.Windows.Threading;
    using Model;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using Database;

    #endregion

    public partial class MainWindow : Window
    {
        #region Constants

        const string DatabaseName = "Customers.mdf";
        const string TableName = "Customers";

        #endregion

        #region Private Fields

        readonly DataBaseEngine dbEngine;
        DispatcherTimer dispatcherTimer;

        #endregion;

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            dbEngine = new DataBaseEngine(Path.GetFileNameWithoutExtension(DatabaseName));
            AddTimer(500);
            FetchCustomers();
        }

        #endregion

        #region Events

        void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var columnsName = new[] {FirstNameBox.Name.Replace("Box", string.Empty), LastNameBox.Name.Replace("Box", string.Empty), AddressBox.Name.Replace("Box", string.Empty), PostCodeBox.Name.Replace("Box", string.Empty), CountryBox.Name.Replace("Box", string.Empty), PhoneNumberBox.Name.Replace("Box", string.Empty), EmailBox.Name.Replace("Box", string.Empty) };
            var values = new[] { FirstNameBox.Text, LastNameBox.Text, AddressBox.Text, PostCodeBox.Text, CountryBox.Text, PhoneNumberBox.Text, EmailBox.Text };
            var customerToUpdate = (Customer)DataContext;
            if (dbEngine.Update(TableName, columnsName, values, $" WHERE Id={IdBox.Text.Replace("Box", string.Empty)}") > 0)
                RefreshCustomers($"{customerToUpdate.ToString()} updated.");
        }

        void allCustomersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => DataContext = (Customer) AllCustomersComboBox.SelectedItem;
        
        void addButton_Click(object sender, RoutedEventArgs e)
        {
            var addCustomerScreen = new AddCustomerScreen(dbEngine);
            addCustomerScreen.NewCustomerAdded += AddCustomerScreen_NewCustomerAdded;
            addCustomerScreen.Show();
        }
        
        void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(AllCustomersComboBox.SelectedIndex < -1)
                return;

            var customerToDelete = (Customer)DataContext;
            if (dbEngine.Delete(Queries.DeleteQuery(TableName, $"Id={AllCustomersComboBox.SelectedIndex}")) > 0)
                RefreshCustomers($"{customerToDelete.ToString()} deleted.");
            
        }

        void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchCustomerScreen = new SearchCustomerScreen();
            searchCustomerScreen.Show();
        }

        void AddCustomerScreen_NewCustomerAdded(object sender, NewCustomerAddedEventArgs e)
        {
            AllCustomersComboBox.Items.Add(e.NewCustomer);
            RefreshCustomers($"{e.NewCustomer.ToString()} added.");
        }

        void DispatcherTimer_Tick(object sender, EventArgs e) => LabelInfo.Content = string.Empty; 

        #endregion

        #region Members

        void AddTimer(int intervalInSeconds)
        {
            dispatcherTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, intervalInSeconds) };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Start();
        }

        void FetchCustomers()
        {
            //foreach (var customer in LinqCustomersAccessor.GetCustomers())
            //    AllCustomersComboBox.Items.Add(customer);
            foreach (var customer in dbEngine.Select($"SELECT * FROM {TableName}"))
                AllCustomersComboBox.Items.Add(customer);
            RefreshCustomers($"{AllCustomersComboBox.Items.Count} customers have been fetched.");
        }

        void RefreshCustomers(string labelInfoContent = "")
        {
            LabelInfo.Content = labelInfoContent;
            if (AllCustomersComboBox.Items.Count > 0)
                AllCustomersComboBox.SelectedIndex = AllCustomersComboBox.Items.Count - 1;
        }

        #endregion
    }
}