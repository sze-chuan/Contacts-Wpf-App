using System.Collections.ObjectModel;
using System.Windows;
using ContactsApp.Models;
using ContactsApp.Utilities;
using ContactsApp.ViewModels;
using ContactsApp.Views;

namespace ContactsApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var contactMainViewModel = new ContactMainViewModel(ContactUtility.GenerateContacts());
            var contactMain = new ContactMain {DataContext = contactMainViewModel};
            contactMain.Show();
        }
    }
}
