﻿

#pragma checksum "C:\Users\Kristin\documents\visual studio 2013\Projects\HiofStudentPlannerSolution\Client\WeekOverview.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "164172B6D3081454149B725706CCC752"
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
    partial class WeekOverview : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 51 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.Course_Click;
                 #line default
                 #line hidden
                #line 51 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.Selector)(target)).SelectionChanged += this.itemGridView_SelectionChanged;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 89 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.LogOut_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 75 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Student_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 125 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Courses_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 127 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Grades_Click;
                 #line default
                 #line hidden
                break;
            case 6:
                #line 128 "..\..\WeekOverview.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Profile_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


