namespace CustomerManagement.View
{
    using System.Collections.Generic;
    using System.Windows;
    using Model;

    public partial class SearchCustomerScreen
    {
        public SearchCustomerScreen()
        {
            InitializeComponent();
        }

        void searchButton_Click(object sender, RoutedEventArgs e)
        {
            var customerSearchResults = new List<Customer>();

            SearchResults.ItemsSource = customerSearchResults;
        }
    }
}