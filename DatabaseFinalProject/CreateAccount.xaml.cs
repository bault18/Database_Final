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
            if (set_username.Text == "" || set_password.Password == "" || set_firstname.Text == "" || set_lastname.Text == "" || set_major.Text == "")
                MessageBox.Show("*****ERROR***** \nPlease fill out all fields");
            else
            {
                string phone;
                string address;
                string email = null;
                if (set_phone.Text == "")
                    phone = null;
                else
                    phone = set_phone.Text;
                if (set_address.Text == "")
                    address = null;
                else
                    address = set_address.Text;

                string url = "http://cs1/whitnetacess/runSQLMSSQL.php?switchcontrol=2&fname=" + set_firstname.Text + "&lname=" + set_lastname.Text + "&uname=" + set_username.Text;
                url += "&pass=" + set_password.Password + "&major=" + set_major.Text + "&phone_number=" + phone + "&email=" + email + "&address=" + address;

                using (var wc = new WebClient())
                {
                    var output= wc.DownloadString(url); //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~FIX ME~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

                    if(output != "Complete")
                    {
                        MessageBox.Show("*****ERROR*****\nFailed to create account");
                    }
                    else
                    {
                        new MainWindow().Show();
                        this.Close();
                    }
                }                
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
