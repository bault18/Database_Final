using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DatabaseFinalProject
{
    public class Student
    {
        #region Student Properties
        private string f_name;
        private string l_name;

        private string username;
        private string password;

        private string major;

        //stores registered classes
        private List<Classes> registered_for;
        #endregion

        public Student(string u_name, string p_word, string first_n, string last_n, string maj, List<Classes> imported)
        {
            username = u_name.Replace('_', ' ');
            password = p_word.Replace('_', ' ');
            f_name = first_n.Replace('_', ' ');
            l_name = last_n.Replace('_', ' ');
            major = maj.Replace('_', ' ');
            registered_for = imported;
        }

        #region Getters/Setters
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public List<Classes> registered_classes()
        {
            return registered_for;
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
            foreach (Classes curr in registered_for)
            {
                if (addit != curr) { /*do nothing*/ }
                else { return; }

            }
            registered_for.Add(addit);
        }
        public void drop_class(Classes drop)
        {
            registered_for.Remove(drop);
        }

        public int update_credits()
        {
            int credits = 0;
            foreach (Classes cla in registered_for)
                credits += Int32.Parse(cla.Credits);
            return credits;
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
            import_class_list();
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

        public bool login(string user, string pass)
        {
            //Import student list from file
            import_student_file();

            //Sets curr_stud
            foreach (var stud in all_students)
            {
                if (user == stud.Username)
                {
                    curr_stud = stud;
                    all_students.Remove(stud);
                    break;
                }
            }

            //Test Password
            if (curr_stud == null || curr_stud.Password != pass) //No student exists
                return false;
            else //Assume pass is true
            {
                import_class_list();
                return true;
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
                        all_students.Add(new Student(curr_stud_info[0], curr_stud_info[1], curr_stud_info[2], curr_stud_info[3], curr_stud_info[4], regsisteredfor));
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
            //Add curent student to the list of students
            all_students.Add(curr_stud);

            //Open file of students
            string filepath = Environment.CurrentDirectory + "\\StudentList.txt";
            using (StreamWriter output = new StreamWriter(filepath))
            {
                //Export each student
                foreach (Student curr in all_students)
                {
                    //Format each student data into one string to be exported line by line
                    string student = curr.Username.Replace(' ', '_') + " " + curr.Password.Replace(' ', '_') + " " + curr.F_name.Replace(' ', '_') + " " + curr.L_name.Replace(' ', '_') + " " + curr.Major.Replace(' ', '_') + " * ";
                    foreach (Classes reg_class in curr.registered_classes())
                    {
                        student += reg_class.Class_type + " " + reg_class.Class_num + " " + reg_class.Class_name.Replace(' ', '_') + " " + reg_class.Credits + " " + reg_class.Professor.Replace(' ', '_') + " ";
                    }

                    output.WriteLine(student);
                }
            }
        }
    }

}
