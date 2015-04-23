﻿using Client.Common;
using Client.Data;
using Client.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class WeekOverview : Page
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

        public WeekOverview()
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
            //this.Frame.Navigate(typeof(GroupDetailPage));

            //var student = await DataSource.GetStudentAsync(currentStudent);
            //this.DefaultViewModel["Students"] = student;
            //this.DefaultViewModel["Monday"] = await SortLectureWeekOverview(DayOfWeek.Monday);
            //this.DefaultViewModel["Tuesday"] = await SortLectureWeekOverview(DayOfWeek.Tuesday);
            //this.DefaultViewModel["Wednesday"] = await SortLectureWeekOverview(DayOfWeek.Wednesday);
            //this.DefaultViewModel["Thursday"] = await SortLectureWeekOverview(DayOfWeek.Thursday);
            //this.DefaultViewModel["Friday"] = await SortLectureWeekOverview(DayOfWeek.Friday);
        }
        void Student_Click(object sender, RoutedEventArgs e)
        {
            // Determine what group the Button instance represents
            var student = (sender as FrameworkElement).DataContext;

            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            this.Frame.Navigate(typeof(GroupDetailPage), ((Student)student));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        private void Course_Click(Object sender, ItemClickEventArgs e)
        {
            var course = (Course)e.ClickedItem;
            this.Frame.Navigate(typeof(ItemDetailPage), course);
        }

        private void Courses_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Courses), currentStudent);
        }

        /*private async Task<ObservableCollection<Lecture>> SortLectureWeekOverview(DayOfWeek dayOfWeek)
        {
            ObservableCollection<Lecture> dayObs = new ObservableCollection<Lecture>();
            ObservableCollection<Lecture> lectureObs = await DataSource.GetLecturesAsync();
            Student studentObs = await DataSource.GetStudentAsync(currentStudent);

            DayOfWeek day = dayOfWeek;

            foreach (var lecture in lectureObs)
            {
                foreach (var course in studentObs.Courses)
                {
                    if (lecture.Course.CourseId == course.CourseId)
                    {
                        if (lecture.Time.DayOfWeek == day)
                        {
                            dayObs.Add(lecture);
                        }
                    }
                }
            }
            return dayObs;

            //ObservableCollection<Lecture> monday = new ObservableCollection<Lecture>();
            //ObservableCollection<Lecture> tuesday = new ObservableCollection<Lecture>();
            //ObservableCollection<Lecture> wednesday = new ObservableCollection<Lecture>();
            //ObservableCollection<Lecture> thursday = new ObservableCollection<Lecture>();
            //ObservableCollection<Lecture> friday = new ObservableCollection<Lecture>();
            //var lectures = DataSource.GetLecturesAsync();
            //ObservableCollection<Lecture> lectureObs = new ObservableCollection<Lecture>();
            //Student studentObs = await DataSource.GetStudentAsync(currentStudent);

            //foreach (var lecture in lectureObs)
            //{
            //    foreach (var course in studentObs.Courses)
            //    {
            //        if (lecture.Course.CourseId == course.CourseId)
            //        {
            //            DayOfWeek lectureDay = lecture.Time.DayOfWeek;
            //            switch(lectureDay){
            //                case DayOfWeek.Monday:
            //                    monday.Add(lecture);
            //                    break;
            //                case DayOfWeek.Tuesday:
            //                    tuesday.Add(lecture);
            //                    break;
            //                case DayOfWeek.Wednesday:
            //                    wednesday.Add(lecture);
            //                    break;
            //                case DayOfWeek.Thursday:
            //                    thursday.Add(lecture);
            //                    break;
            //                case DayOfWeek.Friday:
            //                    friday.Add(lecture);
            //                    break;
            //            }
            //        }
            //    }
            //}
        }*/

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e")]
        private void Submissions_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Submissions), currentStudent);
        }
        private void Grades_Click(Object sender, RoutedEventArgs e)
        {
            //var student = DataSource.GetStudentAsync();
            //Debug.WriteLine(student);
            this.Frame.Navigate(typeof(Grades), currentStudent);
        }
        private void WeekOverview_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WeekOverview));
        }

        private void Profile_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProfilePage), currentStudent);
        }
        private void LogOut_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupedItemsPage));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
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

        private void itemGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
