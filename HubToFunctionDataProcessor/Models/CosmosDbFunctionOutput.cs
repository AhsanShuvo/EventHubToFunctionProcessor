using Microsoft.Azure.Functions.Worker;

namespace HubToFunctionDataProcessor.Models
{
    public class CosmosDbFunctionOutput
    {
        public CosmosDbFunctionOutput()
        {
            SensorData = new List<SensorDataOut>();
        }

        [CosmosDBOutput("%CosmosDB:DatabaseName%", "%CosmosDB:ContainerName%", Connection = "CosmosDB:ConnectionString", CreateIfNotExists = false)]
        public List<SensorDataOut> SensorData { get; set; }
    }
}
