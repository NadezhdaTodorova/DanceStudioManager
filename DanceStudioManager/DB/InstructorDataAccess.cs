using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class InstructorDataAccess
    {
        private readonly ApplicationContext applicationContext;

        public InstructorDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        public List<Instructor> GetAllInstructors()
        {
            List<Instructor> lstInstructors = new List<Instructor>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllInstructors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Instructor instructor = new Instructor();

                    instructor.Firstname = rdr["FirstName"].ToString();
                    instructor.Lastname = rdr["LastName"].ToString();
                    instructor.CellPhone = rdr["CellPhone"].ToString();
                    instructor.Email = rdr["Email"].ToString();
                    instructor.SendEmail = (bool)rdr["SendEmail"];
                    instructor.Gender = rdr["Gender"].ToString();
                    instructor.Id = (int)rdr["Id"];

                    lstInstructors.Add(instructor);
                }
                con.Close();
            }
            return lstInstructors;
        }

        public List<Instructor> SearchInstructors(Instructor instructor)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                List<Instructor> instructors = new List<Instructor>();
                SqlCommand cmd = new SqlCommand("SearchInstructors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Firstname", instructor.Firstname);
                cmd.Parameters.AddWithValue("@Lastname", instructor.Lastname);
                cmd.Parameters.AddWithValue("@Email", instructor.Email);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Instructor i = new Instructor();

                    i.Id = (int)rdr["Id"];
                    i.Firstname = rdr["Firstname"].ToString();
                    i.Lastname = rdr["Lastname"].ToString();
                    i.Email = rdr["Email"].ToString();
                    i.Gender = rdr["Gender"].ToString();

                    instructors.Add(i);
                }
                con.Close();
                return instructors;
            }
        }

        public void AddNewInstructor(Instructor instructor, int studioId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddInstructor", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Firstname", instructor.Firstname);
                cmd.Parameters.AddWithValue("@Lastname", instructor.Lastname);
                cmd.Parameters.AddWithValue("@Email", instructor.Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", instructor.DateOfBirth);
                cmd.Parameters.AddWithValue("@CellPhone", instructor.CellPhone);
                cmd.Parameters.AddWithValue("@SendEmail", instructor.SendEmail);
                cmd.Parameters.AddWithValue("@Gender", instructor.Gender);
                cmd.Parameters.AddWithValue("@StudioId", studioId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public Instructor GetInstructorById(int id)
        {
            Instructor i = new Instructor();
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                
                SqlCommand cmd = new SqlCommand("GetInstructorById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    

                    i.Id = (int)rdr["Id"];
                    i.Firstname = rdr["Firstname"].ToString();
                    i.Lastname = rdr["Lastname"].ToString();
                    i.Email = rdr["Email"].ToString();
                    i.Gender = rdr["Gender"].ToString();

                }
                con.Close();
            }

            return i;
        }
    }
}
