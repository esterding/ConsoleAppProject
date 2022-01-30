using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementModelLibrary
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Start_DateTime { get; set; }
        public DateTime End_DateTime { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public double? Fees { get; set; }
        public string DoctorRemarks { get; set; }
        public string PaymentStatus { get; set; }
        public byte isDeleted { get; set; }

       
        public Appointment()
        {
            PaymentStatus = "";
            isDeleted = 0;
        }

        public override string ToString()
        {
            return "Appointment Id \t\t: " + AppointmentId
                + "\nAppointment Date \t: " + Start_DateTime
                + "\nDoctor Id \t\t: " + DoctorId
                + "\nDoctor Remarks \t\t: " + DoctorRemarks
                + "\nFees \t\t\t: " + Fees
                 + "\nPayment Status \t\t: " + PaymentStatus;
        }

        


    }
}
