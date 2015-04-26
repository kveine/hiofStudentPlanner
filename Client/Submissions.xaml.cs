using Client.Common;
using Client.Data;
using Client.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Client
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class Submissions : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private int currentStudent;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public Submissions()
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
            var submissions = await DataSource.GetSubmissionsAsync(currentStudent);
            this.DefaultViewModel["Submissions"] = submissions;

        }

        private void SubmissionView_SubmissionClick(object sender, ItemClickEventArgs e)
        {
            var submission = (Submission)e.ClickedItem;
            this.Frame.Navigate(typeof(SubmissionDetailPage), submission);
        }

        private async void AddSubmission_Click(Object sender, RoutedEventArgs e)
        {
            string title = titleInput.Text;
            string description = desciptionInput.Text;
            string dueDate = dueDateInput.Text;
            string courseTitle = CoursesComboBox.SelectedValue.ToString();

            if(title == "" || dueDate == "" || courseTitle == ""){
                MessageDialog md = new MessageDialog("");
                await md.ShowAsync();
            }
            ObservableCollection<Course> courses = await DataSource.GetCoursesAsync();

            Course course = new Course();
            Student student = new Student() { StudentId = currentStudent };

            foreach (var entry in courses)
            {
                if (entry.Title == courseTitle)
                {
                    course = entry;
                }
            }

            await DataSource.AddSubmissionAsync(title, course, student, description, dueDate);
            var submissions = await DataSource.GetSubmissionsAsync(currentStudent);
            this.DefaultViewModel["Submissions"] = submissions;
        }

        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> data = new List<String>();
            ObservableCollection<Course> obsColl = await DataSource.GetCoursesAsync();
            
            foreach(var course in obsColl){
                foreach (var student in course.Students)
                {
                    if (student.StudentId != currentStudent)
                    {
                        data.Add(course.Title);
                        break;
                    }
                }
            }
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = data;
            if (data.Count != 0)
            {
                comboBox.SelectedIndex = 0;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var submission = e.OriginalSource;
            Handle(sender as CheckBox, submission);

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var submission = e.OriginalSource;
            Handle(sender as CheckBox, submission);
        }

        async void Handle(CheckBox checkBox, Object submission)
        {
            bool flag = checkBox.IsChecked.Value;
            String content = checkBox.Content.ToString();
            int submissionId = int.Parse(content);
            Submission currentSubmission = await DataSource.GetSubmissionAsync(submissionId);

            
            if (flag)
            {
                Submission updatedSubmission = new Submission()
                {
                    Title = currentSubmission.Title,
                    SubmissionId = currentSubmission.SubmissionId,
                    Completed = true,
                    Course = currentSubmission.Course,
                    Student = currentSubmission.Student,
                    Description = currentSubmission.Description,
                    DueDate = currentSubmission.DueDate
                };

                await DataSource.UpdateSubmissionAync(updatedSubmission);
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                currentStudent = (int)e.Parameter;
            }
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
