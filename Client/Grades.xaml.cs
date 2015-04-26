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
            var grades = await DataSource.GetGradesAsync(currentStudent);
            this.DefaultViewModel["Grades"] = grades;
            OverAllGrade();
        }

        /// <summary>
        /// Calculates the Overs all grade for the current student.
        /// </summary>
        private async void OverAllGrade()
        {
            int count = 0;
            int gradesValue = 0;
            ObservableCollection<Grade> grades = await DataSource.GetGradesAsync(currentStudent);
            foreach(var grade in grades){
                switch (grade.Value)
                {
                    case GradeValue.A:
                        gradesValue += 6;
                        count++;
                        break;
                    case GradeValue.B:
                        gradesValue += 5;
                        count++;
                        break;
                    case GradeValue.C:
                        gradesValue += 4;
                        count++;
                        break;
                    case GradeValue.D:
                        gradesValue += 3;
                        count++;
                        break;
                    case GradeValue.E:
                        gradesValue += 2;
                        count++;
                        break;
                    case GradeValue.F:
                       gradesValue += 1;
                        count++;
                        break;
                }
            }

            double overallGrade1 = gradesValue / count;

            string overAllGradeOutput = overallGrade1.ToString();
            overallGrade.Text = overAllGradeOutput;
            
        }

        /// <summary>
        /// Handles the Click event of the AddGrade control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void AddGrade_Click(Object sender, RoutedEventArgs e)
        {
            if (CoursesComboBox.SelectedValue != null)
            {
                bool courseInGradeAdded = false;
                ObservableCollection<Grade> gradesObs = await DataSource.GetGradesAsync(currentStudent);
                string courseTitle = CoursesComboBox.SelectedValue.ToString();
                foreach (var course in gradesObs)
                {
                    if (course.Course.Title == courseTitle)
                    {
                        courseInGradeAdded = true;
                    }
                }

                if (!courseInGradeAdded)
                {
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
                    OverAllGrade();
                }
                else
                {
                    MessageDialog md = new MessageDialog("The course " + courseTitle + " already have a grade");
                    await md.ShowAsync();
                }

            }
            else
            {
                MessageDialog md = new MessageDialog("You already have a grade in all enrolled courses");
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
            var grade = e.OriginalSource;
            Handle(sender as CheckBox, grade);

        }

        /// <summary>
        /// Handles the Unchecked event of the CheckBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var grade = e.OriginalSource;
            Handle(sender as CheckBox, grade);
        }

        /// <summary>
        /// Handles the specified check box.
        /// </summary>
        /// <param name="checkBox">The check box.</param>
        /// <param name="grade">The grade.</param>
        async void Handle(CheckBox checkBox, Object grade)
        {
            bool flag = checkBox.IsChecked.Value;
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
                OverAllGrade();
            }
        }

        /// <summary>
        /// Handles the Loaded event of the GradeComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Handles the Loaded event of the CourseComboBox control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private async void CourseComboBox_Loaded(object sender, RoutedEventArgs e)
        {

            List<string> data = new List<string>();
            Student student = await DataSource.GetStudentAsync(currentStudent);
            ObservableCollection<Course> studentCoursesObs = new ObservableCollection<Course>();
            studentCoursesObs = student.Courses;
            //ObservableCollection<Course> studentCoursesObs = await DataSource.GetStudentCoursesAsync(currentStudent);
            ObservableCollection<Grade> gradeObs = await DataSource.GetGradesAsync(currentStudent);
            ObservableCollection<Course> courseInGradeObs = new ObservableCollection<Course>();

            foreach (var entry in gradeObs)
            {
                courseInGradeObs.Add(entry.Course);
            }
            

            var response = studentCoursesObs.Where(p => !courseInGradeObs.Any(p2 => p2.CourseId == p.CourseId));

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
