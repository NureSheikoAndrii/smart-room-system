namespace SmartRoomBackend.Models
{
    public class SensorData
    {
        public int Id { get; set; }
        public double Temperature { get; set; }   // Температура
        public double Humidity { get; set; }      // Вологість
        public int LightLevel { get; set; }       // Показник світла (вкл\викл)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
