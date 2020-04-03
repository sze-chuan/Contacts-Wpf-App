using System.Collections.ObjectModel;
using System.Linq;
using ContactsApp.Models;
using NUnit.Framework;
using ContactsApp.ViewModels;

namespace UnitTests
{
    [TestFixture]
    public class Tests
    {
        private ContactMainViewModel ViewModel { get; set; }

        [SetUp]
        public void Setup()
        {
            ViewModel = new ContactMainViewModel();
        }

        [TestCase(ContactInfoMode.Add, true)]
        [TestCase(ContactInfoMode.Edit, true)]
        [TestCase(ContactInfoMode.None, false)]
        public void ShowContactInfoTests(ContactInfoMode mode, bool expected)
        {
            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.ShowContactInfo);
        }

        [TestCase(ContactInfoMode.Add, false)]
        [TestCase(ContactInfoMode.Edit, true)]
        [TestCase(ContactInfoMode.None, false)]
        public void ShowDeleteButtonTests(ContactInfoMode mode, bool expected)
        {
            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.ShowDeleteButton);
        }

        [TestCase(ContactInfoMode.Add)]
        [TestCase(ContactInfoMode.Edit)]
        [TestCase(ContactInfoMode.None)]
        public void ContactHeaderTextTests(ContactInfoMode mode)
        {
            var expected = mode == ContactInfoMode.Add ? "Add New Contact" : "Edit Contact";

            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.ContactHeaderText);
        }

        [TestCase(ContactInfoMode.Add, false)]
        [TestCase(ContactInfoMode.Edit, false)]
        [TestCase(ContactInfoMode.None, true)]
        public void CanAddNewCommandTests(ContactInfoMode mode, bool expected)
        {
            ViewModel.Mode = mode; 
            
            Assert.AreEqual(expected, ViewModel.AddNewContactCommand.CanExecute(null));
        }

        [Test]
        public void AddNewCommandTest()
        {
            ViewModel.AddNewContactCommand.Execute(null);

            Assert.AreEqual(ContactInfoMode.Add, ViewModel.Mode);
            Assert.IsNotNull(ViewModel.SelectedContact);
        }

        [Test]
        public void SaveContactWhenContactIsNewTest()
        {
            ViewModel.Mode = ContactInfoMode.Add;
            ViewModel.SelectedContact = new Contact {FirstName = "test"};
            ViewModel.SaveContactCommand.Execute(null);

            Assert.AreEqual(ContactInfoMode.None, ViewModel.Mode);
            Assert.IsTrue(ViewModel.Contacts.Any(x => x.FirstName == "test"));
        }

        [Test]
        public void SaveContactWhenContactIsExistingTest()
        {
            var contact = new Contact {FirstName = "test"};
            ViewModel.Contacts = new ObservableCollection<Contact>{ contact };
            ViewModel.Mode = ContactInfoMode.Edit;
            ViewModel.SelectedContact = contact;
            ViewModel.SelectedContact.LastName = "test1";
            ViewModel.SaveContactCommand.Execute(null);

            Assert.AreEqual(ContactInfoMode.None, ViewModel.Mode);
            Assert.AreEqual(1, ViewModel.Contacts.Count);
            Assert.IsTrue(ViewModel.Contacts.Any(x => x.LastName == "test1"));
        }

        [Test]
        public void DoNotSaveContactWhenModeIsNoneTest()
        {
            ViewModel.Mode = ContactInfoMode.None;
            ViewModel.SelectedContact = new Contact { FirstName = "test" };
            ViewModel.SaveContactCommand.Execute(null);

            Assert.AreEqual(ContactInfoMode.None, ViewModel.Mode);
            Assert.AreEqual(0, ViewModel.Contacts.Count);
        }

        [Test]
        public void EditContactWhenCommandParameterIsNotNullTest()
        {
            var contact = new Contact {FirstName = "test"};
            ViewModel.EditContactCommand.Execute(contact);

            Assert.AreEqual(ContactInfoMode.Edit, ViewModel.Mode);
            Assert.AreEqual(contact, ViewModel.SelectedContact);
        }

        [Test]
        public void EditContactWhenCommandParameterIsNullTest()
        {
            ViewModel.EditContactCommand.Execute(null);

            Assert.AreEqual(ContactInfoMode.None, ViewModel.Mode);
            Assert.IsNull(ViewModel.SelectedContact);
        }

        [Test]
        public void DeleteContactWhenSelectedContactIsNotNullTest()
        {
            var contact = new Contact { FirstName = "test" };
            ViewModel.Contacts = new ObservableCollection<Contact> { contact };
            ViewModel.Mode = ContactInfoMode.Edit;
            ViewModel.SelectedContact = contact;

            ViewModel.DeleteCommand.Execute(contact);

            Assert.AreEqual(ContactInfoMode.None, ViewModel.Mode);
            Assert.AreEqual(0, ViewModel.Contacts.Count);
        }

        [Test]
        public void DoNotDeleteContactWhenSelectedContactIsNullTest()
        {
            var contact = new Contact { FirstName = "test" };
            ViewModel.Contacts = new ObservableCollection<Contact> { contact };
            ViewModel.Mode = ContactInfoMode.Edit;

            ViewModel.DeleteCommand.Execute(contact);

            Assert.AreEqual(ContactInfoMode.None, ViewModel.Mode);
            Assert.AreEqual(1, ViewModel.Contacts.Count);
        }

        [TestCase(ContactInfoMode.Add, false)]
        [TestCase(ContactInfoMode.Edit, true)]
        [TestCase(ContactInfoMode.None, false)]
        public void CanDeleteContactTests(ContactInfoMode mode, bool expected)
        {
            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.DeleteCommand.CanExecute(null));
        }
    }
}