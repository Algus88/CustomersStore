using Azure;
using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;


namespace CustomersStore
{
    public class Customer : INotifyPropertyChanged, IDataErrorInfo
    {
        private int _id;
        private string _name;
        private string _companyName;
        private string _phone;
        private string _email;

        [Key]
        public int Id { get { return _id; } set { _id = value; } }
        public string Name { 
            get {return _name; }
            set 
            { 
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string CompanyName {
            get { return _companyName; }
            set 
            { 
                _companyName = value;
                OnPropertyChanged("CompanyName");
            } 
        }
        public string Phone {
            get { return _phone; }
            set 
            {
                _phone = value;
                OnPropertyChanged("Phone");
            } 
        }
        public string Email {
            get { return _email; }
            set 
            { 
                _email = value;
                OnPropertyChanged("Email");
            } 
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        if (!IsNameUnique(Name, Id))
                        {
                            error = "This name is used. Please use another one";
                        }
                        break;
                    case "CompanyName":
                        if (CompanyName?.Length > 255)
                        {
                            error = "Comany name is too long";
                        }
                        break;
                    case "Phone":
                        
                        if (!Int32.TryParse(Phone, out var number))
                        {
                            error = "Phone number must contains only digits";
                        }
                        break;
                    case "Email":
                        if(!RegexHelper.IsValidEmail(Email))
                        {
                            error = "Please enter correct email";
                        }
                        break;
                }
                return error;
            }
        }
    
        public string Error => throw new System.NotImplementedException();

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private bool IsNameUnique(string name, int id)
        {
            ApplicationContext db = new ApplicationContext();
            var customer = db.Customers.Where(x => x.Name == name).FirstOrDefault();
            if(customer == null || customer.Id == id) return true;
            return false;
        }
    }
}
