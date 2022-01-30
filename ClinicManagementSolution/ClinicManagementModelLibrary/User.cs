using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementModelLibrary
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string UserPassword { get; set; }
        public int UserAge { get; set; }
        public string UserType { get; set; }

        public void GetUserInformation()
        {
            int age = 0;

            while(true)
            {
                Console.WriteLine("Please enter your UserName : ");
                UserName = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(UserName))
                    Console.WriteLine("Username cannot be empty.");
                else
                    break;
            }
            
            while(true)
            {
                Console.WriteLine("Please enter the user Password : ");
                UserPassword = Console.ReadLine().Trim();

                if (string.IsNullOrEmpty(UserPassword))
                    Console.WriteLine("Password cannot be empty.");
                else
                    break;
            }
            
            while(true)
            {
                Console.WriteLine("Please enter your Name : ");
                Name = Console.ReadLine();

                if (string.IsNullOrEmpty(Name))
                    Console.WriteLine("Name cannot be empty.");
                else
                    break;
            }

            Console.WriteLine("Please enter your Age : ");
            try
            {
                while (!Int32.TryParse(Console.ReadLine(), out age))
                {
                    Console.WriteLine("Plse enter your age in number");
                }

                UserAge = age;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid age");
                Console.WriteLine(ex.Message);
            }
        }

        public override string ToString()
        {
            return " ID :" + UserId + "\n User Name :" + Name;
        }
    }

}
