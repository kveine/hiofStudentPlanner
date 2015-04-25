using Client.Common;
using Client.Data;
using Client.DataModel;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using Windows.UI.Popups;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace Client
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class Grades : Page
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

        public Grades()
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
            var grades = await DataSource.GetGradesAsync(currentStudent);
            this.DefaultViewModel["Grades"] = grades;
        }
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

        /*private string OverAllGrade()
        {
            var grades = DataSource.GetGradesAsync(currentStudent);
            
        }*/
        private async void AddGrade_Click(Object sender, RoutedEventArgs e)
        {
            string courseTitle = CoursesComboBox.SelectedValue.ToString();
            string grade = GradeComboBox.SelectedValue.ToString();
            GradeValue gradeValue = GradeValue.F;
            string imagePath = "";
            
            switch (grade)
            {
                case "A":
                    gradeValue = GradeValue.A;
                    imagePath = "Assets/Grades/a.png";
                    break;
                case "B":
                     gradeValue = GradeValue.B;
                     imagePath = "Assets/Grades/b.png";
                    break;
                case "C":
                     gradeValue = GradeValue.C;
                     imagePath = "Assets/Grades/c.png";
                    break;
                case "D":
                    gradeValue = GradeValue.D;
                    imagePath = "Assets/Grades/d.png";
                    break;
                case "E":
                    gradeValue = GradeValue.E;
                    imagePath = "Assets/Grades/e.png";
                    break;
                case "F":
                    gradeValue = GradeValue.F;
                    imagePath = "Assets/Grades/f.png";
                    break;
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


            await DataSource.AddGradeAsync(gradeValue, course, student, imagePath);
            var grades = await DataSource.GetGradesAsync(currentStudent);
            this.DefaultViewModel["Grades"] = grades;
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var grade = e.OriginalSource;
            //var grade = sender as CheckBox;
            //Debug.WriteLine(grade.Content);
            Handle(sender as CheckBox, grade);

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var grade = e.OriginalSource;
            Handle(sender as CheckBox, grade);
        }

        //Har ikke jobbet med denne metoden enda, skal implementeres ordentlig senere
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        async void Handle(CheckBox checkBox, Object grade)
        {
            // Use IsChecked.
            bool flag = checkBox.IsChecked.Value;
            Debug.WriteLine(checkBox.Content);
            String content = checkBox.Content.ToString();
            ObservableCollection<Grade> gradeObs = await DataSource.GetGradesAsync(currentStudent);
            if (flag)
            {
                foreach(var entry in gradeObs){
                    if (entry.Course.Title == content)
                    {
                        await DataSource.DeleteGradeAsync(entry.GradeId);
                        var grades = await DataSource.GetGradesAsync(currentStudent);
                        this.DefaultViewModel["Grades"] = grades;
                    }
                }
                //Debug.WriteLine(grade);
            }
        }

        private void GradeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<String> data = new List<String>();
            data.Add(GradeValue.A.ToString());
            data.Add(GradeValue.B.ToString());
            data.Add(GradeValue.C.ToString());
            data.Add(GradeValue.D.ToString());
            data.Add(GradeValue.E.ToString());
            data.Add(GradeValue.F.ToString());
            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = data;
            comboBox.SelectedIndex = 0;
        }

        private async void CourseComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> data = new List<string>();
            ObservableCollection<Course> studentCoursesObs = await DataSource.GetStudentCoursesAsync(currentStudent);
            ObservableCollection<Grade> gradeObs = await DataSource.GetGradesAsync(currentStudent);

            foreach (var course in studentCoursesObs)
            {
                foreach (var grade in gradeObs)
                {
                    if (course.CourseId != grade.Course.CourseId)
                    {
                        foreach (var entry in data)
                        {
                            if (entry.Count() != 0)
                            {
                                data.Add(course.Title);
                            }
                        }
                    }
                }
            }
            //foreach (var grade in gradeObs)
            //{
            //    foreach (var course in studentCoursesObs)
            //    {
            //        if (course.CourseId != grade.Course.CourseId)
            //        {
            //            data.Add(course.Title);
            //        }
            //    }
            //}

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = data;
            if (data.Count != 0)
            {
                comboBox.SelectedIndex = 0;
            }
            
            /*List<String> data = new List<String>();
            //data.Clear();
            ObservableCollection<Course> courseObs = await DataSource.GetCoursesAsync();
            ObservableCollection<Grade> gradeObs = await DataSource.GetGradesAsync(currentStudent);

            foreach(var grade in gradeObs)
            {
                foreach(var course in courseObs){
                    if(grade.Student.StudentId == currentStudent && grade.Course.CourseId ==)
                }
                
                /*foreach(var student in cour){
                    if (student.StudentId == currentStudent)
                    {
                        data.Add(course.Title);
                    }
                }
               
            }

            foreach(var course in courseObs)
            {
                foreach (var student in course.Students)
                {
                    if (student.StudentId == currentStudent)
                    {
                        foreach (var grade in gradeObs)
                        {
                            if (grade.Course.CourseId != course.CourseId)
                            {
                                data.Add(course.Title);
                            }
                        }
                    }
                }
            }

            var comboBox = sender as ComboBox;
            comboBox.ItemsSource = data;
            if(data.Count != 0){
                comboBox.SelectedIndex = 0;
            }*/
            
        }

        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            
            Grade grade = (Grade)e.ClickedItem;
            this.Frame.Navigate(typeof(ItemDetailPage), grade);

            /*var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);*/
        }

        void GradeView_GradeClick(Object sender, ItemClickEventArgs e)
        {
            Grade grade = (Grade)e.ClickedItem;
            this.Frame.Navigate(typeof(GradeDetailPage), grade);
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
