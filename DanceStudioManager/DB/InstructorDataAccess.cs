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

        public List<Instructor> GetAllInstructors(int studioId)
        {
            List<Instructor> lstInstructors = new List<Instructor>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllInstructors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (studioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = studioId;

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
                    instructor.DateOfBirth = (DateTime)rdr["DateOfBirth"];
                    instructor.DateOfBirthToString = instructor.DateOfBirth.Date.ToString("yyyy-MM-dd");

                    if (!DBNull.Value.Equals(rdr["ProcenOfProfit"]))
                    {
                        instructor.procentOfProfit = (int)rdr["ProcenOfProfit"];
                    }
                    else
                    {
                        instructor.procentOfProfit = 0;
                    }
                    

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

                cmd.Parameters.Add("@Firstname", SqlDbType.VarChar);
                if (instructor.Firstname == "null") cmd.Parameters["@Firstname"].Value = DBNull.Value;
                else cmd.Parameters["@Firstname"].Value = instructor.Firstname;

                cmd.Parameters.Add("@Lastname", SqlDbType.VarChar);
                if (instructor.Lastname == "null") cmd.Parameters["@Lastname"].Value = DBNull.Value;
                else cmd.Parameters["@Lastname"].Value = instructor.Lastname;

                cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                if (instructor.Email == "null") cmd.Parameters["@Email"].Value = DBNull.Value;
                else cmd.Parameters["@Email"].Value = instructor.Email;

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Instructor i = new Instructor();

                    i.Id = (int)rdr["Id"];
                    i.Firstname = rdr["Firstname"].ToString();
                    i.Lastname = rdr["Lastname"].ToString();
                    i.Email = rdr["Email"].ToString();
                    i.Gender = rdr["Gender"].ToString();
                    i.CellPhone = rdr["CellPhone"].ToString();
                    i.DateOfBirth = (DateTime)rdr["DateOfBirth"];
                    i.DateOfBirthToString = i.DateOfBirth.Date.ToString("yyyy-MM-dd");

                    if (!DBNull.Value.Equals(rdr["ProcenOfProfit"]))
                    {
                        i.procentOfProfit = (int)rdr["ProcenOfProfit"];
                    }
                    else
                    {
                        i.procentOfProfit = 0;
                    }

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
                cmd.Parameters.AddWithValue("@DateOfBirth", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@CellPhone", instructor.CellPhone);
                cmd.Parameters.AddWithValue("@SendEmail", instructor.SendEmail);
                cmd.Parameters.AddWithValue("@Gender", instructor.Gender);
                cmd.Parameters.AddWithValue("@StudioId", studioId);
                cmd.Parameters.AddWithValue("@ProcentOfProfit", instructor.procentOfProfit);

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
                    i.procentOfProfit = (int)rdr["ProcenOfProfit"];

                }
                con.Close();
            }

            return i;
        }

        public void UpdateInstructor(Instructor instructor, int userId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateInstructor", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InstructorId", instructor.Id);

                cmd.Parameters.Add("@Firstname", SqlDbType.VarChar);
                if (instructor.Firstname == null) cmd.Parameters["@Firstname"].Value = DBNull.Value;
                else cmd.Parameters["@Firstname"].Value = instructor.Firstname;

                cmd.Parameters.Add("@Lastname", SqlDbType.VarChar);
                if (instructor.Lastname == null) cmd.Parameters["@Lastname"].Value = DBNull.Value;
                else cmd.Parameters["@Lastname"].Value = instructor.Lastname;

                cmd.Parameters.Add("@CellPhone", SqlDbType.VarChar);
                if (instructor.CellPhone == null) cmd.Parameters["@CellPhone"].Value = DBNull.Value;
                else cmd.Parameters["@CellPhone"].Value = instructor.CellPhone;

                cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                if (instructor.Email == null) cmd.Parameters["@Email"].Value = DBNull.Value;
                else cmd.Parameters["@Email"].Value = instructor.Email;

                cmd.Parameters.Add("@SendEmail", SqlDbType.Bit);
                if (instructor.SendEmail == false) cmd.Parameters["@SendEmail"].Value = false;
                else cmd.Parameters["@SendEmail"].Value = instructor.SendEmail;

                cmd.Parameters.Add("@Gender", SqlDbType.VarChar);
                if (instructor.Gender == null) cmd.Parameters["@Gender"].Value = DBNull.Value;
                else cmd.Parameters["@Gender"].Value = instructor.Gender;

                cmd.Parameters.Add("@StudioId", SqlDbType.Int);
                if (instructor.StudioId == 0) cmd.Parameters["@StudioId"].Value = DBNull.Value;
                else cmd.Parameters["@StudioId"].Value = instructor.StudioId;

                cmd.Parameters.Add("@ProcentOfProfit", SqlDbType.Int);
                if (instructor.procentOfProfit == 0) cmd.Parameters["@ProcentOfProfit"].Value = DBNull.Value;
                else cmd.Parameters["@ProcentOfProfit"].Value = instructor.procentOfProfit;

                cmd.Parameters.Add("@DateOfBirth", SqlDbType.DateTime);
                if (instructor.DateOfBirthToString == null) cmd.Parameters["@DateOfBirth"].Value = DBNull.Value;
                else cmd.Parameters["@DateOfBirth"].Value = DateTime.Parse(instructor.DateOfBirthToString); 

                cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                if (userId == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                else cmd.Parameters["@ModifiedBy"].Value = userId;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteInstructor(Instructor instructor, int userId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteInstructor", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InstructorId", instructor.Id);

                cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                if (userId == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                else cmd.Parameters["@ModifiedBy"].Value = userId;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public void DeleteInstructorFromClass(int instructorId, int userId, int classId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteInstructorFromClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@InstructorId", instructorId);

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
