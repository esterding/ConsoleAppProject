using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ClinicManagementModelLibrary;
using ClinicManagementApplication;

namespace CllinicManagementApplication
{
    class ManageAppointment
    {
        public static List<Appointment> appointments = new List<Appointment>()
        {
            new Appointment{AppointmentId = 101,  Start_DateTime = new DateTime(2022, 5, 26, 15, 30,0), End_DateTime = new DateTime(2022, 5, 26, 16, 00,0), PatientId = 102,
                DoctorId = 103, Fees = null, DoctorRemarks = "", PaymentStatus = "", isDeleted = 0 },
            new Appointment{AppointmentId = 102,  Start_DateTime = new DateTime(2022, 1, 18, 08, 30,0), End_DateTime =  new DateTime(2022, 1, 18, 09, 00,0), PatientId = 101,
                DoctorId = 101, Fees = 200, DoctorRemarks = "", PaymentStatus = "Paid", isDeleted = 0},
            new Appointment{AppointmentId = 103,  Start_DateTime = new DateTime(2022, 2, 25, 09, 30,0), End_DateTime = new DateTime(2022, 2, 25, 10, 00,0), PatientId = 102,
                DoctorId = 101, Fees = null, DoctorRemarks = "", PaymentStatus = "", isDeleted = 1},
            new Appointment{AppointmentId = 104,  Start_DateTime = new DateTime(2022, 1, 26, 16, 30,0), End_DateTime = new DateTime(2022, 1,26, 17, 00,0), PatientId = 102,
                DoctorId = 101, Fees = 150, DoctorRemarks = "", PaymentStatus = "Pending", isDeleted = 0},
            new Appointment{AppointmentId = 105,  Start_DateTime = new DateTime(2022, 2, 07, 10, 00,0), End_DateTime = new DateTime(2022, 2, 07, 10, 30,0), PatientId = 103,
                DoctorId = 102, Fees = null, DoctorRemarks = "", PaymentStatus = "", isDeleted = 0,},
             new Appointment{AppointmentId = 106,  Start_DateTime = new DateTime(2022, 1, 27, 16, 30,0), End_DateTime = new DateTime(2022, 1, 27, 17, 00,0), PatientId = 102,
                DoctorId = 101, Fees = 150, DoctorRemarks = "", PaymentStatus = "Pending", isDeleted = 0},
            new Appointment{AppointmentId = 107,  Start_DateTime = new DateTime(2022, 2, 07, 11, 00,0), End_DateTime = new DateTime(2022, 2, 07, 11, 30,0), PatientId = 103,
                DoctorId = 101, Fees = null, DoctorRemarks = "", PaymentStatus = "", isDeleted = 0},
            new Appointment{AppointmentId = 108,  Start_DateTime = new DateTime(2022, 1, 29, 11, 00,0), End_DateTime = new DateTime(2022, 1, 29, 11, 30,0), PatientId = 103,
                DoctorId = 101, Fees = null, DoctorRemarks = "", PaymentStatus = "", isDeleted = 0}

        };

        public void MakeAppointments(Patient patient)
        {
            int choice = 0;
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Welcome to Clinic Medidion");
            Console.WriteLine("Name : " + patient.Name + " User Id : " + patient.UserId);

            do
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("1: Make an appointment");
                Console.WriteLine("2. Edit an appointment");
                Console.WriteLine("3. Cancel an appointment");
                Console.WriteLine("4. Print an appointment");
                Console.WriteLine("5. Print all my appointment");
                Console.WriteLine("6. Make Payment");
                Console.WriteLine("0. Exit");
                Console.WriteLine("---------------------------------------------------");
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid. Please enter a number");
                }

