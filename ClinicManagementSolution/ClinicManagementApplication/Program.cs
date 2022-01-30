using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementModelLibrary;
using CllinicManagementApplication;
namespace ClinicManagementApplication
{

    class Program
    {
      

        static void Main(string[] args)
        {
 
            ManageUser user = new ManageUser();
            ManageAppointment app = new ManageAppointment();
            user.UserLogin();


            Console.ReadKey();
        }
    }
}
