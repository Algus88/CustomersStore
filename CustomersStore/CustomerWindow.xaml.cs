using System.Windows;
using System.Windows.Controls;


namespace CustomersStore
{
    public partial class CustomerWindow : Window
    {
        public Customer Customer{ get; private set; }
        public CustomerWindow(Customer customer)
        {
            InitializeComponent();
            Customer = customer;
            DataContext = Customer;
        }
        void Accept_Click(object sender, RoutedEventArgs e)
        {

            DialogResult = true;
        }
        void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            MessageBox.Show(e.Error.ErrorContent.ToString());
        }
    }
}
