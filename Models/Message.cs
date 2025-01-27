namespace Models{

    public class Message{
        public int MessageId {get; set; }
        public int ChatId {get; set; }
        public string Role {get; set; }
        public string Text {get; set; }
        public DateTime Time {get; set; }

    }
}