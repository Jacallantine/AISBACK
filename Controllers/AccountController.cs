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

    public class AccountController : ControllerBase
    {
        [HttpPost("NewAccount")]
        public async Task<Dictionary<string,object>> NewAccount(Account newAccount)
        {
            Database myDatabase = new();
            var account = await myDatabase.PostAccountBridge(newAccount);
            return account;

        }

        [HttpPut("UpdateAccount")]
        public async Task<Dictionary<string, object>> UpdateAccount(AccountDto updateAccount)
        {
            Database myDatabase = new();
            var account = await myDatabase.PutAccountBridge(updateAccount);
            return account;
        }
    }
}