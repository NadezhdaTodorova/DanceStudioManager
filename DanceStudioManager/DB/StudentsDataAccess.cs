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

        public List<Student> GetAllStudents(int studioId)
        {
            List<Student> lstStudents = new List<Student>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllStudents", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

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
                    student.DateOfBirth = (DateTime)rdr["DateOfBirth"];
                    student.Id = (int)rdr["Id"];
                    student.DateOfBirthToString = student.DateOfBirth.Date.ToString("yyyy-MM-dd");

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

                cmd.Parameters.Add("@Firstname", SqlDbType.VarChar);
                if (student.Firstname == "null") cmd.Parameters["@Firstname"].Value = DBNull.Value;
                else cmd.Parameters["@Firstname"].Value = student.Firstname;

                cmd.Parameters.Add("@Lastname", SqlDbType.VarChar);
                if (student.Lastname == "null") cmd.Parameters["@Lastname"].Value = DBNull.Value;
                else cmd.Parameters["@Lastname"].Value = student.Lastname;

                cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                if (student.Email == "null") cmd.Parameters["@Email"].Value = DBNull.Value;
                else cmd.Parameters["@Email"].Value = student.Email;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Student s = new Student();

                    s.Id = (int)rdr["Id"];
                    s.Firstname = rdr["Firstname"].ToString();
                    s.Lastname = rdr["Lastname"].ToString();
                    s.Email = rdr["Email"].ToString();
                    s.Gender = rdr["Gender"].ToString();
                    s.CellPhone = rdr["CellPhone"].ToString();
                    s.DateOfBirth = (DateTime)rdr["DateOfBirth"];
                    s.DateOfBirthToString = s.DateOfBirth.Date.ToString("yyyy-MM-dd");

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
        public Student GetStudentById(int id)
        {
            Student i = new Student();
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {

                SqlCommand cmd = new SqlCommand("GetStudentById", con);
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

        public void UpdateStudent(Student student, int userId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", student.Id);

                cmd.Parameters.Add("@Firstname", SqlDbType.VarChar);
                if (student.Firstname == null) cmd.Parameters["@Firstname"].Value = DBNull.Value;
                else cmd.Parameters["@Firstname"].Value = student.Firstname;

                cmd.Parameters.Add("@Lastname", SqlDbType.VarChar);
                if (student.Lastname == null) cmd.Parameters["@Lastname"].Value = DBNull.Value;
                else cmd.Parameters["@Lastname"].Value = student.Lastname;

                cmd.Parameters.Add("@CellPhone", SqlDbType.VarChar);
                if (student.CellPhone == null) cmd.Parameters["@CellPhone"].Value = DBNull.Value;
                else cmd.Parameters["@CellPhone"].Value = student.CellPhone;

                cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                if (student.Email == null) cmd.Parameters["@Email"].Value = DBNull.Value;
                else cmd.Parameters["@Email"].Value = student.Email;

                cmd.Parameters.Add("@SendEmail", SqlDbType.Bit);
                if (student.SendEmail == false) cmd.Parameters["@SendEmail"].Value = false;
                else cmd.Parameters["@SendEmail"].Value = student.SendEmail;

                cmd.Parameters.Add("@Gender", SqlDbType.VarChar);
                if (student.Gender == null) cmd.Parameters["@Gender"].Value = DBNull.Value;
                else cmd.Parameters["@Gender"].Value = student.Gender;

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (student.StudioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = student.StudioId;

                cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime);
                if (student.DateOfBirthToString == null) cmd.Parameters["@DateOfBirth"].Value = DBNull.Value;
                else cmd.Parameters["@DateOfBirth"].Value = DateTime.Parse(student.DateOfBirthToString); ;

                cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                if (userId == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                else cmd.Parameters["@ModifiedBy"].Value = userId;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteStudent(Student student, int userId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", student.Id);

                cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                if (userId == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                else cmd.Parameters["@ModifiedBy"].Value = userId;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteStudentFromClass(int studentId, int userId, int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteStudentFromClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StudentId", studentId);

                cmd.Parameters.AddWithValue("@ClassId", classId);

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
