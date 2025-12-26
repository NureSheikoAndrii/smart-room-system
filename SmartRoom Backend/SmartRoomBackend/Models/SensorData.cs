namespace SmartRoomBackend.Models
{
    public class SensorData
    {
        public int Id { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public int LightLevel { get; set; }       //показник світла (вкл\викл)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
