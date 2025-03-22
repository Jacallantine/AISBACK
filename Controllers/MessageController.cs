using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using DATABASE;

namespace AISBACK.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        [HttpPost("NewMessage")]
        public async void NewMessage(Message newMessage)
        {
            Database myDatabase = new();
            System.Console.WriteLine(newMessage.ChatId);
            System.Console.WriteLine(newMessage.MessageId);
            System.Console.WriteLine(newMessage.Text);
            System.Console.WriteLine(newMessage.Time);
            myDatabase.PostMessageBridge(newMessage);
        }
    }
}