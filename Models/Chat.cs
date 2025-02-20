using Models;
namespace Models
{
    public class Chat
    {
        public string ChatId { get; set;}
        public string AccountId { get; set;}
        public DateTime Time { get; set; }
        public string Title { get; set; }
    }

    public class ChatList
    {
        public string ChatId { get; set;}
        public string AccountId { get; set;}
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}