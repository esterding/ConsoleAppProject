using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementModelLibrary
{
    public class Appointment_PrintDetail : Appointment
    {
        public string DoctorName { get; set; }
        public string DoctorSpeciality { get; set; }
        public string PatientName { get; set; }

        public override string ToString()
        {
            return "\nAppointment Date \t: " + Start_DateTime
                + "\nPatient Name \t\t: " + PatientName
                + "\nDoctor Name \t\t: " + DoctorName
                + "\nDoctor Speciality \t: " + DoctorSpeciality
                + "\nDoctor Remarks \t\t: " + DoctorRemarks
                + "\nFees \t\t\t: " + Fees
                 + "\nPayment Status \t\t: " + PaymentStatus;
        }

    }
}
