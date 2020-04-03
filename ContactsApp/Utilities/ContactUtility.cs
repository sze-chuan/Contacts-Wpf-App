using System.Collections.Generic;
using ContactsApp.Models;

namespace ContactsApp.Utilities
{
    public class ContactUtility
    {
        /// <summary>
        /// Method to pre-populate data
        /// </summary>
        /// <returns>List of contacts</returns>
        public static IList<Contact> GenerateContacts()
        {
            var contacts = new List<Contact>();

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

                contacts.Add(contact);
            }

            return contacts;
        }
    }
}
