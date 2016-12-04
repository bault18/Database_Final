﻿using System;
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
        private string department;
        private int class_number;
        private int section;
        private string name;
        private int credits;
        private string professor;
        private int size;

        private bool isChecked;
        #endregion

        public Classes(string dept, int num, int sect, string nam, int cred, string prof, int sze)
        {
            department = dept;
            class_number = num;
            section = sect;
            name = nam;
            credits = cred;
            professor = prof;
            size = sze;
            isChecked = false;
        }

        #region Getters/Setters
        public string Department
        { get { return department; } }

        public int Class_number
        { get { return class_number; } }

        public int Section
        { get { return section; } }

        public string Name
        { get { return name; } }

        public int Credits
        { get { return credits; } }

        public string Professor
        { get { return professor; } }

        public int Size
        { get { return size; } }

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

        public static Registrar get_shared_instance()
        {
            if (instance == null) //if no registrar has been created, make one
                instance = new Registrar();

            return instance; //always return created registrar
        }

        //Constructor. Nothing needs to be done at the creation
        private Registrar(){}

        private void import_class_list()
        {

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
                    dynamic cur = result[0];

                    //If we don't do this then for some reason it breaks when we try to create a new student
                    int id = result[0].id;
                    string fname = result[0].First_Name;
                    string lname = result[0].Last_Name;
                    string major = result[0].Major;

                    //Deal with possible null values
                    int phone = (result[0].Phone_Number == null) ? 0 : result[0].Phone_Number;
                    String email = (result[0].Email == null) ? "teqwe" : result[0].Email;
                    String address = (result[0].Address == null) ? "asdfa" : result[0].Address;
                    

                    curr_stud = new Student(id, user, fname, lname, major, phone, email, address);
                    return true;
                }

            }
        }


        public void import_student_file()
        {

        }


        public void export_student_file()
        {

        }
    }
}
