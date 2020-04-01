using ContactsApp.Commands;
using ContactsApp.Models;
using System.Windows.Input;

namespace ContactsApp.ViewModels
{
    class ContactMainViewModel : ViewModelBase
    {
        private bool _showContactInfo;

        public Contact SelectedContact { get; set; }

        public ICommand AddNewContactCommand { get; set; }

        public bool ShowContactInfo
        {
            get => _showContactInfo;
            set { _showContactInfo = value; OnPropertyChanged(nameof(ShowContactInfo));
            }
        }

        public ContactMainViewModel()
        {
            AddNewContactCommand = new DelegateCommand(AddNewContact, CanAddNewContact);
            ShowContactInfo = false;
        }

        private void AddNewContact(object commandParameter)
        {
            ShowContactInfo = true;
        }

        private bool CanAddNewContact(object commandParameter)
        {
            return true;
        }
    }
}
