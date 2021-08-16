using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using No_EF_Api.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace No_EF_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NoEnFrApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public NoEnFrApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult addUser(Users user)
        {

            SqlConnection sqlConnection = null;
            try
            {

                sqlConnection = new SqlConnection(_configuration.GetConnectionString("testeApi"));
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("addUser", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("Nome", user.Nome);
                cmd.Parameters.AddWithValue("Obs", user.Obs);

                cmd.ExecuteNonQuery();

                return Ok("Adicionado com Sucesso");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }
               
            }
        }

        [HttpGet]
        public IActionResult index()
        {
            SqlConnection sqlConnection = null;
            List<Users> users = new List<Users>();
            Users user;
            try
            {
                sqlConnection = new SqlConnection(_configuration.GetConnectionString("testeApi"));
                sqlConnection.Open();
                SqlCommand cmd = new SqlCommand("getAllUsers", sqlConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user = new Users()
                        {
                            UserId = reader.GetInt32("UserId"),
                            Nome = reader.GetString("Nome"),
                            Obs = reader.GetString("Obs")
                        };
                            
                        users.Add(user);
                    }
                }

                return Ok(users);



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (sqlConnection != null)
                {
                    sqlConnection.Close();
                    sqlConnection.Dispose();
                }

            }
        }
    }
}
