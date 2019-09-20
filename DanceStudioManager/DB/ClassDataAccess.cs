using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class ClassDataAccess
    {
        private readonly ApplicationContext applicationContext;

        public ClassDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        public List<Class> GetAllClasses()
        {
            List<Class> lstClasses = new List<Class>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllClasses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Class _class = new Class();

                    _class.Genre = rdr["Genre"].ToString();
                    _class.Level = rdr["Level"].ToString();
                    _class.PricePerHour = (double)rdr["PricePerHour"];
                    _class.ClassType = rdr["ClassType"].ToString();
                    _class.NumberOfStudents = (int)rdr["NumberOfStudents"];
                    _class.Id = (int)rdr["Id"];

                    lstClasses.Add(_class);
                }
                con.Close();
            }
            return lstClasses;
        }

        public void AddNewClass(Class _class, int studioId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Genre", SqlDbType.VarChar);
                if (_class.Genre == null) cmd.Parameters["@Genre"].Value = DBNull.Value;
                else cmd.Parameters["@Genre"].Value = _class.Genre;

                cmd.Parameters.Add("@Level", SqlDbType.VarChar);
                if (_class.Level == null) cmd.Parameters["@Level"].Value = DBNull.Value;
                else cmd.Parameters["@Level"].Value = _class.Level;

                cmd.Parameters.Add("@PricePerHour", SqlDbType.Float);
                if (_class.PricePerHour == 0) cmd.Parameters["@PricePerHour"].Value = DBNull.Value;
                else cmd.Parameters["@PricePerHour"].Value = _class.PricePerHour;

                cmd.Parameters.Add("@ClassType", SqlDbType.VarChar);
                if (_class.ClassType == null) cmd.Parameters["@ClassType"].Value = DBNull.Value;
                else cmd.Parameters["@ClassType"].Value = _class.ClassType;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddNewShedule(int classId, int studioId, string shedule)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddShedule", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClassId", classId);
                cmd.Parameters.AddWithValue("@StudioId", studioId);
                cmd.Parameters.AddWithValue("@Repetition", shedule);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddClassType(string type, int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddClassType", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@ClassId", classId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetClassId(Class _class)
        {
            int classId = 0;

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetClassId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@Level", SqlDbType.VarChar);
                if (_class.Level == null) cmd.Parameters["@Level"].Value = DBNull.Value;
                else cmd.Parameters["@Level"].Value = _class.Level;

                cmd.Parameters.Add("@Genre", SqlDbType.VarChar);
                if (_class.Genre == null) cmd.Parameters["@Genre"].Value = DBNull.Value;
                else cmd.Parameters["@Genre"].Value = _class.Genre;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    _class.Id = (int)rdr["ID"];
                    classId = _class.Id;
                }
                con.Close();
            }

            return classId;
        }

        public void AddStudentToClass(int studentId, int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddStudentToClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@ClassId", classId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void AddInstructorToClass(int instructorId, int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddInstructorToClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InstructorId", instructorId);
                cmd.Parameters.AddWithValue("@ClassId", classId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<int> GetInstructorsConnectedToClass(int classId)
        {
            List<int> instructorsIds = new List<int>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetInstructorsConnectedToClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Classid", classId);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int id = (int)rdr["InstructorId"];
                    instructorsIds.Add(id);
                }
                con.Close();
            }
            return instructorsIds;
        }

        public void AddDayToShedule(string day, int classId, string hour, int studioId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddDayToShedule", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@Day", SqlDbType.VarChar);
                if (day == null) cmd.Parameters["@Day"].Value = DBNull.Value;
                else cmd.Parameters["@Day"].Value = day;

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                cmd.Parameters.Add("@Hour", SqlDbType.VarChar);
                if (hour == null) cmd.Parameters["@Hour"].Value = DBNull.Value;
                else cmd.Parameters["@Hour"].Value = hour;

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<Shedule> GetClassShedule(int classId)
        {
            List<Shedule> shedules = new List<Shedule>();
            

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetClassShedule", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Classid", classId);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var shedule = new Shedule();
                    shedule.Id = (int)rdr["ID"];
                    shedule.ClassId = (int)rdr["ClassId"];
                    shedule.Day = (string)rdr["Day"];
                    shedule.Hour = (string)rdr["Hour"];
                    shedules.Add(shedule);
                }
                con.Close();
            }

            return shedules;
        }
    }
}
