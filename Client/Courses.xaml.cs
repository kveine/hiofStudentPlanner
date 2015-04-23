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
            //this.Fill_ComboBox();
        }

        private async void Fill_ComboBox()
        {
            ObservableCollection<Course> obsColl =  await DataSource.GetCoursesAsync();

            foreach (var course in obsColl)
            {
               // CoursesComboBox.Items.Add(course[].Title);
            }
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
            this.DefaultViewModel["Courses"] = await DataSource.GetStudentCoursesAsync(currentStudent);
        }

        //Har prøvd å fjerne ubrukte parameter, men da får jeg feil melding. Aner derfor ikke hvordan jeg skal gjøre det. Er mange warnings på dette, skriver bare begrunnelsen her.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        private void Course_Click(Object sender, ItemClickEventArgs e)
        {
            var course = (Course)e.ClickedItem;
            this.Frame.Navigate(typeof(ItemDetailPage), course);
        }

        private void Courses_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Courses));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "sender")]
        private void Submissions_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Submissions));
        }
        private void Grades_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Grades));
        }
        private void WeekOverview_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WeekOverview));
        }
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }

        private async void AddCourse_Click(Object sender, RoutedEventArgs e)
        {
            string selectedCourse = CoursesComboBox.SelectedValue.ToString();

            Student studentObs = await DataSource.GetStudentAsync(currentStudent);
            ObservableCollection<Course> courseObs = await DataSource.GetCoursesAsync();
            ObservableCollection<Course> updatedCourse = new ObservableCollection<Course>();

            string firstname = studentObs.FirstName;
            string lastname = studentObs.LastName;
            string username = studentObs.UserName;
            string password = studentObs.Password;

            foreach (var course in courseObs)
            {
                if (selectedCourse == course.Title)
                {
                    updatedCourse.Add(course);
                }
            }
            foreach (var course in studentObs.Courses)
            {
                updatedCourse.Add(course);
            }
            Student updatedStudentCourses = new Student() { StudentId = currentStudent, FirstName = firstname, LastName = lastname, Password = password, Courses = updatedCourse };
            await DataSource.UpdateStudentAync(updatedStudentCourses, currentStudent);
            /*foreach (var student in studentObs)
            {
                firstname = student.FirstName;
                lastname = student.LastName;
                username = student.UserName;
                password = student.Password;

                foreach (var course in student.Courses)
                {
                    updatedCourse.Add(course);

                }
                Student updatedStudentCourses = new Student() { StudentId = currentStudent, FirstName = firstname, LastName = lastname, Password = password, Courses = updatedCourse };
                await DataSource.UpdateStudentAync(updatedStudentCourses, currentStudent);
            }*/

        }
        private async void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> data = new List<String>();
            ObservableCollection<Course> obsColl = await DataSource.GetCoursesAsync();
            
            foreach(var course in obsColl){
                if (course.Students.Count == 0)
                {
                    data.Add(course.Title);
                }
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


        //Har ikke jobbet med denne enda, skal implementeres ordentlig senere
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "e")]
        private string ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            string value = comboBox.SelectedItem as string;
            return value;
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
