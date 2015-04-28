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
    public sealed partial class Courses : Page
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

        public Courses()
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
            ObservableCollection<Course> studentCoursesObs = await DataSource.GetStudentCoursesAsync(currentStudent); 
            this.DefaultViewModel["Courses"] = studentCoursesObs;
        }

        /// <summary>
        /// Handles the CourseClick event of the CourseView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ItemClickEventArgs"/> instance containing the event data.</param>
        private void CourseView_CourseClick(Object sender, ItemClickEventArgs e)
        {
            var course = (Course)e.ClickedItem;
            this.Frame.Navigate(typeof(CourseDetailPage), course);
        }

        /// <summary>
        /// Handles the Click event of the AddCourse control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AddCourse_Click(Object sender, RoutedEventArgs e)
        {
            if(CoursesComboBox.SelectedValue != null){
                string selectedCourse = CoursesComboBox.SelectedValue.ToString();

                bool courseAdded = false;
                Student student = await DataSource.GetStudentAsync(currentStudent);

                if (student != null)
                {
                    ObservableCollection<Course> studentCoursesObs1 = student.Courses;

                    foreach (var course in studentCoursesObs1)
                    {
                        if (course.Title == selectedCourse)
                        {
                            courseAdded = true;
                        }
                    }

                    if (!courseAdded)
                    {
                        Student studentObs = await DataSource.GetStudentAsync(currentStudent);
                        ObservableCollection<Course> courseObs = await DataSource.GetCoursesAsync();
                        ObservableCollection<Course> updatedCourse = studentObs.Courses;

                        string firstname = studentObs.FirstName;
                        string lastname = studentObs.LastName;
                        string username = studentObs.UserName;
                        string password = studentObs.Password;

                        foreach (var course in courseObs)
                        {
                            if (selectedCourse == course.Title)
                            {
                                updatedCourse.Add(course);
                                break;
                            }
                        }

                        Student updatedStudentCourses = new Student() { StudentId = currentStudent, FirstName = firstname, LastName = lastname, UserName = username, Password = password, Courses = updatedCourse };
                        await DataSource.UpdateStudentAync(updatedStudentCourses, currentStudent);

                        ObservableCollection<Course> studentCoursesObs = await DataSource.GetStudentCoursesAsync(currentStudent);
                        this.DefaultViewModel["Courses"] = studentCoursesObs;
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("You have already enrolled to " + selectedCourse);
                        await md.ShowAsync();
                    }
                }
                else
                {
                    MessageDialog md = new MessageDialog("Could not add course, check your internet connection and try again.");
                    await md.ShowAsync();
                }
                
            }
            else
            {
                MessageDialog md = new MessageDialog("You have already enrolled to all available courses");
                await md.ShowAsync();
            }
        
        }
        /// <summary>
        /// Handles the Loaded event of the ComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> data = new List<String>();
            ObservableCollection<Course> coursesObs = await DataSource.GetCoursesAsync();
            Student student = await DataSource.GetStudentAsync(currentStudent);

            if (coursesObs != null && student != null)
            {
                ObservableCollection<Course> studentCoursesObs = student.Courses;

                var response = coursesObs.Where(p => !studentCoursesObs.Any(p2 => p2.CourseId == p.CourseId));

                foreach (var course in response)
                {
                    data.Add(course.Title);
                }

                var comboBox = sender as ComboBox;
                comboBox.ItemsSource = data;
                if (data.Count != 0)
                {
                    comboBox.SelectedIndex = 0;
                }
            }
            else
            {
                MessageDialog md = new MessageDialog("Could not load courses, check your internet connection and try again.");
                await md.ShowAsync();
            }
            
        }

        /// <summary>
        /// Handles the Checked event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var submission = e.OriginalSource;
            Handle(sender as CheckBox, submission);

        }

        /// <summary>
        /// Handles the Unchecked event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var submission = e.OriginalSource;
            Handle(sender as CheckBox, submission);
        }

        /// <summary>
        /// Handles the specified check box.
        /// </summary>
        /// <param name="checkBox">The check box.</param>
        /// <param name="submission">The submission.</param>
        async void Handle(CheckBox checkBox, Object submission)
        {
            bool flag = checkBox.IsChecked.Value;
            String content = checkBox.Content.ToString();
            int courseId = int.Parse(content);

            if (flag)
            {
                Course selectedCourse = await DataSource.GetCourseAsync(courseId);
                Student student = await DataSource.GetStudentAsync(currentStudent);

                if (student != null)
                {
                    ObservableCollection<Course> updatedCoursesObs = student.Courses;

                    updatedCoursesObs.Remove(updatedCoursesObs.Where(x => x.CourseId == selectedCourse.CourseId).Single());

                    Student updatedStudent = new Student()
                    {
                        StudentId = student.StudentId,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Courses = updatedCoursesObs,
                        Password = student.Password,
                        UserName = student.UserName
                    };
                    await DataSource.UpdateStudentAync(updatedStudent, currentStudent);
                    ObservableCollection<Course> studentCoursesObs = await DataSource.GetStudentCoursesAsync(currentStudent);
                    this.DefaultViewModel["Courses"] = studentCoursesObs;
                }
                else
                {
                    MessageDialog md = new MessageDialog("Could not delete course, check your internet connection and try again.");
                    await md.ShowAsync();
                }
                
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
            if (e.Parameter != null)
            {
                currentStudent = (int)e.Parameter;
            }
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
