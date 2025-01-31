using Azure.Storage.Queues;

namespace AzureFunctionApp2.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService()
        {
            string connectionString = "UseDevelopmentStorage=true";//todo - configurable
            string queueName = "myqueue";
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public void SendMessage(string message)
        {
            _queueClient.SendMessage(message);
        }
    }
}
