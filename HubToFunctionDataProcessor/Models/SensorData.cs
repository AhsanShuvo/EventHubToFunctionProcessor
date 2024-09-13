namespace HubToFunctionDataProcessor.Models
{
    public class SensorDataOut
    {
        public string id { get; set; }
        public string deviceId { get; set; }
        public long timeStamp { get; set; }
        public string sensorId { get; set; }
        public string sensorType { get; set; }
        public string unit { get; set; }
        public double value { get; set; }
    }
}
