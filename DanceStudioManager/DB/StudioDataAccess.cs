using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class StudioDataAccess
    {
        private readonly ApplicationContext applicationContext;

        public StudioDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        //To View all studios 
        public IEnumerable<Studio> GetAllStudios()
        {
            List<Studio> lstStudios = new List<Studio>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllStudios", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Studio studio = new Studio();

                    studio.Name = rdr["Name"].ToString();
                    studio.Password = rdr["Password"].ToString();

                    lstStudios.Add(studio);
                }
                con.Close();
            }
            return lstStudios;
        }

        public void AddNewStudio(Studio studio)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddStudio", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", studio.Name);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public int GetStudioId(Studio studio)
        {
            int studioId = 0;

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetStudioId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Name", studio.Name);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    studio.Id = (int)rdr["ID"];
                    studioId = studio.Id;
                }
                con.Close();
            }
            return studioId;
        }

        public Studio GetStudioInfo(int studioId)
        {

            Studio studio = new Studio();
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetStudioInfo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@Id", studioId);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    studio.Id = (int)rdr["ID"];
                    studio.Name = rdr["Name"].ToString();
                    studio.Photo_url = rdr["Photo_url"].ToString();

                }
                con.Close();
            }
            return studio;
        }

        public void UpdateStudio(Studio studio, int Id)
        {
            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateStudio", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", Id);

                cmd.Parameters.Add("@Name", SqlDbType.VarChar);
                if (studio.Name == null) cmd.Parameters["@Name"].Value = DBNull.Value;
                else cmd.Parameters["@Name"].Value = studio.Name;

                cmd.Parameters.AddWithValue("@ModifiedOn", DateTime.Now);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
