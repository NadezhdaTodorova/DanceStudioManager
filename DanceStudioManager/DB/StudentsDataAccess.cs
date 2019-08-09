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

                    lstStudents.Add(student);
                }
                con.Close();
            }
            return lstStudents;
        }
    }
}
