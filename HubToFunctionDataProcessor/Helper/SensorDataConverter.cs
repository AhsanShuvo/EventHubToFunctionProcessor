using HubToFunctionDataProcessor.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace HubToFunctionDataProcessor.Helper
{
    public class SensorDataConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType == typeof(SensorData);
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            var sensorData = new SensorData
            {
                SensorId = (string)jsonObject["SensorId"],
                SensorType = (string)jsonObject["SensorType"],
                Unit = (string)jsonObject["Unit"],
                LocationTimeStamp = (long)jsonObject["LocationTimeStamp"]
            };

            // Deserialize the Value property based on the SensorType
            switch (sensorData.SensorType)
            {
                case "Temperature":
                    sensorData.Value = jsonObject["Value"].ToObject<double>();
                    break;
                case "Humidity":
                    sensorData.Value = jsonObject["Value"].ToObject<int>();
                    break;
                case "Light":
                    sensorData.Value = jsonObject["Value"].ToObject<double>();
                    break;
                case "AirPressure":
                    sensorData.Value = jsonObject["Value"].ToObject<float>();
                    break;
                case "Shock":
                    sensorData.Value = jsonObject["Shock"].ToObject<double>();
                    break;
                default:
                    var tiltSensorObj = jsonObject["Value"].ToObject<JObject>();
                    sensorData.Value = tiltSensorObj.ToObject<TiltSensorValue>();
                    // Default to raw JSON token
                    break;
            }

            return sensorData;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Custom serialization logic if needed (or you can skip it)
            var sensorData = (SensorData)value;
            JObject jsonObject = new JObject
        {
            { "SensorId", sensorData.SensorId },
            { "SensorType", sensorData.SensorType }
        };

            if (sensorData.Value != null)
            {
                jsonObject.Add("Value", JToken.FromObject(sensorData.Value));
            }

            jsonObject.WriteTo(writer);
        }
    }
}
