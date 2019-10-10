using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class AttendanceDataAccess
    {
        private readonly ApplicationContext applicationContext;

        public AttendanceDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        public void AddAttendance(int classId, int studioId, DateTime date)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

                cmd.Parameters.Add("@Date", SqlDbType.DateTime);
                if (date == null) cmd.Parameters["@Date"].Value = DBNull.Value;
                else cmd.Parameters["@Date"].Value = date;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddStudentAttendance(int studentId, int attendanceId, int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddStudentAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                cmd.Parameters.Add("@AttendanceId", SqlDbType.Int);
                if (attendanceId == 0) cmd.Parameters["@AttendanceId"].Value = DBNull.Value;
                else cmd.Parameters["@AttendanceId"].Value = attendanceId;

                cmd.Parameters.Add("@StudentId", SqlDbType.Int);
                if (studentId == 0) cmd.Parameters["@StudentId"].Value = DBNull.Value;
                else cmd.Parameters["@StudentId"].Value = studentId;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddInstructorAttendance(int instructorId, int attendanceId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddInstructorAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@AttendanceId", SqlDbType.Int);
                if (attendanceId == 0) cmd.Parameters["@AttendanceId"].Value = DBNull.Value;
                else cmd.Parameters["@AttendanceId"].Value = attendanceId;

                cmd.Parameters.Add("@InstructorId", SqlDbType.Int);
                if (instructorId == 0) cmd.Parameters["@InstructorId"].Value = DBNull.Value;
                else cmd.Parameters["@InstructorId"].Value = instructorId;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetAttendanceId(int classId)
        {
            int attendanceId = 0;

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAttendanceId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    attendanceId = (int)rdr["ID"];
                }
                con.Close();
            }
            return attendanceId;
        }

        public List<Attendance> SearchAttendancesByClassId(int classId)
        {
            List<Attendance> attendances = new List<Attendance>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAttendanceId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Attendance attendance = new Attendance();
                    attendance.Id = (int)rdr["ID"];
                    attendance.ClassId = (int)rdr["ClassId"];
                    attendance.StudioId = (int)rdr["StudioId"];
                    attendance.Date = (DateTime)rdr["Date"];

                    attendances.Add(attendance);
                }
                con.Close();
            }
            return attendances;
        }
        public List<Attendance> GetAllAttendances()
        {
            List<Attendance> attendances = new List<Attendance>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllAttendances", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var attendance = new Attendance();
                    attendance.Id = (int)rdr["Id"];
                    attendance.ClassId = (int)rdr["ClassId"];
                    attendance.Date = (DateTime)rdr["Date"];
                    attendance.StudioId = (int)rdr["StudioId"];
                    attendances.Add(attendance);
                }
                con.Close();
            }
            return attendances;
        }
    }
}
