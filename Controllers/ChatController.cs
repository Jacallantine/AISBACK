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

    public class ChatController : ControllerBase
    {
        
        [HttpPost("NewChat")]
        public async void NewChat(Chat newChat)
        {
            Database myDatabase = new();
            myDatabase.PostChatsBridge(newChat);
        }


        [HttpPost("GetChats")]
        public async Task<List<ChatList>> GetChats(ChatDto allChats)
        {
            Database myDatabase = new();
            var chats = await myDatabase.GetChatsBridge(allChats);
            return chats;
        }
    }
}