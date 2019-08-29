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
                    _class.Shedule = rdr["Shedule"].ToString();
                    _class.ClassType = rdr["ClassType"].ToString();
                    _class.NumberOfStudents = (int)rdr["NumberOfStudents"];

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

                cmd.Parameters.AddWithValue("@Genre", _class.Genre);
                cmd.Parameters.AddWithValue("@Level", _class.Level);
                cmd.Parameters.AddWithValue("@PricePerHour", _class.PricePerHour);
                cmd.Parameters.AddWithValue("@Shedule", _class.Shedule);
                cmd.Parameters.AddWithValue("@ClassType", _class.ClassType);

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

                cmd.Parameters.AddWithValue("@Shedule", _class.Shedule);
                cmd.Parameters.AddWithValue("@Genre", _class.Genre);

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
    }
}
