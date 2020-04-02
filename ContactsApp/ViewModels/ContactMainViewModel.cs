using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
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
            ContactsView = CollectionViewSource.GetDefaultView(Contacts);
            ContactsView.Filter = contact => string.IsNullOrEmpty(SearchText) || ((Contact)contact).FullName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            Mode = ContactInfoMode.None;
            GenerateData();
        }

        private Contact _selectedContact;

        private ObservableCollection<Contact> _contacts;

        private ICollectionView ContactsView { get; }

        private ContactInfoMode _mode;

        private string _searchText;

        private DelegateCommand _addNewContactCommand;
        private DelegateCommand _saveContactCommand;
        private DelegateCommand _editContactCommand;
        private DelegateCommand _cancelCommand;
        private DelegateCommand _deleteContactCommand;

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

        public ContactInfoMode Mode
        {
            get => _mode;
            set { _mode = value; OnPropertyChanged(nameof(Mode)); OnPropertyChanged(nameof(ShowContactInfo)); OnPropertyChanged(nameof(ShowDeleteButton)); OnPropertyChanged(nameof(ContactHeaderText)); }
        }

        public string SearchText
        {
            get => _searchText;
            set {
                if (value == _searchText)
                    return;
                _searchText = value;
                ContactsView.Refresh();
                OnPropertyChanged(nameof(SearchText));
            }
        }

        public ICommand AddNewContactCommand => _addNewContactCommand ??= new DelegateCommand(AddNewContact, CanAddNewContact);

        public ICommand SaveContactCommand => _saveContactCommand ??= new DelegateCommand(SaveContact, CanSaveContact);

        public ICommand EditContactCommand => _editContactCommand ??= new DelegateCommand(EditContact, CanEditContact);

        public ICommand CancelCommand => _cancelCommand ??= new DelegateCommand(CancelContact, CanCancelContact);

        public ICommand DeleteCommand => _deleteContactCommand ??= new DelegateCommand(DeleteContact, CanDeleteContact);

        public bool ShowContactInfo => Mode == ContactInfoMode.Add || Mode == ContactInfoMode.Edit;

        public bool ShowDeleteButton => Mode == ContactInfoMode.Edit;

        public string ContactHeaderText => Mode == ContactInfoMode.Add ? "Add New Contact" : "Edit Contact";

        #region Command related
        private void AddNewContact(object commandParameter)
        {
            Mode = ContactInfoMode.Add;
            SelectedContact = new Contact();
            _addNewContactCommand.InvokeCanExecuteChanged();
        }

        private bool CanAddNewContact(object commandParameter)
        {
            return Mode == ContactInfoMode.None;
        }

        private void SaveContact(object commandParameter)
        {
            if (Mode == ContactInfoMode.Add)
            {
                Contacts.Add(SelectedContact);
            }
            else
            {
                var contactIndex = Contacts.IndexOf(SelectedContact);
                Contacts[contactIndex].FirstName = SelectedContact.FirstName;
            }

            Mode = ContactInfoMode.None;
        }

        private bool CanSaveContact(object commandParameter)
        {
            return true;
        }

        private void CancelContact(object commandParameter)
        {
            SetContactInfoModeToNone();
        }
        private bool CanCancelContact(object commandParameter)
        {
            return true;
        }

        private void EditContact(object commandParameter)
        {
            if (commandParameter != null)
            { 
                Mode = ContactInfoMode.Edit;
                SelectedContact = (Contact)commandParameter;
                _addNewContactCommand.InvokeCanExecuteChanged();
                _deleteContactCommand.InvokeCanExecuteChanged();
            }
        }

        private bool CanEditContact(object commandParameter)
        {
            return true;
        }

        private void DeleteContact(object commandParameter)
        {
            if (SelectedContact != null)
            {
                Contacts.Remove(SelectedContact);
                SetContactInfoModeToNone();
            }
        }
        private bool CanDeleteContact(object commandParameter)
        {
            return Mode == ContactInfoMode.Edit;
        }
        #endregion

        private void SetContactInfoModeToNone()
        {
            Mode = ContactInfoMode.None;
            _addNewContactCommand.InvokeCanExecuteChanged();
        }

        // TODO: For testing purpose. To remove later.
        private void GenerateData()
        {
            for (var i = 0; i < 100; i++)
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

        public enum ContactInfoMode
        {
            Add,
            Edit,
            None
        }
    }
}
