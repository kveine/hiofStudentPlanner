﻿

#pragma checksum "C:\Users\Kristin\documents\visual studio 2013\Projects\HiofStudentPlannerSolution\Client\Courses.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A4FADBDAAD695D4B5DAB750B667ABF3E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client
{
    partial class Courses : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 53 "..\..\Courses.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.CourseView_CourseClick;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 102 "..\..\Courses.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.ComboBox_Loaded;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 104 "..\..\Courses.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AddCourse_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 66 "..\..\Courses.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Checked += this.CheckBox_Checked;
                 #line default
                 #line hidden
                #line 67 "..\..\Courses.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ToggleButton)(target)).Unchecked += this.CheckBox_Unchecked;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


