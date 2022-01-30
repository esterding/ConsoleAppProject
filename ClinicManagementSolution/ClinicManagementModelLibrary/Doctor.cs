using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ClinicManagementModelLibrary
{
    public class Doctor : User
    {     
        public int Experience { get; set; }
        public string Specialty { get; set; }

        public string Status { get; set; }

        public Doctor()
        {
            UserType = "Doctor";
            Status = "Active";
        }

        public void GetDoctorInformation()
        {
            int exp = 0;
            Console.WriteLine("Please enter the Doctor Experience (year) : ");
            while(!Int32.TryParse(Console.ReadLine(), out exp))
            {
                Console.WriteLine("Plse enter your exprience in number");
            }

            Experience = exp;
            Console.WriteLine("Please enter the Doctor Speciality : ");
            Specialty = Console.ReadLine();
        }

        public override string ToString()
        {
            return "ID :" + UserId + "\t Doctor's Name : " + Name + "\t Experince (year) : " + Experience + " \t Speciality in " + Specialty;
        }
    }
}
