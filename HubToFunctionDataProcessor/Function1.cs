using Azure.Messaging.EventHubs;
using HubToFunctionDataProcessor.Helper;
using HubToFunctionDataProcessor.Models;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace HubToFunctionDataProcessor
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("EventHubToFunctionTrigger")]
        public async Task<CosmosDbFunctionOutput> RunAsync(
             [EventHubTrigger("%EventHubSettings:EventHubName%", Connection = "EventHubSettings:ConnectionString", ConsumerGroup = "$Default")] EventData[] events)
        {
            var sensorDataList = new List<SensorDataOut>();
            foreach (var eventData in events)
            {
                Console.WriteLine($"EventData byte: {eventData}");
                var evNB = Encoding.UTF8.GetString(eventData.Body.ToArray());

                Console.WriteLine($"eventData: {evNB}");
                if (eventData != null)
                {
                    await Task.Run(() =>
                    {
                        var sensorData =  ProcessEvent(evNB);
                        sensorDataList.AddRange(sensorData.ToList());
                    });
                }
            }

            _logger.LogInformation("Finished processing events.");

            return new CosmosDbFunctionOutput
            {
                SensorData = sensorDataList
            };
        }


        private IEnumerable<SensorDataOut> ProcessEvent(string eventData)
        {

            using (StringReader stringReader = new StringReader(eventData))
            using (JsonTextReader jsonReader = new JsonTextReader(stringReader))
            {
                JsonSerializer serializer = new JsonSerializer();
                DeviceData deviceData = serializer.Deserialize<DeviceData>(jsonReader);
                if (deviceData?.Sensors == null)
                {
                    return Enumerable.Empty<SensorDataOut>();
                }

                var sensorData = deviceData.Sensors.AsParallel().Select(s =>
                {
                    return new SensorDataOut
                    {
                        id = s.SensorId + "-" + deviceData.TimeStamp.ToString(),
                        deviceId = deviceData.DeviceId,
                        timeStamp = deviceData.TimeStamp,
                        sensorId = s.SensorId,
                        sensorType = s.SensorType,
                        unit = s.Unit,
                        value = 10002.3434
                    };

                });
                return sensorData;
            }
        }
    }
}
