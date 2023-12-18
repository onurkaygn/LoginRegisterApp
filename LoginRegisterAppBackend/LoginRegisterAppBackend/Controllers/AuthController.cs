using LoginRegisterAppBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LoginRegisterAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                connection.Open();
                connection.ExecuteAsync("SP_CreateUser", 
                    new {
                        Username = request.Username,
                        PasswordHash = passwordHash
                    },
                    commandType: CommandType.StoredProcedure);
                return Ok(true);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(UserDto request)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DbConnection")))
            {
                connection.Open();
                var user = await connection.QueryFirstOrDefaultAsync<User>("SP_GetUserByUsername",
                    new { Username = request.Username },
                    commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    return NotFound("User with the specified name was not found on the server.");
                }

                if (BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return Ok();
                }else
                {
                    return BadRequest("Wrong password.");
                }
            }
        }

    }
}
