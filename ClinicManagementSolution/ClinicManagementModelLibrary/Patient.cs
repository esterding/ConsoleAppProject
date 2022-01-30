using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementModelLibrary
{
    public class Patient : User
    {
        public string Remarks { get; set; }
        public string Status { get; set; }

        public Patient()
        {
            UserType = "Patient";
        }

        public void GetPatientInformation()
        {
            //Console.WriteLine("Please enter Patient Status : ");
            //Status = Console.ReadLine();
            //Console.WriteLine("Please enter remarks : ");
            //Remarks = Console.ReadLine();
        }

        public override string ToString()
        {
            return base.ToString() + "\n User Type is : " +  UserType;
        }
    }
}
