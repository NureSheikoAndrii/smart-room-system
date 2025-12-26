namespace SmartRoom.IoT_Simulator
{

    //SENSOR 

    internal class SensorEmulator
    {
        private double _temperature = 25.0;
        private double _humidity = 45.0;
        private int _lightLevel = 120;

        public (double temperature, double humidity, int lightLevel) Read()
        {
            //температурний дрейф
            _temperature += Random.Shared.NextDouble() * 0.6 - 0.3;
            _temperature = Math.Clamp(_temperature, 20, 35);

            //вологість — плавні зміни
            _humidity += Random.Shared.NextDouble() * 2 - 1;
            _humidity = Math.Clamp(_humidity, 30, 70);

            //освітленість — шум
            _lightLevel += Random.Shared.Next(-10, 10);
            _lightLevel = Math.Clamp(_lightLevel, 0, 500);

            return (Math.Round(_temperature, 1), Math.Round(_humidity, 1), _lightLevel);
        }
    }
}



