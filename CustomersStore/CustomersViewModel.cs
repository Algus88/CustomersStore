using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls.Primitives;

namespace CustomersStore
{
    public class CustomersViewModel
    {
        ApplicationContext db = new ApplicationContext();
        RelayCommand? addCommand;
        RelayCommand? editCommand;
        RelayCommand? deleteCommand;

        public ObservableCollection<Customer> Customers { get; set; }
        public CustomersViewModel()
        {
            //db.Database.EnsureCreated();
            db.Customers.Load();
            Customers = db.Customers.Local.ToObservableCollection();
        }
        // команда добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      CustomerWindow customerWindow = new CustomerWindow(new Customer());
                      if (customerWindow.ShowDialog() == true)
                      {
                          Customer customer = customerWindow.Customer;
                          db.Customers.Add(customer);
                          db.SaveChangesAsync();
                      }
                  }));
            }
        }
        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {

                      Customer? customer = selectedItem as Customer;
                      if (customer == null) return;

                      Customer vm = new Customer
                      {
                          Id = customer.Id,
                          Name = customer.Name,
                          CompanyName = customer.CompanyName,
                          Phone = customer.Phone,
                          Email = customer.Email
                      };
                      CustomerWindow customerWindow = new CustomerWindow(vm);


                      if (customerWindow.ShowDialog() == true)
                      {
                          customer.Name = customerWindow.Customer.Name;
                          customer.CompanyName = customerWindow.Customer.CompanyName;
                          customer.Phone = customerWindow.Customer.Phone;
                          customer.Email = customerWindow.Customer.Email;
                          db.Customers.Update(customer);
                          db.SaveChanges();
                      }
                  }));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      // получаем выделенный объект
                      Customer? customer = selectedItem as Customer;
                      if (customer == null) return;
                      db.Customers.Remove(customer);
                      db.SaveChanges();
                  }));
            }
        }
           
        public void Filter(DataGridColumnHeader columnHeader)
        {
            string columnName = columnHeader.Content.ToString();
            string direction = Direction(columnHeader.SortDirection);
            SqlParameter columnFilter = new("@filterParameter", columnName);
            SqlParameter directionFilter = new("@sortingDirection", direction);
            var result = db.Customers.FromSqlRaw("Sorting @filterParameter, @sortingDirection", columnFilter, directionFilter);
            Customers.Clear();
            foreach (var item in result)
            {
                Customers.Add(item);
            }
        }
        private string Direction(ListSortDirection? sortDirection)
        {
            if (sortDirection is null || sortDirection.Value.Equals(1)) return "D";
            return "A";
        }
    }
}

