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

        [TestCase(ApplicationMode.Add, true)]
        [TestCase(ApplicationMode.Edit, true)]
        [TestCase(ApplicationMode.None, false)]
        public void ShowContactInfoTests(ApplicationMode mode, bool expected)
        {
            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.ShowContactInfo);
        }

        [TestCase(ApplicationMode.Add, false)]
        [TestCase(ApplicationMode.Edit, true)]
        [TestCase(ApplicationMode.None, false)]
        public void ShowDeleteButtonTests(ApplicationMode mode, bool expected)
        {
            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.ShowDeleteButton);
        }

        [TestCase(ApplicationMode.Add)]
        [TestCase(ApplicationMode.Edit)]
        [TestCase(ApplicationMode.None)]
        public void ContactHeaderTextTests(ApplicationMode mode)
        {
            var expected = mode == ApplicationMode.Add ? "Add New Contact" : "Edit Contact";

            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.ContactHeaderText);
        }

        [TestCase(ApplicationMode.Add, false)]
        [TestCase(ApplicationMode.Edit, false)]
        [TestCase(ApplicationMode.None, true)]
        public void CanAddNewCommandTests(ApplicationMode mode, bool expected)
        {
            ViewModel.Mode = mode; 
            
            Assert.AreEqual(expected, ViewModel.AddNewContactCommand.CanExecute(null));
        }

        [Test]
        public void AddNewCommandTest()
        {
            ViewModel.AddNewContactCommand.Execute(null);

            Assert.AreEqual(ApplicationMode.Add, ViewModel.Mode);
            Assert.IsNotNull(ViewModel.SelectedContact);
        }

        [Test]
        public void SaveContactWhenContactIsNewTest()
        {
            ViewModel.Mode = ApplicationMode.Add;
            ViewModel.SelectedContact = new Contact {FirstName = "test"};
            ViewModel.SaveContactCommand.Execute(null);

            Assert.AreEqual(ApplicationMode.None, ViewModel.Mode);
            Assert.IsTrue(ViewModel.Contacts.Any(x => x.FirstName == "test"));
        }

        [Test]
        public void SaveContactWhenContactIsExistingTest()
        {
            var contact = new Contact {FirstName = "test"};
            ViewModel.Contacts = new ObservableCollection<Contact>{ contact };
            ViewModel.Mode = ApplicationMode.Edit;
            ViewModel.SelectedContact = contact;
            ViewModel.SelectedContact.LastName = "test1";
            ViewModel.SaveContactCommand.Execute(null);

            Assert.AreEqual(ApplicationMode.None, ViewModel.Mode);
            Assert.AreEqual(1, ViewModel.Contacts.Count);
            Assert.IsTrue(ViewModel.Contacts.Any(x => x.LastName == "test1"));
        }

        [Test]
        public void DoNotSaveContactWhenModeIsNoneTest()
        {
            ViewModel.Mode = ApplicationMode.None;
            ViewModel.SelectedContact = new Contact { FirstName = "test" };
            ViewModel.SaveContactCommand.Execute(null);

            Assert.AreEqual(ApplicationMode.None, ViewModel.Mode);
            Assert.AreEqual(0, ViewModel.Contacts.Count);
        }

        [Test]
        public void EditContactWhenCommandParameterIsNotNullTest()
        {
            var contact = new Contact {FirstName = "test"};
            ViewModel.EditContactCommand.Execute(contact);

            Assert.AreEqual(ApplicationMode.Edit, ViewModel.Mode);
            Assert.AreEqual(contact, ViewModel.SelectedContact);
        }

        [Test]
        public void EditContactWhenCommandParameterIsNullTest()
        {
            ViewModel.EditContactCommand.Execute(null);

            Assert.AreEqual(ApplicationMode.None, ViewModel.Mode);
            Assert.IsNull(ViewModel.SelectedContact);
        }

        [Test]
        public void DeleteContactWhenSelectedContactIsNotNullTest()
        {
            var contact = new Contact { FirstName = "test" };
            ViewModel.Contacts = new ObservableCollection<Contact> { contact };
            ViewModel.Mode = ApplicationMode.Edit;
            ViewModel.SelectedContact = contact;

            ViewModel.DeleteCommand.Execute(contact);

            Assert.AreEqual(ApplicationMode.None, ViewModel.Mode);
            Assert.AreEqual(0, ViewModel.Contacts.Count);
        }

        [Test]
        public void DoNotDeleteContactWhenSelectedContactIsNullTest()
        {
            var contact = new Contact { FirstName = "test" };
            ViewModel.Contacts = new ObservableCollection<Contact> { contact };
            ViewModel.Mode = ApplicationMode.Edit;

            ViewModel.DeleteCommand.Execute(contact);

            Assert.AreEqual(ApplicationMode.None, ViewModel.Mode);
            Assert.AreEqual(1, ViewModel.Contacts.Count);
        }

        [TestCase(ApplicationMode.Add, false)]
        [TestCase(ApplicationMode.Edit, true)]
        [TestCase(ApplicationMode.None, false)]
        public void CanDeleteContactTests(ApplicationMode mode, bool expected)
        {
            ViewModel.Mode = mode;

            Assert.AreEqual(expected, ViewModel.DeleteCommand.CanExecute(null));
        }
    }
}