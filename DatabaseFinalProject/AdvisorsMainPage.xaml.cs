using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Collections;
using System.Text.RegularExpressions;

namespace DatabaseFinalProject
{
    /// <summary>
    /// Interaction logic for AdvisorsMainPage.xaml
    /// </summary>
    public partial class AdvisorsMainPage : Window
    {
        public AdvisorsMainPage()
        {
            InitializeComponent();

    }

        private void dropBttn_Click(object sender, RoutedEventArgs e)
        {
            foreach (Student stu in Registrar.get_shared_instance().Selected_students)
            {
                if (stu.IsChecked == true)
                {
                    stu.IsChecked = false;
                    string url = "http://cs1/whitnetacess/runSQLMSSQL.php?switchcontrol=12&sid=" + stu.ID;

                    using (var wc = new WebClient())
                    {
                        var output = wc.DownloadString(url);

                        if (output != "Complete")
                        {
                            MessageBox.Show("*****ERROR***** \nFailed to drop student.");
                        }
                       
                    }
                }
            }

            tabControl.SelectedIndex = 1;
            tabControl.SelectedIndex = 0;
            
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl item = (TabControl)sender;
            TabItem selected = item.SelectedItem as TabItem;
            if(selected.Name == "advisees")
            {
                string url = "http://cs1/whitnetacess/runSQLMSSQL.php?switchcontrol=11&aid=" + Registrar.get_shared_instance().Curr_Prof.ID;
                List<Student> advise = new List<Student>();

                using (var wc = new WebClient())
                {
                    var json = wc.DownloadString(url);

                    dynamic par = JArray.Parse(json);
                    foreach (dynamic tuple in par)
                    {
                        string f_name = tuple.First_Name;
                        string l_name = tuple.Last_Name;
                        int id = tuple.id;
                        advise.Add(new Student(id, f_name, l_name));
                    }


                    Registrar.get_shared_instance().Selected_students = advise;
                }

                advisees_list.ItemsSource = advise;

                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(advisees_list.ItemsSource);
                view.SortDescriptions.Add(new SortDescription("L_name", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("F_name", ListSortDirection.Ascending));
            }
        }
    }
}
