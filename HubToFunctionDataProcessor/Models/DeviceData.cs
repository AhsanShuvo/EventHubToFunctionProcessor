using Newtonsoft.Json;

namespace HubToFunctionDataProcessor.Models
{
    public  class DeviceData
    {
        public string DeviceId { get; set; }
        public long TimeStamp { get; set; }
        public Location Location { get; set; }
        public List<SensorData> Sensors { get; set; }
        public BatteryStatus BatteryStatus { get; set; }
        public Dictionary<string, string> AdditionalData { get; set; }

        public DeviceData(string deviceId, string deviceType, long timeStamp)
        {
            DeviceId = deviceId;
            this.TimeStamp = timeStamp;
            this.Sensors = new List<SensorData>();
            this.AdditionalData = new Dictionary<string, string>();

            this.Location = null;
            this.BatteryStatus = null;
        }

        public void AddAdditionalData(string key, string value)
        {
            AdditionalData[key] = value;
        }
    }

    public class Location
    {
        public required double Latitude { get; set; }
        public required double Longitude { get; set; }
        public string Name { get; set; }
        public int? Altitude { get; set; }
        public int? Accuracy { get; set; }
        public required long Timestamp { get; set; }

        // TODO:: Location obtaining method
    }

    public class SensorData
    {
        public string SensorId { get; set; }
        public string SensorType { get; set; }
        public string Unit { get; set; }
        public long? LocationTimeStamp { get; set; }
        public object Value { get; set; }
    }

    
    public class TiltSensorValue
    {
        public TiltSensorValue(int x, int y, int z)
        {
            this.X = x; this.Y = y; this.Z = z;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }


    public class BatteryStatus
    {
        public int Level { get; set; }

        public int SignalStrength { get; set; }
        public string DeviceStatus { get; set; }
    }
}
