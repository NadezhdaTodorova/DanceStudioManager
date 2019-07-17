using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DanceStudioManager
{
    public class ApplicationContext 
    {
        private readonly IConfiguration configuration;
        public ApplicationContext(IConfiguration config)
        {
            configuration = config;
        }

        public string GetConnectionString()
        {
            return configuration.GetConnectionString("DeafaultConnection");
        }
    }
}
