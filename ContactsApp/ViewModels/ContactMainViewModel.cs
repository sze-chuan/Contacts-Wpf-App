using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using ContactsApp.Commands;
using ContactsApp.Models;
using System.Windows.Input;

namespace ContactsApp.ViewModels
{
    public class ContactMainViewModel : ViewModelBase
    {
        public ContactMainViewModel(IList<Contact> preGeneratedContacts = null)
        {
            Contacts = preGeneratedContacts != null ? new ObservableCollection<Contact>(preGeneratedContacts) : new ObservableCollection<Contact>();
            SetupContactsView();
            Mode = ApplicationMode.None;
        }

        private Contact _selectedContact;

        private ObservableCollection<Contact> _contacts;

        private ICollectionView ContactsView { get; set; }

        private ApplicationMode _mode;

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

        public ApplicationMode Mode
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

        public bool ShowContactInfo => Mode == ApplicationMode.Add || Mode == ApplicationMode.Edit;

        public bool ShowDeleteButton => Mode == ApplicationMode.Edit;

        public string ContactHeaderText => Mode == ApplicationMode.Add ? "Add New Contact" : "Edit Contact";

        #region Command related
        public ICommand AddNewContactCommand => _addNewContactCommand ??= new DelegateCommand(AddNewContact, CanAddNewContact);

        public ICommand SaveContactCommand => _saveContactCommand ??= new DelegateCommand(SaveContact, CanSaveContact);

        public ICommand EditContactCommand => _editContactCommand ??= new DelegateCommand(EditContact, CanEditContact);

        public ICommand CancelCommand => _cancelCommand ??= new DelegateCommand(CancelContact, CanCancelContact);

        public ICommand DeleteCommand => _deleteContactCommand ??= new DelegateCommand(DeleteContact, CanDeleteContact);
        
        private void AddNewContact(object commandParameter)
        {
            Mode = ApplicationMode.Add;
            SelectedContact = new Contact();
        }

        private bool CanAddNewContact(object commandParameter)
        {
            return Mode == ApplicationMode.None;
        }

        private void SaveContact(object commandParameter)
        {
            if (Mode == ApplicationMode.Add)
            {
                Contacts.Add(SelectedContact);
            }
            else if (Mode == ApplicationMode.Edit)
            {
                var contactIndex = Contacts.IndexOf(SelectedContact);
                Contacts[contactIndex].FirstName = SelectedContact.FirstName;
            }

            Mode = ApplicationMode.None;
        }

        private bool CanSaveContact(object commandParameter)
        {
            return true;
        }

        private void CancelContact(object commandParameter)
        {
            Mode = ApplicationMode.None;
        }
        private bool CanCancelContact(object commandParameter)
        {
            return true;
        }

        private void EditContact(object commandParameter)
        {
            if (commandParameter != null)
            { 
                Mode = ApplicationMode.Edit;
                SelectedContact = (Contact)commandParameter;
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
            }

            Mode = ApplicationMode.None;
        }
        private bool CanDeleteContact(object commandParameter)
        {
            return Mode == ApplicationMode.Edit;
        }
        #endregion

        private void SetupContactsView()
        {
            ContactsView = CollectionViewSource.GetDefaultView(Contacts);
            ContactsView.Filter = contact => string.IsNullOrEmpty(SearchText) || ((Contact)contact).FullName.Contains(SearchText, StringComparison.InvariantCultureIgnoreCase);
            ContactsView.SortDescriptions.Add(new SortDescription("FullName", ListSortDirection.Ascending));
        }
    }
    public enum ApplicationMode
    {
        Add,
        Edit,
        None
    }
}
