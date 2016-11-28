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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void login_btn_press(object sender, RoutedEventArgs e)
        {
            //If login passes
            if (Registrar.get_shared_instance().login(enter_user.Text, enter_pass.Password) == true)
            {
                //open next page and close login page
                new RegistrationPage().Show();
                this.Close();
            }
            else
                MessageBox.Show("*******ERROR****** \nUsername or password is invalid.");
        }

        private void create_acc_press(object sender, RoutedEventArgs e)
        {
            new CreateAccount().Show();
            this.Close();
        }
    }
}
