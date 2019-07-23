﻿using System;
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
                cmd.Parameters.AddWithValue("@Password", studio.Password);

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
                cmd.Parameters.AddWithValue("@Password", studio.Password);

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
    }
}
