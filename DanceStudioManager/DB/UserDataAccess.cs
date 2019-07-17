using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

                    user.ID = Convert.ToInt32(rdr["Id"]);
                    user.Username = rdr["Username"].ToString();
                    user.Password = rdr["Password"].ToString();
                    user.StudioId = rdr["StudioId"].ToString();
                    user.Email = rdr["Email"].ToString();

                    lstUsers.Add(user);
                }
                con.Close();
            }
            return lstUsers;
        }
    }
}
