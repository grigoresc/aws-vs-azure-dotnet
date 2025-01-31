using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

namespace AWSLambda2
{
    public class QueueService
    {
        private static readonly AmazonSQSClient sqsClient = new AmazonSQSClient();
        //private const string QueueUrl = "https://sqs.<region>.amazonaws.com/<account-id>/<queue-name>";
        private const string QueueUrl = "http://sqs.us-east-1.localhost.localstack.cloud:4566/000000000000/MyQueue";//todo - configurable

        public async Task<string> SendMessageAsync(string message)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = QueueUrl,
                MessageBody = JsonConvert.SerializeObject(message)
            };

            var response = await sqsClient.SendMessageAsync(sendMessageRequest);
            return response.MessageId;
        }
    }
}
