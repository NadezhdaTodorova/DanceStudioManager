using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class ClassDataAccess
    {
        private readonly ApplicationContext applicationContext;

        public ClassDataAccess(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        public List<Class> GetAllClasses()
        {
            List<Class> lstClasses = new List<Class>();

            using (SqlConnection con = new SqlConnection(applicationContext.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetAllClasses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Class _class = new Class();

                    _class.Genre = rdr["Genre"].ToString();
                    _class.Level = rdr["Level"].ToString();
                    _class.PricePerHour = (double)rdr["PricePerHour"];
                    _class.Shedule = rdr["Shedule"].ToString();
                    _class.ClassType = rdr["ClassType"].ToString();
                    _class.NumberOfStudents = (int)rdr["NumberOfStudents"];

                    lstClasses.Add(_class);
                }
                con.Close();
            }
            return lstClasses;
        }
    }
}
