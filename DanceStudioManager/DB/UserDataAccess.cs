using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class UserDataAccess 
    {
        private readonly ApplicationContext applicationContext;

        public UserDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        //To View all user details    
        public IEnumerable<User> GetAllUsers()
        {
            List<User> lstUsers = new List<User>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    User user = new User();

                    user.Username = rdr["Username"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.StudioName = rdr["StudioId"].ToString();
                    user.Email = rdr["Email"].ToString();

                    lstUsers.Add(user);
                }
                con.Close();
            }
            return lstUsers;
        }

        public void AddNewUser(User user, int studioId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", user.Username);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@StudioId", studioId);
                cmd.Parameters.AddWithValue("@ConfirmAccount", user.ConfirmAccount);
                cmd.Parameters.AddWithValue("@Salt", user.Salt);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetUserId(User user)
        {
            int userId = 0;

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetUserId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Email", user.Email);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.Id = (int)rdr["ID"];
                    userId = user.Id;
                }
                con.Close();
            }
            return userId;
        }

        public void UpdateUser(User user)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@userId", user.Id);
                cmd.Parameters.AddWithValue("@ConfirmAccount", user.ConfirmAccount);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                User user = new User();
                SqlCommand cmd = new SqlCommand("GetUserById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@userId", userId);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    user.Id = (int)rdr["Id"];
                    user.Username = rdr["Username"].ToString();
                    user.Salt = (byte[])rdr["Salt"];
                    user.Password = rdr["Password"].ToString();
                }
                con.Close();
                return user;
            }
        }


    }
}
