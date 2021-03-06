﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Client.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Client.DataModel;
using System.Collections.ObjectModel;

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace Client
{
    /// <summary>
    /// A page that displays details for a single item within a group while allowing gestures to
    /// flip through other items belonging to the same group.
    /// </summary>
    public sealed partial class SubmissionDetailPage : Page
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

        public SubmissionDetailPage()
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
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.DefaultViewModel["Submission"] = (Submission)e.NavigationParameter;
        }

        //Did not have time to finsish implementing this method, so it is never used.
        private async void UpdateSubmission_Click(object sender, RoutedEventArgs e)
        {
            string courseTitle = courseTitleInput.Text;
            string title = titleInput.Text;
            string description = descriptionInput.Text;
            string dueDate = datePickerInput.ToString();
            int submissionId = int.Parse(id.Text);
            Student student = await DataSource.GetStudentAsync(currentStudent);

            if (student != null)
            {
                ObservableCollection<Course> coursesObs = student.Courses;
                Course course = new Course();

                foreach (var entry in coursesObs)
                {
                    if (entry.Title == courseTitle)
                    {
                        course = entry;
                    }
                }
                Submission updatedSubmission = new Submission() { SubmissionId = submissionId, Course = course, Description = description, Student = student, DueDate = dueDate, Title = title, Completed = false };
            }
            else
            {

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
