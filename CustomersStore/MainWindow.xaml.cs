using System.Windows;

namespace CustomersStore
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new CustomersViewModel();
        }
        private void ColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var model = new CustomersViewModel();
            var columnHeader = sender as System.Windows.Controls.Primitives.DataGridColumnHeader;
            if (columnHeader != null)
            {
                model.Filter(columnHeader);
            }
        }
    }
}
