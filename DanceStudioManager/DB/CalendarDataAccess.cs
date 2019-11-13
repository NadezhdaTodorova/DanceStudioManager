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
            foreach (var day in days)
            {
                using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("InsertCalendar", con);
                    cmd.CommandType = CommandType.StoredProcedure;

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

        public List<CalendarData> GetAllClassesShedule(CalendarSearchVM calendarSearch, int studioId)
        {
            var calData = new List<CalendarData>();

            List<Class> classes = classDataAccess.GetAllClasses(studioId);

            foreach (var _class in classes)
            {
                List<Shedule> shedules = classDataAccess.GetClassShedule(_class.Id);
                List<int> idInstructors = classDataAccess.GetInstructorsConnectedToClass(_class.Id, studioId);
                List<Instructor> instructors = new List<Instructor>();

                foreach (var id in idInstructors)
                {
                    instructors.Add(instructorDataAccess.GetInstructorById(id));
                }

                foreach (var s in shedules)
                {
                    List<DayVM> classesDays = new List<DayVM>();
                    foreach (var day in calendarSearch.Days)
                    {
                        if ((day.Day.DayOfWeek.ToString() == s.Day) && _class.StartDay.Date < day.Day.Date)
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
