using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace DatabaseFinalProject
{
    public class Student
    {
        #region Student Properties
        private int id;
        private string f_name;
        private string l_name;

        private string username;

        private string major;
        private string address;
        private int phone_number;
        private string email;

        
        #endregion

        public Student(int iden, string u_name, string first_n, string last_n, string maj, int phone, string em, string addres)
        {
            id = iden;
            username = u_name;
            f_name = first_n;
            l_name = last_n;
            major = maj;
            phone_number = phone;
            email = em;
            address = addres;
        }

        #region Getters/Setters
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string F_name
        {
            get { return f_name; }
            set { f_name = value; }
        }

        public string L_name
        {
            get { return l_name; }
            set { l_name = value; }
        }

        public string Major
        {
            get { return major; }
            set { major = value; }
        }
        #endregion

        public void add_class(Classes addit)
        {
            //check if already registered for class
            
        }
        public void drop_class(Classes drop)
        {
            
        }

        public int update_credits()
        {
            return 0;
        }

    }

    public class Classes
    {
        #region Class Properties
        private string class_type;
        private string class_num;
        private string class_name;
        private string credits;
        private string professor;

        private bool isChecked;
        #endregion

        public Classes(string type, string num, string name, string cred, string proff)
        {
            class_type = type;
            class_num = num;
            class_name = name;
            credits = cred;
            professor = proff;
            isChecked = false;
        }

        #region Getters/Setters
        public string Class_type
        { get { return class_type; } }

        public string Class_num
        { get { return class_num; } }

        public string Class_name
        { get { return class_name; } }

        public string Credits
        { get { return credits; } }

        public string Professor
        { get { return professor; } }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
        #endregion
    }


    public class Registrar
    {
        private static Student curr_stud = null;
        private static Registrar instance = null;

        private List<Student> all_students = new List<Student>();
        private List<Classes> all_classes = null;
        public static Registrar get_shared_instance()
        {
            if (instance == null) //if no registrar has been created, make one
                instance = new Registrar();

            return instance; //always return created registrar
        }

        private Registrar()
        {
            //import_class_list();
        }

        private void import_class_list()
        {
            //Import class list here
            all_classes = new List<Classes>();

            //FILE IO
            string filepath = Environment.CurrentDirectory + "\\Avalibleclasses.txt";

            using (StreamReader classfile = File.OpenText(filepath))
            {

                for (; !classfile.EndOfStream;)
                {
                    string[] words = classfile.ReadLine().Split(' ');
                    words[2] = words[2].Replace('_', ' ');
                    all_classes.Add(new Classes(words[0], words[1], words[2], words[3], words[4]));
                }
            }
        }

        public Student Curr_Stud
        {
            get { return curr_stud; }
            set { curr_stud = value; }
        }

        public bool login(string user, string pass, string type)
        {

            using (var wc = new WebClient())
            {
                //Run login query
                var json = wc.DownloadString("http://cs1/whitnetacess/runSQLMSSQL.php?switchcontrol=1&uname=" + user + "&pass=" + pass + "&utype=" + type);
                
                //Test results if account exists
                if (json == "[]")
                    return false;
                else
                {
                    //Decode the JSON to create the student object
                    dynamic result = JArray.Parse(json);

                    curr_stud = new Student(result[0].id, user, result[0].First_Name, result[0].Last_Name, result[0].Major, result[0].Phone_Number, result[0].Email, result[0].Address);
                    return true;
                }

            }
        }

        public List<Classes> ALL_classes
        {
            get { return all_classes; }
        }

        public void import_student_file()
        {
            if (all_students.Count == 0)
            {
                string filepath = Environment.CurrentDirectory + "\\StudentList.txt";
                using (StreamReader studaccs = File.OpenText(filepath))
                {
                    do
                    {
                        //Stores: username, password, f_name, l_name
                        string[] curr_stud_info = new string[5];
                        //Stores: up to 10 classes worth of properties
                        string[] curr_stud_classes = new string[100];
                        //Temporary saveplace for student info
                        string stud_info = "";
                        List<Classes> regsisteredfor = new List<Classes>();

                        //Retrieves student account info
                        string currline = studaccs.ReadLine();
                        if (currline == null)
                            break;

                        for (int i = 0; currline[i] != '*'; i++)
                        {
                            if (currline[i] != '*')
                                stud_info += currline[i];
                        }

                        //Retrieves current student class info
                        //Statements go through each character the imported information
                        bool classtime = false;
                        for (int i = 0, word = -1; i < currline.Length; i++)
                        {
                            if (currline[i] == '*')
                                classtime = true;
                            else if (classtime == true && currline[i] != ' ')
                                curr_stud_classes[word] += currline[i];
                            else if (classtime == true && currline[i] == ' ')
                                word++;


                        }
                        //Updates current student class list
                        for (int i = 0; curr_stud_classes[i] != null; i += 5)
                        {
                            curr_stud_classes[i + 2] = curr_stud_classes[i + 2].Replace('_', ' '); //removes underscore from class title
                            regsisteredfor.Add(new Classes(curr_stud_classes[i], curr_stud_classes[i + 1], curr_stud_classes[i + 2], curr_stud_classes[i + 3], curr_stud_classes[i + 4]));
                        }

                        //Creates student
                        curr_stud_info = stud_info.Split(' ');
                        //all_students.Add(new Student(curr_stud_info[0], curr_stud_info[1], curr_stud_info[2], curr_stud_info[3], curr_stud_info[4], regsisteredfor));
                    } while (true);
                }
            }

            //if a new account was created
            if (all_students.Contains(curr_stud))
            {
                all_students.Add(curr_stud);
                curr_stud = null;
            }

        }


        public void export_student_file()
        {

        }
    }
}
