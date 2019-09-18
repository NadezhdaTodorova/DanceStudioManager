using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class StudentsDataAccess
    {
        private readonly ApplicationContext applicationContext;

        public StudentsDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        public List<Student> GetAllStudents()
        {
            List<Student> lstStudents = new List<Student>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Student student = new Student();

                    student.Firstname = rdr["FirstName"].ToString();
                    student.Lastname = rdr["LastName"].ToString();
                    student.CellPhone = rdr["CellPhone"].ToString();
                    student.Email = rdr["Email"].ToString();
                    student.SendEmail = (bool)rdr["SendEmail"];
                    student.Gender = rdr["Gender"].ToString();
                    student.Id = (int)rdr["Id"];

                    lstStudents.Add(student);
                }
                con.Close();
            }
            return lstStudents;
        }

        public List<Student> SearchStudents(Student student)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                List<Student> students = new List<Student>();
                SqlCommand cmd = new SqlCommand("SearchStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Firstname", student.Firstname);
                cmd.Parameters.AddWithValue("@Lastname", student.Lastname);
                cmd.Parameters.AddWithValue("@Email", student.Email);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Student s = new Student();

                    s.Id = (int)rdr["Id"];
                    s.Firstname = rdr["Firstname"].ToString();
                    s.Lastname = rdr["Lastname"].ToString();
                    s.Email = rdr["Email"].ToString();
                    s.Gender = rdr["Gender"].ToString();

                    students.Add(s);
                }
                con.Close();
                return students;
            }
        }

        public void AddNewStudent(Student student, int studioId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Firstname", student.Firstname);
                cmd.Parameters.AddWithValue("@Lastname", student.Lastname);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                cmd.Parameters.AddWithValue("@CellPhone", student.CellPhone);
                cmd.Parameters.AddWithValue("@SendEmail", student.SendEmail);
                cmd.Parameters.AddWithValue("@Gender", student.Gender);
                cmd.Parameters.AddWithValue("@StudioId", studioId);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public Instructor GetStudentById(int id)
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