                switch (choice)
                {
                    case 1:
                        AddAppointment(patient.UserId);
                        break;
                    case 2:
                        EditAppointment(patient.UserId);
                        break;
                    case 3:
                        CancelAppointment(patient.UserId);
                        break;
                    case 4:
                        PrintAnAppointment(patient);
                        break;
                    case 5:
                        PrintUpcomingAppointments(patient.UserId);
                        break;
                    case 6:
                        MakePayment(patient.UserId);
                        break;
                    case 0:
                        Console.WriteLine("Bye Bye");
                        break;
                    default:
                        Console.WriteLine("Invalid. Please enter a number");
                        break;

                }

            } while (choice != 0);
        }

        private void MakePayment(int userId)
        {
            var needPayment = appointments.Where(e => e.PatientId == userId && e.Fees != null && (e.PaymentStatus != "" && e.PaymentStatus != "Paid")).OrderBy(a => a.Start_DateTime);
            if (!needPayment.Any())
            {
                Console.WriteLine("No payment required at the moment");
                return;
            }
            else
            {
                int appId = 0;
                PrintAppointments(needPayment);
                Console.WriteLine("Please key in the Appointment Id that you wish to pay.");
                while (!Int32.TryParse(Console.ReadLine(), out appId))
                {
                    Console.WriteLine("Invalid. Please try again...");
                }

                bool validAppId = needPayment.Select(e => e.AppointmentId).Contains(appId);
                if (validAppId)
                {
                    int decision = 0;
                    var paymentAppId = needPayment.SingleOrDefault(e => e.AppointmentId == appId);
                    PrintAppointment((Appointment)paymentAppId);
                    Console.WriteLine("Do you want to make payment? \n1.Yes \n2.No");
                    do
                    {
                        while (!Int32.TryParse(Console.ReadLine(), out decision))
                        {
                            Console.WriteLine("Invalid, please insert 1 or 2.");
                        }

                        switch (decision)
                        {
                            case 1:
                                paymentAppId.PaymentStatus = "Paid";
                                Console.WriteLine("Payment made. Thank you.");
                                break;
                            case 2:
                                Console.WriteLine("Your appointment fees is still unpaid.");
                                break;
                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }
                    } while (decision != 1 && decision != 2);
                }
                return;
            }
        }

        public void AddAppointment(int patientId)
        {
            Appointment appointment = new Appointment();
            ManageUser mu = new ManageUser();

            appointment.AppointmentId = GenerateId();
            appointment.PatientId = patientId;
            try
            {
                appointment.DoctorId = GetAppointmentDoctor();
                appointment.Start_DateTime = (DateTime)GetAppointmentDateTime();
                appointment.End_DateTime = appointment.Start_DateTime.AddMinutes(30);

                bool isAvailable = CheckAppointmentAvalaibility(appointment.DoctorId, appointment.Start_DateTime, appointment.End_DateTime);
                if (isAvailable)
                {
                    appointments.Add(appointment);
                    Console.WriteLine("\nAppointment is made. \nHere's the informataion of your new appointment : ");
                    PrintAppointment(appointment);
                }
                else
                {
                    Console.WriteLine("This date and time is not available.");
                }

            }
            catch (ArgumentOutOfRangeException aor)
            {
                Console.WriteLine(aor.Message);
                Console.WriteLine("Date and Time is not in right format.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool CheckAppointmentAvalaibility(int docId, DateTime start, DateTime end)
        {
            var appointmentDate = start.Date;
            var slotAvailability = appointments.Where(e => e.DoctorId == docId && e.isDeleted == 0 && (e.Start_DateTime > DateTime.Now && e.Start_DateTime.Date == appointmentDate));
            var allDoctorSlotOnThatDay = slotAvailability.Where(e => (e.Start_DateTime < end && e.End_DateTime > start));

            if (!allDoctorSlotOnThatDay.Any())
            {
                return true;
            }
            else
            {
                Console.WriteLine("Sorry, doctor is unavailable. Please change the date or time.");
                return false;
            }
        }

        private int GenerateId()
        {
            if (appointments.Count == 0)
                return 101;

            else
                return appointments.Count + 101;
        }

        public void EditAppointment(int patientId)
        {
            var now = DateTime.Now;
            var comingAppointment = appointments.Where(a => a.PatientId == patientId && a.Start_DateTime > now & a.isDeleted == 0).OrderBy(a => a.Start_DateTime);

            if (!comingAppointment.Any())
            {
                Console.WriteLine("There is no upcoming appointment.");
                return;
            }
            else
            {
                PrintAppointments(comingAppointment);
                int id = GetIdFromUser();
                int selection = 0;
                var selectedAppointment = comingAppointment.Where(a => a.AppointmentId == id).SingleOrDefault();

                if (selectedAppointment == null)
                {
                    Console.WriteLine("Invalid Appointment Id");
                }
                else
                {

                    Console.WriteLine("The appointment you had selected to edit is : ");
                    PrintAppointment(selectedAppointment);
                    do
                    {
                        Console.WriteLine("1: Edit Date and Time");
                        //Console.WriteLine("2. Edit Doctor");
                        Console.WriteLine("0. Back to menu");

                        while (!int.TryParse(Console.ReadLine(), out selection))
                        {
                            Console.WriteLine("Invalid. Please try again...");
                        }

                        switch (selection)
                        {
                            case 1:
                                EditAppointmentDateTime(selectedAppointment);
                                break;
                            case 0:
                                Console.WriteLine("Bye Bye");
                                return;
                            default:
                                Console.WriteLine("Invalid, please try again...");
                                break;
                        }
                    } while (selection != 0);
                }
            }
            return;
        }

        public void EditAppointmentDateTime(Appointment app)
        {
            DateTime start_DateTime = (DateTime)GetAppointmentDateTime();
            DateTime end_DateTime = start_DateTime.AddMinutes(30);

            if (app.Start_DateTime == start_DateTime)
            {
                Console.WriteLine("No changes made. Date and time remain the same");
                return;
            }

            bool isAvailable = CheckAppointmentAvalaibility(app.DoctorId, start_DateTime, end_DateTime);

            if (isAvailable)
            {
                bool isAssured = AssureChangesMade();
                if (isAssured)
                {
                    app.Start_DateTime = start_DateTime;
                    app.End_DateTime = end_DateTime;
                    Console.WriteLine("\nChanges made. \nHere's the informataion of the appointment : ");
                    PrintAppointment(app);
                }
            }
            return;
        }

        public int GetIdFromUser()
        {
            int id = 0;

            Console.WriteLine("Please enter the Appointment Id : ");

            while (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid. Please enter the Appointment Id : ");
            }

            return id;
        }

        private void PrintAnAppointment(Patient patient)
        {
            ManageUser mu = new ManageUser();
            int appId = 0;
            //bool isValid = true;
            var doc = mu.GetAllDoctor();

            var comingAppointment = appointments.Where(a => a.PatientId == patient.UserId && a.isDeleted == 0).OrderBy(a => a.Start_DateTime);

            if (!comingAppointment.Any())
            {
                Console.WriteLine("There is no appointment.");
                return;
            }
            else
            {
                try
                {
                    Console.WriteLine("Please enter the appointment Id : ");
                    while(!Int32.TryParse(Console.ReadLine(), out appId))
                    {
                        Console.WriteLine("Key in the appointment Id in number. Try again.");
                    }

                    var deletedApp = appointments.Where(a => a.isDeleted == 1 && a.AppointmentId == appId);
                    if(deletedApp.Any())
                    {
                        Console.WriteLine("This appointment is invalid.");
                    }
                    else
                    {
                        var appDetail = appointments.Join(doc, a => a.DoctorId, d => d.UserId, (a, d) => new Appointment_PrintDetail
                        {
                            DoctorName = d.Name,
                            DoctorSpeciality = d.Specialty,
                            DoctorId = d.UserId,
                            AppointmentId = a.AppointmentId,
                            Start_DateTime = a.Start_DateTime,
                            PatientId = a.PatientId,
                            PatientName = patient.Name,
                            Fees = a.Fees,
                            DoctorRemarks = a.DoctorRemarks,
                            PaymentStatus = a.PaymentStatus
                        }).SingleOrDefault(e => e.PatientId == patient.UserId && e.AppointmentId == appId);                    

                        if (appDetail != null)
                        {
                            PrintAppointment(appDetail);
                        }
                        else
                            Console.WriteLine("Invalid. Please try again.");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Please enter the Appointment Id without alphebet letters.");
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void PrintAppointments(IOrderedEnumerable<Appointment> comingAppointment)
        {
            foreach (var app in comingAppointment)
            {
                PrintAppointment(app);
            }
        }

        private void PrintUpcomingAppointments(int patientId)
        {
            var now = DateTime.Now;
            var comingAppointment = appointments.Where(a => a.PatientId == patientId && a.Start_DateTime > now && a.isDeleted == 0).OrderBy(a => a.Start_DateTime);

            if (!comingAppointment.Any())
            {
                Console.WriteLine("There is no upcoming appointment.");
                return;
            }
            else
            {
                PrintAppointments(comingAppointment);
            }
        }

        private void PrintAppointment(Appointment app)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine(app);
            Console.WriteLine("----------------------------------------------------------");
        }

        private void PrintAppointment(Appointment_PrintDetail app)
        {
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine(app);
            Console.WriteLine("----------------------------------------------------------");
        }

        public void ManageAppointments(Doctor doctor)
        {
            int choice = 0;
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Welcome to Clinic Medidion");
            do
            {
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("1: View all your appointments");
                Console.WriteLine("2. View your past appointments");
                Console.WriteLine("3. View your today's appointments");
                Console.WriteLine("4. Raise payment request for an appointment");
                Console.WriteLine("0. Exit");
                Console.WriteLine("---------------------------------------------------");
                while (!int.TryParse(Console.ReadLine(), out choice))
                {
                    Console.WriteLine("Invalid. Please enter a number");
                }

                switch (choice)
                {
                    case 1:
                        ViewAllAppointments(doctor.UserId);
                        break;
                    case 2:
                        ViewPastAppointments(doctor.UserId);
                        break;
                    case 3:
                        ViewTodayAppoinments(doctor.UserId);
                        break;
                    case 4:
                        PaymentAndRemark(doctor.UserId);
                        break;
                    case 0:
                        Console.WriteLine("Bye Bye");
                        break;
                    default:
                        Console.WriteLine("Invalid. Please enter a number");
                        break;

                }

            } while (choice != 0);
        }

        private void PaymentAndRemark(int doctorId)
        {
            var comingAppointment = appointments.Where(a => a.DoctorId == doctorId && a.Start_DateTime < DateTime.Now && a.isDeleted == 0
                                        && (a.DoctorRemarks == "" || a.Fees == null) && a.PaymentStatus == "").OrderBy(a => a.Start_DateTime);
            if (!comingAppointment.Any())
            {
                Console.WriteLine("There is no appointment need to take any action.");
                return;
            }
            else
            {
                int decision = 0;
                bool isAppValid = true;
                Appointment selectedAppointment;

                PrintAppointments(comingAppointment);
                do
                {
                    int id = GetIdFromUser();
                    selectedAppointment = comingAppointment.Where(a => a.AppointmentId == id).SingleOrDefault();

                    if (selectedAppointment == null)
                    {
                        isAppValid = false;
                        Console.WriteLine("Invalid Appointment Id");
                    }
                } while (!isAppValid);

                double fee = 0.00;
                do
                {
                    Console.WriteLine("You would like to....");
                    Console.WriteLine("1. Put remarks");
                    Console.WriteLine("2. Raise Fees");
                    Console.WriteLine("0. Exit");

                    while (!Int32.TryParse(Console.ReadLine(), out decision))
                    {
                        Console.WriteLine("Invalid, please try again.");
                    }

                    switch (decision)
                    {
                        case 1:
                            Console.WriteLine("Put the remarks :");
                            selectedAppointment.DoctorRemarks = Console.ReadLine();
                            break;
                        case 2:
                            Console.WriteLine("Key in the fees :");
                            while (!Double.TryParse(Console.ReadLine(), out fee))
                            {
                                Console.WriteLine("Please key in a right figure.");
                            }
                            selectedAppointment.Fees = fee;
                            selectedAppointment.PaymentStatus = "Pending";
                            break;
                        case 0:
                            PrintAppointment(selectedAppointment);
                            break;
                        default:
                            Console.WriteLine("Invalid input.");
                            break;
                    }
                } while (decision != 0);
            }
        }

        private void ViewPastAppointments(int doctorId)
        {
            DateTime today = DateTime.Today;

            var pastAppointment = appointments.Where(a => a.DoctorId == doctorId && a.Start_DateTime < today && a.isDeleted == 0).OrderBy(a => a.Start_DateTime);


            if (!pastAppointment.Any())
            {
                Console.WriteLine("There is no past appointment.");
                return;
            }
            else
            {
                PrintAppointments(pastAppointment);
            }
        }

        private void ViewTodayAppoinments(int doctorId)
        {
            DateTime startDateTime = DateTime.Today; //Today at 00:00:00
            DateTime endDateTime = DateTime.Today.AddDays(1).AddTicks(-1); //Today at 23:59:59
            var todayAppointment = appointments.Where(a => a.DoctorId == doctorId && (a.Start_DateTime >= startDateTime && a.Start_DateTime <= endDateTime) && a.isDeleted == 0).OrderBy(a => a.Start_DateTime);


            if (!todayAppointment.Any())
            {
                Console.WriteLine("There is no upcoming appointment.");
                return;
            }
            else
            {
                PrintAppointments(todayAppointment);
            }
        }

        public void ViewAllAppointments(int doctorId)
        {
            var comingAppointment = appointments.Where(a => a.DoctorId == doctorId && a.isDeleted == 0).OrderBy(a => a.Start_DateTime);


            if (!comingAppointment.Any())
            {
                Console.WriteLine("There is no upcoming appointment.");
                return;
            }
            else
            {
                PrintAppointments(comingAppointment);
            }
        }

        public void CancelAppointment(int patientId)
        {
            var now = DateTime.Now;
            var comingAppointment = appointments.Where(a => a.PatientId == patientId && a.Start_DateTime > now && a.isDeleted == 0).OrderBy(a => a.Start_DateTime);
            if (!comingAppointment.Any())
            {
                Console.WriteLine("There is no upcoming appointment.");
            }
            else
            {
                int decision = 0;
                PrintAppointments(comingAppointment);
                int id = GetIdFromUser();

                var selectedAppointment = comingAppointment.Where(a => a.AppointmentId == id).SingleOrDefault();

                if (selectedAppointment == null)
                {
                    Console.WriteLine("Invalid Appointment Id");
                }
                else
                {
                    Console.WriteLine("Do you want to cancel the following Appointment? \n1.Yes \n2.No");
                    do
                    {
                        while (!Int32.TryParse(Console.ReadLine(), out decision))
                        {
                            Console.WriteLine("Invalid, please insert 1 or 2.");
                        }

                        switch (decision)
                        {
                            case 1:
                                selectedAppointment.isDeleted = 1;
                                Console.WriteLine("Your appointment with " + selectedAppointment.DoctorId + "is canceled");
                                break;
                            case 2:
                                Console.WriteLine("Your appointment is still remain no changes.");
                                break;
                            default:
                                Console.WriteLine("Invalid input.");
                                break;
                        }
                    } while (decision != 1 && decision != 2);
                }
            }

            return;
        }

        public bool AssureChangesMade()
        {

            Console.WriteLine("Please check if all your new changes is correct.");
            Console.WriteLine("1. Press 1 to confirm changes.");
            Console.WriteLine("2. Press 2 to cancel your changes.");
            int select = Convert.ToInt32(Console.ReadLine());
            switch (select)
            {
                case 1:
                    return true;
                case 2:
                    Console.WriteLine("No changes made.");
                    return false;
                default:
                    Console.WriteLine("Invalid. Please try again...");
                    return false;
            }
        }

        public bool CheckDoctorAvailablity(int docId, List<Doctor> doctors, bool isDocIdValid = false)
        {
            var docIds = doctors.Select(e => e.UserId).ToList();
            return isDocIdValid = (from ids in docIds select ids).Contains(docId);
        }

        public int GetAppointmentDoctor()
        {
            ManageUser mu = new ManageUser();
            List<Doctor> doctors = mu.GetAllDoctor();
            mu.PrintAllDoctors(doctors);
            int doctorId;
            bool isDocIdValid = false;

            do
            {
                Console.WriteLine("Please enter the doctor's id");
                doctorId = Convert.ToInt32(Console.ReadLine());
                var searchId = doctors.SingleOrDefault(e => e.UserId == doctorId);
                isDocIdValid = searchId == null ? false : true;
                if (!isDocIdValid)
                    Console.WriteLine("Please enter a valid Doctor's Id. Try again...");

            } while (isDocIdValid == false);

            return doctorId;
        }

        public DateTime GetAppointmentDateTime()
        {
            bool isClinicOpen = true;
            DateTime start_DateTime;
            do
            {
                string[] date;
                string[] time;
                bool isDateTimeFormatValid;
                isClinicOpen = true;
                int intDateTIme = 0;
                do
                {
                    isDateTimeFormatValid = true;
                    Console.WriteLine("Please enter the date : yyyy/mm/dd");
                    string dateInput = Console.ReadLine();
                    date = dateInput.Split('/');
                    Console.WriteLine("Please enter the time : hh:mm");
                    string timeInput = Console.ReadLine();
                    time = timeInput.Split(':');

                    if ((!Int32.TryParse((date[0]), out intDateTIme)) || (!Int32.TryParse((date[1]), out intDateTIme)) || (!Int32.TryParse((date[2]), out intDateTIme)) ||
                            (!Int32.TryParse((time[0]), out intDateTIme)))
                    {
                        Console.WriteLine("Fail. Please input the date and time with correct format.");
                        isDateTimeFormatValid = false;
                    }

                } while (!isDateTimeFormatValid) ;

                start_DateTime = new DateTime(Convert.ToInt32(date[0]), Convert.ToInt32(date[1]), Convert.ToInt32(date[2]), Convert.ToInt32(time[0]), Convert.ToInt32(time[1]), 0);

                DateTime openHour = start_DateTime.Date.AddHours(9);
                DateTime closeHour = start_DateTime.Date.AddHours(18);

                if (start_DateTime < openHour || start_DateTime > closeHour)
                {
                    Console.WriteLine("Sorry, clinic is closed in this hour.");
                    isClinicOpen = false;
                }

                if (start_DateTime < DateTime.Now)
                {
                    Console.WriteLine("Invalid date.");
                    isClinicOpen = false;
                }

            } while (!isClinicOpen);

            Console.WriteLine("Date and Time is : " + start_DateTime);
            return start_DateTime;
        }
    }
}
