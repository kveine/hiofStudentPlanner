﻿using Client.Common;
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
            this.DefaultViewModel["Grades"] = await DataSource.GetGradesAsync();
        }
         private void Course_Click(Object sender, ItemClickEventArgs e)
        {
            var course = (Course)e.ClickedItem;
            this.Frame.Navigate(typeof(ItemDetailPage), course);
        }

        private void Courses_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Courses));
        }

        private void Submissions_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Submissions));
        }
        private void Grades_Click(Object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Grades));
        }

        private async void AddGrade_Click(Object sender, RoutedEventArgs e)
        {
            int courseId = int.Parse(courseIdInput.Text);
            int studentId = int.Parse(studentIdInput.Text);
            string grade = gradeInput.Text;
            Debug.WriteLine(courseId);
            Debug.WriteLine(studentId);
            Debug.WriteLine(grade);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:42015/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                Grade newGrade = new Grade() { CourseId = courseId, StudentId = studentId, Value = grade };
                var jsonSerializer = new DataContractJsonSerializer(typeof(Grade));

                var stream = new MemoryStream();
                jsonSerializer.WriteObject(stream, newGrade);
                stream.Position = 0;   // Make sure to rewind the cursor before you try to read the stream
                var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/Grades", content);

                response.EnsureSuccessStatusCode();
                //var jsonSerializer = new DataContractJsonSerializer(typeof(Grade));
                //var stream = new MemoryStream();
                //jsonSerializer.WriteObject(stream, newGrade);
                //var content = new StringContent(new StreamReader(stream).ReadToEnd(), System.Text.Encoding.UTF8, "application/json");
                //var result = await client.PostAsJsonAsync<Grade>("api/Grades", newGrade);

                /*if (result.IsSuccessStatusCode)
                {
                    var resultSTream = await result.Content.ReadAsStreamAsync();
                }
                else
                {
                    result.EnsureSuccessStatusCode(); 
                }*/
                
            }
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var grade = (Grade)e.OriginalSource;
            Handle(sender as CheckBox, grade);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var grade = (Grade)e.OriginalSource;
            Handle(sender as CheckBox, grade);
        }

        void Handle(CheckBox checkBox, Grade grade)
        {
            // Use IsChecked.
            bool flag = checkBox.IsChecked.Value;

            if (flag)
            {
                Debug.WriteLine(grade);
            }
        }
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
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}
