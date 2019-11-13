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

        public List<Class> GetAllClasses(int studioId)
        {
            List<Class> lstClasses = new List<Class>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllClasses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

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
                    _class.StartDay = (DateTime)rdr["StartDay"];

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

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

                cmd.Parameters.Add("@StartDay", SqlDbType.DateTime);
                if (_class.StartDay == default(DateTime)) cmd.Parameters["@StartDay"].Value = default(DateTime); 
                else cmd.Parameters["@StartDay"].Value = _class.StartDay;

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

        public List<int> GetInstructorsConnectedToClass(int classId, int StudioId)
        {
            List<int> instructorsIds = new List<int>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetInstructorsConnectedToClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Classid", classId);
                cmd.Parameters.AddWithValue("@StudioId", StudioId);

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

        public List<int> GetStudentsConnectedToClass(int classId, int studioId)
        {
            List<int> studentsIds = new List<int>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetStudentsConnectedToClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Classid", classId);
                cmd.Parameters.AddWithValue("@StudioId", studioId);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int id = (int)rdr["StudentId"];
                    studentsIds.Add(id);
                }
                con.Close();
            }
            return studentsIds;
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

        public Class SearchClass(int classId, int studioId)
        {
            Class _class = new Class();
            string genre = null; 
            string type = null;
            string level = null;

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SearchClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                cmd.Parameters.Add("@Genre", SqlDbType.VarChar);
                if (genre == null) cmd.Parameters["@Genre"].Value = DBNull.Value;
                else cmd.Parameters["@Genre"].Value = genre;

                cmd.Parameters.Add("@Level", SqlDbType.VarChar);
                if (level == null) cmd.Parameters["@Level"].Value = DBNull.Value;
                else cmd.Parameters["@Level"].Value = level;

                cmd.Parameters.Add("@Type", SqlDbType.VarChar);
                if (type == null) cmd.Parameters["@Type"].Value = DBNull.Value;
                else cmd.Parameters["@Type"].Value = type;

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    _class.Id = (int)rdr["ID"];
                    _class.Genre = (string)rdr["Genre"];
                    _class.Level = (string)rdr["Level"];
                    _class.PricePerHour = (double)rdr["PricePerHour"];
                }
                con.Close();
            }

            return _class;
        }

         public List<Class> SearchClass(string genre, string level, string type, int studioId)
        {
            List<Class> _classes = new List<Class> ();
            int classId = 0;

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("SearchClass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@ClassId", SqlDbType.Int);
                if (classId == 0) cmd.Parameters["@ClassId"].Value = DBNull.Value;
                else cmd.Parameters["@ClassId"].Value = classId;

                cmd.Parameters.Add("@Genre", SqlDbType.VarChar);
                if (genre == "null") cmd.Parameters["@Genre"].Value = DBNull.Value;
                else cmd.Parameters["@Genre"].Value = genre;

                cmd.Parameters.Add("@Level", SqlDbType.VarChar);
                if (level == "null") cmd.Parameters["@Level"].Value = DBNull.Value;
                else cmd.Parameters["@Level"].Value = level;

                cmd.Parameters.Add("@Type", SqlDbType.VarChar);
                if (type == "null") cmd.Parameters["@Type"].Value = DBNull.Value;
                else cmd.Parameters["@Type"].Value = type;

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var _class = new Class();
                    _class.Id = (int)rdr["ID"];
                    _class.Genre = (string)rdr["Genre"];
                    _class.Level = (string)rdr["Level"];
                    _class.PricePerHour = (double)rdr["PricePerHour"];
                    _class.ClassType = (string)rdr["ClassType"];
                    _class.NumberOfStudents = (int)rdr["NumberOfStudents"];

                    _classes.Add(_class);
                }
                con.Close();
            }

            return _classes;
        }

        public void RemoveClass(int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("RemoveClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClassId", classId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void UpdateClass(Class _class, int userId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ClassId", _class.Id);

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

                cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                if (userId == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                else cmd.Parameters["@ModifiedBy"].Value = userId;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
