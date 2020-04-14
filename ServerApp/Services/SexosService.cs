using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ServerApp.DAO;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Services
{


    public interface ISexosServices
    {
        List<Sexo> GetAll();
    }


    public class SexosServices : ISexosServices
    {
        IConfiguration _configuration;
        public SexosServices(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").
                GetSection("DefaultConnection").Value;
            return connection;
        }

      
        public List<Sexo> GetAll()
        {
            var connectionString = this.GetConnection();
            List<Sexo> sexos = new List<Sexo>();
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var query = "select * from Sexos;";
                    con.Open();
                    sexos = con.Query<Sexo>(query).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return sexos;
            }
        }

    }
}