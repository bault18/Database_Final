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

namespace DatabaseFinalProject
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Window
    {
        public CreateAccount()
        {
            InitializeComponent();
        }

        private void create_acc_btn_press(object sender, RoutedEventArgs e)
        {
            //if blank field
            if (set_username.Text == "" || set_password.Text == "" || set_firstname.Text == "" || set_lastname.Text == "" || set_major.Text == "")
                MessageBox.Show("*****ERROR***** \nPlease fill out all fields");
            //if underscore used
            else if (set_username.Text.Contains("_") || set_password.Text.Contains("_") || set_firstname.Text.Contains("_") || set_lastname.Text.Contains("_"))
                MessageBox.Show("*****ERROR***** \nDo not use underscores");
            //assume filled out correctly
            else
            {
                Registrar.get_shared_instance().Curr_Stud = new Student(set_username.Text, set_password.Text, set_firstname.Text, set_lastname.Text, set_major.Text, new List<Classes>());
                new MainWindow().Show();
                this.Close();
            }
        }

        //if cancel btn clicked
        private void cancel_acc_creation(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

    }
}
