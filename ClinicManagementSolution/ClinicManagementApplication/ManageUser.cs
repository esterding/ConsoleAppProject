using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementModelLibrary;
using CllinicManagementApplication;

namespace ClinicManagementApplication
{
    public class ManageUser
    {
        User NewUser = new User();

        public static List<Doctor> doctors = new List<Doctor>()
        {
            new Doctor{ UserId = 101, UserName = "michiko", Name = "Michiko Wong", UserPassword = "abc123", UserType = "Doctor", Experience = 10, 
                Specialty = "Anaesthesiology and Critical Care", UserAge = 45, Status = "Active"},
            new Doctor{ UserId = 102, UserName = "david", Name = "David Lawson", UserPassword = "abc123", UserType = "Doctor", Experience = 20,
                Specialty = "Anatomical Pathology", UserAge = 49, Status = "Active"},
            new Doctor{ UserId = 103, UserName = "mithuya", Name = "Mithuya Dywah", UserPassword = "abc123", UserType = "Doctor", Experience = 18,
                Specialty = "Cardiothoracic Surgery ", UserAge = 52, Status = "Active"}
        };

        public static List<Patient> patients = new List<Patient>()
        {
            new Patient{ UserId = 101, UserName = "limls", Name = "Lim Lay Sia", UserPassword = "abc123", UserType = "Patient"},
            new Patient{ UserId = 102, UserName = "brownson", Name = "Brownson Fillips", UserPassword = "abc123", UserType = "Patient"},
            new Patient{ UserId = 103, UserName = "rahmat", Name = "Ali Rahmat", UserPassword = "abc123", UserType = "Patient"}
        };

        public void UserLogin()
        {
            int choice = 0;
            do
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Hi, Welcome to Clinic Medidion");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1. Login to my account");
                Console.WriteLine("2. Register as new user");
                Console.WriteLine("0. Exit");

                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid entry. Please enter a number : ");
                }

