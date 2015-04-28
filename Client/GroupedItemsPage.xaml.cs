using Client.Common;
using Client.Data;
using Client.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Grouped Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234231

namespace Client
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class GroupedItemsPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private int currentStudent { get; set; }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public GroupedItemsPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.DefaultViewModel["Students"] = await DataSource.GetStudentsAsync();
        }

        /// <summary>
        /// Handles the Click event of the Register control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void Register_Click(Object sender, RoutedEventArgs e)
        {
            string firstName = firstNameInput.Text;
            string lastName = lastNameInput.Text;
            string userName = userNameInput.Text;
            string password = passwordInput.Password;
            ObservableCollection<Student> studentsObs = await DataSource.GetStudentsAsync();
            bool userNameTaken = false;

            if (studentsObs != null)
            {
                foreach (var student in studentsObs)
                {
                    if (student.UserName == userName)
                    {
                        userNameTaken = true;
                        MessageDialog md = new MessageDialog("Username is already taken!");
                        await md.ShowAsync();
                    }
                }
                if (firstName == "" || lastName == "" || userName == "" || password == "")
                {
                    MessageDialog md = new MessageDialog("All fields must be filled out!");
                    await md.ShowAsync();
                }
                else if (!userNameTaken)
                {
                    await DataSource.AddStudentAsync(firstName, lastName, userName, password);
                    MessageDialog md = new MessageDialog("User " + userName + " is created");
                    await md.ShowAsync();
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("User not created, check your internet connection and try again");
                await md.ShowAsync();
            }
        }


        /// <summary>
        /// Handles the Click event of the LogIn control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void LogIn_Click(Object sender, RoutedEventArgs e)
        {
            string userName = usernameLogIn.Text;
            string password = PasswordLogIn.Password;

            if (userName == "" || password == "")
            {
                MessageDialog md = new MessageDialog("Username or password is incorrect!");
                await md.ShowAsync();
            }

            bool registered = false;
            var students = await DataSource.GetStudentsAsync();

            if (students != null)
            {
                foreach (var item in students)
                {
                    if (item.UserName == userName && item.Password == password)
                    {
                        registered = true;
                        currentStudent = item.StudentId;
                        this.Frame.Navigate(typeof(WeekOverview), currentStudent);
                    }
                }
                if (!registered)
                {
                    MessageDialog md = new MessageDialog("Username or password is incorrect!");
                    await md.ShowAsync();
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("Unable to log in, check your internet connection and try again");
                await md.ShowAsync();
            }
        }

        #region NavigationHelper registration

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the pending navigation that will load the current Page. Usually the most relevant property to examine is Parameter.</param>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="GridCS.Common.NavigationHelper.LoadState" />
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState" />.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current source of a parent Frame.
        /// </summary>
        /// <param name="e">Event data that can be examined by overriding code. The event data is representative of the navigation that has unloaded the current Page.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}