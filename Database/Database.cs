using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Text.Json;
using Models;
using Controllers;
// using JWT;

namespace DATABASE
{
     public class Database 
    {


        private string cs;

       public Database(){
        cs = "Server=xefi550t7t6tjn36.cbetxkdyhwsb.us-east-1.rds.amazonaws.com;Database=z93990s55wb3qpia;User=ky5r2mnipjgwso83;Password=zx6yrhytzsxzndb2;Port=3306;";

       }

   private async Task<List<ChatList>> GetChats(string sql, List<MySqlParameter> parms)
{
    List<ChatList> chats = new();
    using var connection = new MySqlConnection(cs);
    await connection.OpenAsync();
    using var command = new MySqlCommand(sql, connection);

    if (parms != null)
    {
        command.Parameters.AddRange(parms.ToArray());
    }

    using var reader = await command.ExecuteReaderAsync();

    ChatList currentChat = null;

    while (await reader.ReadAsync())
    {
        
        string chatId = reader.GetString(reader.GetOrdinal("chat_id"));

       
        if (currentChat == null || currentChat.ChatId != chatId)
        {
           
            currentChat = new ChatList
            {
                ChatId = chatId,
                AccountId = reader.GetString(reader.GetOrdinal("account_id")),
                Title = reader.GetString(reader.GetOrdinal("title")),
                Time = reader.GetDateTime(reader.GetOrdinal("datetime")),
                Messages = new List<Message>()
            };

            chats.Add(currentChat);
        }

     
        if (!reader.IsDBNull(reader.GetOrdinal("message_id")))
        {
            var message = new Message
            {
                MessageId = reader.GetString(reader.GetOrdinal("message_id")),
                ChatId = reader.GetString(reader.GetOrdinal("Chat_id")),
                Role = reader.GetString(reader.GetOrdinal("role")),
                Text = reader.GetString(reader.GetOrdinal("content")),
                Time = reader.GetDateTime(reader.GetOrdinal("datetime"))
            };

            currentChat.Messages.Add(message);
        }
    }

    return chats;
}


        private async void PostChats(string sql, List<MySqlParameter> parms)
        {
            using var connection = new MySqlConnection(cs);
            await connection.OpenAsync();
            using var command = new MySqlCommand(sql, connection);

            if(parms != null)
            {
                command.Parameters.AddRange(parms.ToArray());
            }
            await command.ExecuteNonQueryAsync();
    
        }

        private async Task<Dictionary<string,object>> VerifyLogin (string sql, List<MySqlParameter> parms)
        {
            Dictionary<string, object> LoginData = new();
            using var connection = new MySqlConnection(cs);
            await connection.OpenAsync();
            using var command = new MySqlCommand(sql, connection);


            if(parms != null)
            {
                command.Parameters.AddRange(parms.ToArray());
            }

            using var reader = command.ExecuteReader();
            if(await reader.ReadAsync())
            {
                LoginData.Add("School", reader.GetString("school"));
               LoginData.Add("AccountId", reader.GetString("account_id"));
               LoginData.Add("FirstName", reader.GetString("first_name"));
               LoginData.Add("Email", reader.GetString("email"));
                 Console.WriteLine(JsonSerializer.Serialize(LoginData, new JsonSerializerOptions { WriteIndented = true }));
                return LoginData;
            }
         
         return null;

           


        }
      
        private async Task<Dictionary<string,object>> PostAccount(string sql, List<MySqlParameter> parms)
        {
            Dictionary<string, object> AccountData = new();
            using var connection = new MySqlConnection(cs);
            await connection.OpenAsync();
            using var command = new MySqlCommand(sql, connection);

             if(parms != null)
            {
                command.Parameters.AddRange(parms.ToArray());
            }

            using var reader = command.ExecuteReader();
            if(await reader.ReadAsync())
            {
               AccountData.Add("AccountId", reader.GetString("account_id"));
               AccountData.Add("FirstName", reader.GetString("first_name"));
               AccountData.Add("Email", reader.GetString("email"));
               AccountData.Add("School", reader.GetString("school"));
            }

            return AccountData;
        }
     




     public async Task<Dictionary<string,object>> LoginBridge(Login newLogin)
      {
        string sql = "Select account_id, email, first_name, school from Accounts where email = @Email and password = @Password;";
        newLogin.Password = HashPassword(newLogin.Password);
        System.Console.WriteLine(newLogin.Password);
        
        List<MySqlParameter> parms = new()
        {
            new MySqlParameter("@Email", MySqlDbType.String) {Value = newLogin.Email},
            new MySqlParameter("@Password", MySqlDbType.String) {Value = newLogin.Password}
        };
       
        return await VerifyLogin(sql, parms);

    }