                switch (choice)
                {
                    case 1:
                        LoginToUserAccount(GetUserTypeFromUserInput());
                        break;

                    case 2:
                        GetUserDetailsForRegisteration(GetUserTypeFromUserInput());
                        break;

                    case 0:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid. Please enter a number");
                        break;
                }
            } while (choice != 0);
        }
        public string GetUserTypeFromUserInput()
        {
            int selection = 0;
            string type = "";
            Console.WriteLine("What is the user type ? \n1.Patient \n2.Doctor \n0.Back");
            do
            {
                while (!Int32.TryParse(Console.ReadLine(), out selection))
                {
                    Console.WriteLine("Invalid. Please type 1 or 2");
                }
               switch(selection)
                {
                    case 1:
                        type = "patient"; break;
                    case 2:
                        type = "doctor"; break;
                    case 0:
                        UserLogin();
                        break;
                    default:
                        Console.WriteLine("Invalid");
                        break;
                }
            } while (selection != 1 && selection !=2);

            return type;
        }

        public void GetUserDetailsForRegisteration(string userType)
        {
            switch (userType)
            {
                case "patient" :
                    NewUser = new Patient();
                    ((Patient)NewUser).GetUserInformation();
                    bool isAllowToProceed = IsPatientNotDuplicate((Patient)NewUser);
                    if(isAllowToProceed)
                    {
                        ((Patient)NewUser).UserId = GenerateId(userType);
                        ((Patient)NewUser).GetPatientInformation();
                        patients.Add(((Patient)NewUser));
                        Console.WriteLine("Welcome " + ((Patient)NewUser).Name + "! Thank you for joining us!");
                    }
                    break;

                case "doctor" :
                    NewUser = new Doctor();
                    ((Doctor)NewUser).GetUserInformation();
                    bool isAllowedToProceed = IsDoctorNotDuplicate((Doctor)NewUser);
                    if(isAllowedToProceed)
                    {
                        ((Doctor)NewUser).UserId = GenerateId(userType);
                        ((Doctor)NewUser).GetDoctorInformation();
                        doctors.Add(((Doctor)NewUser));
                        Console.WriteLine("Welcome Doctor " + ((Doctor)NewUser).Name + "! Thank you for joining us!");
                    }
                    break;

                default       : 
                    Console.WriteLine("Invalid entry. There is no type : " + userType);
                    Console.WriteLine("Please try again.");
                    return;
            }
        }

        private bool IsPatientNotDuplicate(Patient newUser)
        {
            var isPatientDuplicate = patients.Where(p => p.UserName == newUser.UserName);
            if(!isPatientDuplicate.Any())
            {
                return true;
            }
            else
            {
                Console.WriteLine("This username not available. Try again...");
                return false;
            }
        }
        private bool IsDoctorNotDuplicate(Doctor newUser)
        {
            var isDoctorDuplicate = doctors.Where(p => p.UserName == newUser.UserName);
            if (!isDoctorDuplicate.Any())
            {
                return true;
            }
            else
            {
                Console.WriteLine("This username not available. Try again...");
                return false;
            }
        }
      
        public void LoginToUserAccount(string userType)
        {
            try
            {
                switch (userType)
                {
                    case "patient":
                        Console.WriteLine("---------------------------------------------------");
                        Console.WriteLine("Login as Patient");
                        Console.WriteLine("---------------------------------------------------");
                        User patient = LoginToUserAccount(GetUserNamePassword(), userType);
                        if(patient != null)
                        {
                            Console.WriteLine("Hi " + patient.Name + "!");
                            ManageAppointment manageApp = new ManageAppointment();
                            manageApp.MakeAppointments((Patient)patient);
                        }
                        else
                            return;
                        break;

                    case "doctor":
                        Console.WriteLine("---------------------------------------------------");
                        Console.WriteLine("Login as Doctor");
                        Console.WriteLine("---------------------------------------------------");
                        User doctor = LoginToUserAccount(GetUserNamePassword(), userType);
                        if (doctor != null)
                        {
                            Console.WriteLine("Hi Doctor " + doctor.Name + "! Good to see you again!");
                            ManageAppointment manageApp = new ManageAppointment();
                            manageApp.ManageAppointments((Doctor)doctor);
                        }
                        else
                            return;
                        break;

                    default:
                        Console.WriteLine("Invalid entry. There is no such type. Please Try Again.");
                        return;
                }
            }
            catch(NullReferenceException ne)
            {
                Console.WriteLine(ne.Message);
                Console.WriteLine("Username or Password are incorrect.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //Console.WriteLine("Username or Password are incorrect.");
            }
        }

        private User LoginToUserAccount(Dictionary<string,string> myDictionary, string userType)
        {
            User user = new User();
            string uName = "", uPassword = "";

            foreach(KeyValuePair<string, string> pair in myDictionary)
            {
                uName = pair.Key;
                uPassword = pair.Value;
            }

            switch (userType)
            {
                case "patient":
                    var patient = patients.SingleOrDefault(e => e.UserName == uName && e.UserPassword == uPassword);
                    if (patient != null)
                        user = patient;
                    else
                    {
                        Console.WriteLine("Fail to login. Username or password is wrong.");
                        return null;
                    }
                    break;

                case "doctor":
                    var doctor = doctors.SingleOrDefault(e => e.UserName == uName && e.UserPassword == uPassword);
                    if (doctor != null)
                        user = doctor;
                    else
                    {
                        Console.WriteLine("Fail to login. Username or password is wrong.");
                        return null;
                    }
                    break;  

                default:
                    Console.WriteLine("Fail to login. Username or password is wrong.");
                    break;
            }

            return user;
        }

        public List<Doctor> GetAllDoctor()
        {
            var allDoctors = doctors.Where(e => e.Status == "Active")
                .OrderBy(e => e.Name).ToList();

            return allDoctors;
        }

        public void PrintAllDoctors(List<Doctor> doctors)
        {
            Console.WriteLine(".........................................................");
            foreach (var d in doctors)
            {
                Console.WriteLine(d);
            }
            Console.WriteLine(".........................................................");

          
        }

        private int GenerateId(string type)
        {
            switch(type)
            {
               
                case "doctor":
                    if (doctors.Count == 0)
                        return 101;
                    else
                        return doctors.Count + 101;

                case "patient":

                default:
                    if (patients.Count == 0)
                        return 101;
                    else
                        return patients.Count + 101;
            }
        }

        public Dictionary<string, string> GetUserNamePassword()
        {
            ConsoleKeyInfo key; string userPassword = "";
            Dictionary<string, string> loginDetails = new Dictionary<string, string>();

            Console.WriteLine("Please enter your Username : ");
            string userName = Console.ReadLine().Trim();
            Console.WriteLine("Please enter your Password : ");

            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace) // Backspace Should Not Work
                {
                    userPassword += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter); // Stops Receving Keys Once Enter is Pressed
            Console.WriteLine();
            loginDetails.Add(userName, userPassword.Trim());

            return loginDetails;
        }


        public void PrintDoctorByUserName(Doctor doctor)
        {
            Console.WriteLine("Welcome Dr. " + doctor.Name);
        }

        public void PrintUserDetails()
        {
            Console.WriteLine(NewUser);
        }
    }
}
