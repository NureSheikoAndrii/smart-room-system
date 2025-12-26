namespace SmartRoom.TestClient.Models
{
    public class AdminLog
    {
        public int id { get; set; }
        public string message { get; set; } = "";
        public DateTime createdAt { get; set; }
    }
}
