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

                    _class.Genre = rdr["Gender"].ToString();
                    _class.Level = rdr["Level"].ToString();
                    _class.PricePerHour = (float)rdr["PricePerHour"];
                    _class.SheduleId = (int)rdr["SheduleId"];
                    _class.ClassTypeId = (int)rdr["ClassTypeId"];

                    lstClasses.Add(_class);
                }
                con.Close();
            }
            return lstClasses;
        }
    }
}
