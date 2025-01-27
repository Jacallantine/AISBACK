using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient; 
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using DATABASE;
using Models;



namespace Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]

    public class LoginController : ControllerBase
    {

        [HttpPost("Login")]
        public async Task<Dictionary<string,object>> Login(Login newLogin)
        {

            Database myDatabase = new();
            var account = await myDatabase.LoginBridge(newLogin);
            return account;
        }






    }

}