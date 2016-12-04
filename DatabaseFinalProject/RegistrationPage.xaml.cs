﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DatabaseFinalProject
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Window
    {
        public RegistrationPage()
        {
            InitializeComponent();
        }

        private void class_search(object sender, RoutedEventArgs e) //TODO~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        {
            //create query link and get results
            string url = "http://cs1/whitnetacess/runSQLMSSQL.php?switchcontrol=3&dept=" + dept.Text + "&num=" + class_num.Text + "&name=" + class_title.Text + "&prof=" + prof.Text;

            //parse results into Classes object
            List<Classes> search = new List<Classes>();
            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~FIX ME: get the actual professor for the class
                string prof = string.Empty;
                dynamic par = JArray.Parse(json);
                foreach(dynamic tuple in par)
                {
                    string dep = tuple.Department; //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~Issue with dep having whitespaces, not sorting properly
                    int cnum = tuple.Course_Number;
                    int sec = tuple.Section_Number;
                    string name = tuple.Course_Name;
                    int cred = tuple.Credits;
                    int size = tuple.Size;
                    search.Add(new Classes(dep, cnum, sec, name, cred, prof, size));
                }

            }

            //Displays list
            class_list_view.ItemsSource = search;

            //Sorts list that is dislayed
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(class_list_view.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("department", ListSortDirection.Ascending)); //~~~~~~~~~~~~~~~~~~~~~~FIX ME: Not working right now
            view.SortDescriptions.Add(new SortDescription("class_number", ListSortDirection.Ascending));

            //if no results to display
            if (search.Count == 0)
                MessageBox.Show("No classes meet your search criteria");
        }

        //Detects when tabs change and updates items in selected tab
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl item = sender as TabControl;
            TabItem selected = item.SelectedItem as TabItem;

            //updates 'registered classes' tab
            if ("update_reg_class" == selected.Name)
            {
                //Query # of credits registered for and push to 'credits_reg_for.Text'
                
                //Generate query link to get courses we are in
                
                //create object for each course
                
                //push courses into list and make that list get shown (see below) 
               
                /*
                //Updates list of classes registered for
                //registered_class_list_view.ItemsSource = Registrar.get_shared_instance().Curr_Stud.registered_classes();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(registered_class_list_view.ItemsSource);

                //Sort first by 'Class_type' then by 'Class_num'
                view.SortDescriptions.Add(new SortDescription("Class_type", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("Class_num", ListSortDirection.Ascending));*/
            }
            else if ("edit_acc_info_tab" == selected.Name)
            {
                //query in student account info and populate the various text boxes
                

                /*
                //Updates display of account information
                change_f_name.Text = Registrar.get_shared_instance().Curr_Stud.F_name;
                change_l_name.Text = Registrar.get_shared_instance().Curr_Stud.L_name;
                change_username.Text = Registrar.get_shared_instance().Curr_Stud.Username;
                
                change_major.Text = Registrar.get_shared_instance().Curr_Stud.Major;*/
            }
        }

        //Updates 'Curr_Stud' to changed account info
        private void update_acc_btn(object sender, RoutedEventArgs e)
        {
            //push changes to database

            /*
            Registrar.get_shared_instance().Curr_Stud.F_name = change_f_name.Text;
            Registrar.get_shared_instance().Curr_Stud.L_name = change_l_name.Text;
            Registrar.get_shared_instance().Curr_Stud.Username = change_username.Text;
            
            Registrar.get_shared_instance().Curr_Stud.Major = change_major.Text;*/
        }


        //Adds and drops classes from 'Curr_Stud'
        private void add_drop_btn(object sender, RoutedEventArgs e)
        {/*
            Button pressed = (Button)sender;
            List<Classes> drop_list = Registrar.get_shared_instance().Curr_Stud.registered_classes();

            //if button is for dropping classes
            if (pressed.Name == "drop_btn")
            {
                //Had to use for-loop otherwise crash
                for (int i = drop_list.Count - 1; i >= 0; i--)
                {
                    if (drop_list.ElementAt(i).IsChecked == true)
                        Registrar.get_shared_instance().Curr_Stud.drop_class(drop_list.ElementAt(i));
                }

                //Updates page info
                tabControl.SelectedIndex = 1;
                tabControl.SelectedIndex = 0;
                
            }
            //if button is for adding classes
            else if (pressed.Name == "add_btn")
            {   //Add classes to student's registered classes
                foreach (Classes curr in Registrar.get_shared_instance().ALL_classes)
                {
                    if (curr.IsChecked == true && (Registrar.get_shared_instance().Curr_Stud.update_credits() + Int32.Parse(curr.Credits)) < 18)
                    {
                        curr.IsChecked = false;
                        Registrar.get_shared_instance().Curr_Stud.add_class(curr);
                    }
                    else if (curr.IsChecked == true)
                    {
                        MessageBox.Show("\t\t*****NOTE*****\nYou have reached the maximum amount of \ncredits a fulltime student can register for");
                        break;
                    }
                }

                //Resets checkboxes in search tab
                Button update = seach_classes_btn;
                update.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

                tabControl.SelectedIndex = 0;
            }
        */}

    }
}
