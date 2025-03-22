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
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RedirectLink { get; set; }
    }
}