    public async Task<Dictionary<string,object>> PostAccountBridge(Account newAccount)
    {
       string sql = @"
        INSERT INTO Accounts (account_id, email, password, school, first_name, last_name)
        VALUES (@AccountId, @Email, @Password, @School, @FirstName, @LastName);";

        // newAccount.AccountId = Guid.NewGuid().ToString();
        newAccount.Password = HashPassword(newAccount.Password);

         List<MySqlParameter> parms = new()
        {
            new MySqlParameter("@AccountId", MySqlDbType.String) {Value = newAccount.AccountId},
            new MySqlParameter("@Email", MySqlDbType.String) {Value = newAccount.Email},
            new MySqlParameter("@Password", MySqlDbType.String) {Value = newAccount.Password},
            new MySqlParameter("@School", MySqlDbType.String) {Value = newAccount.School},
            new MySqlParameter("@FirstName", MySqlDbType.String) {Value = newAccount.FirstName},
            new MySqlParameter("@LastName", MySqlDbType.String) {Value = newAccount.LastName}
        };
        return await PostAccount(sql, parms);
    }

    public async void PostChatsBridge(Chat newChat)
    {
        string sql = "insert into Chats (account_id, chat_id, datetime, title) values (@AccountId, @ChatId, @Time, @Title);";
         List<MySqlParameter> parms = new()
        {
            new MySqlParameter("@AccountId", MySqlDbType.String) {Value = newChat.AccountId},
            new MySqlParameter("@ChatId", MySqlDbType.String) {Value = newChat.ChatId},
            new MySqlParameter("@Time", MySqlDbType.DateTime) {Value = newChat.Time.ToUniversalTime()},
            new MySqlParameter("@Title", MySqlDbType.String) {Value = newChat.Title}
        };
        PostChats(sql, parms);
    }


    public async Task<List<ChatList>> GetChatsBridge(ChatDto allChats)
    {
        string sql = @"
    SELECT 
        c.Chat_id, c.account_id, c.title, c.datetime, m.Chat_id, m.content,
        m.message_id, m.datetime, m.role
    FROM 
        Chats c
    LEFT JOIN 
        Messages m ON c.Chat_id = m.Chat_id
    WHERE 
        c.account_id = @AccountId
    ORDER BY 
        c.chat_id, m.datetime;";
         List<MySqlParameter> parms = new()
        {
            new MySqlParameter("@AccountId", MySqlDbType.String) {Value = allChats.AccountId}
        };

        return await GetChats(sql, parms);
    }

   private async Task<Dictionary<string, object>> PutAccount(string sql, List<MySqlParameter> parms)
{
    Dictionary<string, object> result = new();
    using var connection = new MySqlConnection(cs);
    await connection.OpenAsync();
    using var command = new MySqlCommand(sql, connection);

    if (parms != null)
    {
        command.Parameters.AddRange(parms.ToArray());
    }

    int rowsAffected = await command.ExecuteNonQueryAsync();

    if (rowsAffected > 0)
    {
        result.Add("success", true);
        result.Add("message", "Account updated successfully.");
    }
    else
    {
        result.Add("success", false);
        result.Add("message", "No rows updated.");
    }

    return result;
}




    public async Task<Dictionary<string, object>> PutAccountBridge(AccountDto UpdateAccount)
    {
         string sql = "UPDATE accounts SET first_name = @FirstName, school = @School, email = @Email WHERE account_id = @AccountId;";
     
              List<MySqlParameter> parms = new()
        {
            new MySqlParameter("@AccountId", MySqlDbType.String) {Value = UpdateAccount.AccountId},
            new MySqlParameter("@School", MySqlDbType.String) {Value = UpdateAccount.School},
            new MySqlParameter("@FistName", MySqlDbType.String) {Value = UpdateAccount.FirstName},
            new MySqlParameter("@Email", MySqlDbType.String) { Value = UpdateAccount.Email}
        };

        return await PutAccount(sql, parms);

    }

    private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower(); // Convert bytes to a lowercase hex string
        }
    public async void PostMessageBridge(Message newMessage){
        string sql = @"INSERT INTO Messages (message_id, Chat_id, role, content, datetime )
        VALUES (@messageId, @ChatId, @Role, @Text, @Time);";
     
              List<MySqlParameter> parms = new()
        {
            new MySqlParameter("@MessageId", MySqlDbType.String) {Value = newMessage.MessageId},
            new MySqlParameter("@ChatId", MySqlDbType.String) {Value = newMessage.ChatId},
            new MySqlParameter("@Role", MySqlDbType.String) {Value = newMessage.Role},
            new MySqlParameter("@Time", MySqlDbType.DateTime) { Value = newMessage.Time},
            new MySqlParameter("@Text", MySqlDbType.String) { Value = newMessage.Text},
        };

        PostMessage(sql, parms);
    }

    private async void PostMessage(string sql, List<MySqlParameter> parms){
            Dictionary<string, object> MessageData = new();
            using var connection = new MySqlConnection(cs);
            await connection.OpenAsync();
            using var command = new MySqlCommand(sql, connection);

             if(parms != null)
            {
                command.Parameters.AddRange(parms.ToArray());
            }

            using var reader = command.ExecuteReader();
            if(await reader.ReadAsync())
            {
               MessageData.Add("MessageId", reader.GetString("message_id"));
               MessageData.Add("ChatId", reader.GetString("Chat_id"));
               MessageData.Add("Role", reader.GetString("role"));
               MessageData.Add("Text", reader.GetString("content"));
               MessageData.Add("Time", reader.GetDateTime("datetime"));
            }
    }







}



}




  