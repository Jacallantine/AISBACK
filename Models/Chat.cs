using Models;
namespace Models
{
    public class Chat
    {
        public int ChatId { get; set;}
        public int AccountId { get; set;}
        public DateTime Time { get; set; }
        public string Title { get; set; }
    }

    public class ChatList
    {
        public int ChatId { get; set;}
        public int AccountId { get; set;}
        public DateTime Time { get; set; }
        public string Title { get; set; }
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}