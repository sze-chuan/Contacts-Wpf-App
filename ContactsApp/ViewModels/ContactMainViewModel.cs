using System.Collections.ObjectModel;
using ContactsApp.Commands;
using ContactsApp.Models;
using System.Windows.Input;

namespace ContactsApp.ViewModels
{
    class ContactMainViewModel : ViewModelBase
    {
        public ContactMainViewModel()
        {
            Contacts = new ObservableCollection<Contact>();
            GenerateData();
        }

        private bool _showContactInfo;

        private Contact _selectedContact;

        private ObservableCollection<Contact> _contacts;

        public Contact SelectedContact
        {
            get => _selectedContact;
            set { _selectedContact = value; OnPropertyChanged(nameof(SelectedContact)); }
        }

        public ObservableCollection<Contact> Contacts
        {
            get => _contacts;
            set { _contacts = value; OnPropertyChanged(nameof(Contacts)); }
        }
        public ICommand AddNewContactCommand => new DelegateCommand(AddNewContact, CanAddNewContact);

        public ICommand SaveContactCommand => new DelegateCommand(SaveContact, CanSaveContact);

        public bool ShowContactInfo
        {
            get => _showContactInfo;
            set { _showContactInfo = value; OnPropertyChanged(nameof(ShowContactInfo));
            }
        }
 
        private void AddNewContact(object commandParameter)
        {
            ShowContactInfo = true;
            SelectedContact = new Contact();
        }

        private bool CanAddNewContact(object commandParameter)
        {
            return true;
        }

        private void SaveContact(object commandParameter)
        {
            Contacts.Add(SelectedContact);
            ShowContactInfo = false;
        }

        private bool CanSaveContact(object commandParameter)
        {
            return true;
        }

        // TODO: For testing purpose. To remove later.
        private void GenerateData()
        {
            for (var i = 0; i < 8; i++)
            {
                var contact = new Contact
                {
                    FirstName = "Person",
                    LastName = "Test" + i,
                    AddressLine1 = "Address Line 1",
                    AddressLine2 = "Address Line 2",
                    Country = "Singapore",
                    Email = "test@mail.com",
                    Mobile = "91234554" + i,
                    PostalCode = "123456"
                };

                Contacts.Add(contact);
            }
        }
    }
}
