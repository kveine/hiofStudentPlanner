﻿using Client.Common;
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
            // TODO: Assign a bindable collection of items to this.DefaultViewModel["Items"]
            //this.DefaultViewModel["Submissions"] = await DataSource.GetSubmissionsAsync();
            var submissions = await DataSource.GetSubmissionsAsync();
            this.DefaultViewModel["Submissions"] = submissions;

        }

        private async void AddSubmission_Click(Object sender, RoutedEventArgs e)
        {
            string title = titleInput.Text;
            string description = desciptionInput.Text;
            string dueDate = dueDateInput.Text;
            string courseTitle = CoursesComboBox.SelectedValue.ToString();

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
            var submissions = await DataSource.GetSubmissionsAsync();
            this.DefaultViewModel["Submissions"] = submissions;
        }

         [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
         private void Course_Click(Object sender, ItemClickEventArgs e)
        {
            var course = (Course)e.ClickedItem;
            this.Frame.Navigate(typeof(ItemDetailPage), course);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        private void Courses_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Courses));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        private void Submissions_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Submissions));
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        private void Grades_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Grades));
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
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

    }
}
