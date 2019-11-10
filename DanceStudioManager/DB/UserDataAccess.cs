using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
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

                cmd.Parameters.Add("@userId", SqlDbType.Int);
                if (user.Id == 0) cmd.Parameters["@userId"].Value = DBNull.Value;
                else cmd.Parameters["@userId"].Value = user.Id;

                cmd.Parameters.AddWithValue("@ConfirmAccount", user.ConfirmAccount);

                cmd.Parameters.Add("@Password", SqlDbType.VarChar);
                if (user.Password == null) cmd.Parameters["@Password"].Value = DBNull.Value;
                else cmd.Parameters["@Password"].Value = user.Password;

                cmd.Parameters.Add("@Username", SqlDbType.VarChar);
                if (user.Username == null) cmd.Parameters["@Username"].Value = DBNull.Value;
                else cmd.Parameters["@Username"].Value = user.Username;

                cmd.Parameters.Add("@Email", SqlDbType.VarChar);
                if (user.Email == null) cmd.Parameters["@Email"].Value = DBNull.Value;
                else cmd.Parameters["@Email"].Value = user.Email;

                cmd.Parameters.Add("@Salt", SqlDbType.VarChar);
                if (user.Salt == null) cmd.Parameters["@Salt"].Value = DBNull.Value;
                else cmd.Parameters["@Salt"].Value = user.Salt;

                cmd.Parameters.Add("@ModifiedBy", SqlDbType.Int);
                if (user.Id == 0) cmd.Parameters["@ModifiedBy"].Value = DBNull.Value;
                else cmd.Parameters["@ModifiedBy"].Value = user.Id;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

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
                    user.Salt = rdr["Salt"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.Email= rdr["Email"].ToString();
                    user.ConfirmAccount = (bool)rdr["ConfirmAccount"];
                    user.StudioId = (int)rdr["StudioId"];
                }
                con.Close();
                return user;
            }
        }
        public async void SignIn(HttpContext httpContext,int userId, bool isPersistent = false)
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
                    user.Username = rdr["Username"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.Email = rdr["Email"].ToString();
                }
                con.Close();

                ClaimsIdentity identity = new ClaimsIdentity(this.GetUserClaims(user), CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            }
        }

        public async void SignOut(HttpContext httpContext)
        {
            await httpContext.SignOutAsync();
        }

        private IEnumerable<Claim> GetUserClaims(User user)
        {

            var claims = new List<Claim>
            {
                new Claim("Name", user.Username),
                new Claim("Email", user.Email),
            };
            return claims;
        }

    }
}
