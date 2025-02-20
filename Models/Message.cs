namespace Models{

    public class Message{
        public string MessageId {get; set; }
        public string ChatId {get; set; }
        public string Role {get; set; }
        public string Text {get; set; }
        public DateTime Time {get; set; }

    }
}