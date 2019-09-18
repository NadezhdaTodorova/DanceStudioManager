using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Claims;

namespace DanceStudioManager
{
    public class CalendarDataAccess
    {
        private readonly ApplicationContext applicationContext;
        private readonly ClassDataAccess classDataAccess;
        private readonly InstructorDataAccess instructorDataAccess;

        public CalendarDataAccess(ApplicationContext _applicationContext, ClassDataAccess _classDataAccess, InstructorDataAccess _instructorDataAccess)
        {
            applicationContext = _applicationContext;
            classDataAccess = _classDataAccess;
            instructorDataAccess = _instructorDataAccess;
        }

        public void AddDaysToCalendar(List<DayVM> days)
        {
            int? userId = 0;
            foreach (var day in days)
            {
                using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("InsertCalendar", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@CreatedBy", SqlDbType.Int);
                    if (userId == 0) cmd.Parameters["@CreatedBy"].Value = DBNull.Value;
                    else cmd.Parameters["@CreatedBy"].Value = userId;

                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                    if (userId == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                    else cmd.Parameters["@ModifiedBy"].Value = userId;

                    cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime);
                    if (userId == 0) cmd.Parameters["@ModifiedOn"].Value = DBNull.Value;
                    else cmd.Parameters["@ModifiedOn"].Value = userId;

                    cmd.Parameters.Add("@Day", SqlDbType.DateTime);
                    if (day.Day == null) cmd.Parameters["@Day"].Value = DBNull.Value;
                    else cmd.Parameters["@Day"].Value = day.Day;

                    cmd.Parameters.AddWithValue("@WorkDay", day.WorkDay);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        public List<CalendarData> GetAllClassesShedule(CalendarSearchVM calendarSearch)
        {
            var calData = new List<CalendarData>();
            List<DayVM> classesDays = new List<DayVM>();
            List<DayVM> days = GetDays();
            List<Class> classes = classDataAccess.GetAllClasses();
            



            foreach (var _class in classes)
            {
                List<Shedule> shedules = classDataAccess.GetClassShedule(_class.Id);
                List<int> idInstructors = classDataAccess.GetInstructorsConnectedToClass(_class.Id);
                List<Instructor> instructors = new List<Instructor>();

                foreach (var id in idInstructors)
                {
                    instructors.Add(instructorDataAccess.GetInstructorById(id));
                }

                foreach (var s in shedules)
                {
                    foreach (var day in days)
                    {
                        if (day.Day.DayOfWeek.ToString() == s.Day)
                        {
                            classesDays.Add(new DayVM { Day = day.Day.Date, WorkDay = day.WorkDay });
                        }
                    }

                    calData.Add(new CalendarData { Hour = s.Hour, Level = _class.Level, Name = _class.Genre, SheduleDays = classesDays, Instructors = instructors, NumberOfStudents = _class.NumberOfStudents });
                }
            }



            return calData;
        }

        public List<DayVM> GetDays()
        {
            var days = new List<DayVM>();
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {

                SqlCommand cmd = new SqlCommand("GetDays", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var day = (DateTime)rdr["Day"];
                    var workDay = (bool)rdr["WorkDay"];

                    days.Add(new DayVM { Day = day, WorkDay = workDay });
                }
                con.Close();
            }

            return days;
        }
    }
}
