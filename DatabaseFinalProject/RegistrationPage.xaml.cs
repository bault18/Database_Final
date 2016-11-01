using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        //When user closes window
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Save all data changed in current student
            Registrar.get_shared_instance().export_student_file();
        }

        private void class_search(object sender, RoutedEventArgs e)
        {
            //Resets checkboxes in search tab
            foreach (Classes curr in Registrar.get_shared_instance().ALL_classes)
                curr.IsChecked = false;

            //Search stores all classes that meet search criteria
            List<Classes> search = new List<Classes>();
            //all_class stores all the classes
            List<Classes> all_class = Registrar.get_shared_instance().ALL_classes;



            //Searches all classes for criteria
            for (int i = 0; i < all_class.Count; i++)
            {
                Classes current = all_class.ElementAt(i);
                bool match_filter = true;

                if (classType.Text != "")
                    match_filter = current.Class_type == classType.Text;

                if (match_filter == true && class_num_box.Text != "")
                    match_filter = current.Class_num.Contains(class_num_box.Text);

                if (match_filter == true && class_title_box.Text != "")
                    match_filter = current.Class_name.Contains(class_title_box.Text);

                //If the class fulfills all criteria add it to displaylist
                if (match_filter)
                    search.Add(current);
            }

            //Displays list
            class_list_view.ItemsSource = search;

            //Sorts list that is dislayed
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(class_list_view.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Class_type", ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription("Class_num", ListSortDirection.Ascending));

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
                //Updates amount of credits registered for
                credits_reg_for.Text = string.Format("{0}", Registrar.get_shared_instance().Curr_Stud.update_credits());
                //Updates amount of classes registered for
                num_class_reg_for.Text = string.Format("{0}", Registrar.get_shared_instance().Curr_Stud.registered_classes().Count);

                //Updates list of classes registered for
                registered_class_list_view.ItemsSource = Registrar.get_shared_instance().Curr_Stud.registered_classes();
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(registered_class_list_view.ItemsSource);

                //Sort first by 'Class_type' then by 'Class_num'
                view.SortDescriptions.Add(new SortDescription("Class_type", ListSortDirection.Ascending));
                view.SortDescriptions.Add(new SortDescription("Class_num", ListSortDirection.Ascending));
            }
            else if ("edit_acc_info_tab" == selected.Name)
            {
                //Updates display of account information
                change_f_name.Text = Registrar.get_shared_instance().Curr_Stud.F_name;
                change_l_name.Text = Registrar.get_shared_instance().Curr_Stud.L_name;
                change_username.Text = Registrar.get_shared_instance().Curr_Stud.Username;
                change_password.Text = Registrar.get_shared_instance().Curr_Stud.Password;
                change_major.Text = Registrar.get_shared_instance().Curr_Stud.Major;
            }
        }

        //Updates 'Curr_Stud' to changed account info
        private void update_acc_btn(object sender, RoutedEventArgs e)
        {
            Registrar.get_shared_instance().Curr_Stud.F_name = change_f_name.Text;
            Registrar.get_shared_instance().Curr_Stud.L_name = change_l_name.Text;
            Registrar.get_shared_instance().Curr_Stud.Username = change_username.Text;
            Registrar.get_shared_instance().Curr_Stud.Password = change_password.Text;
            Registrar.get_shared_instance().Curr_Stud.Major = change_major.Text;
        }


        //Adds and drops classes from 'Curr_Stud'
        private void add_drop_btn(object sender, RoutedEventArgs e)
        {
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
        }

    }
}
