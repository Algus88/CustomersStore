using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomersStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
