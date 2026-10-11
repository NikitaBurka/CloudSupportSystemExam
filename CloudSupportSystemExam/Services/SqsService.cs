using Amazon.SQS;
using Amazon.SQS.Model;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace CloudSupportSystem.Services
{
    public interface ISqsService
    {
        Task SendMessageAsync(int id, string subject, string priority);
    }

    public class SqsService : ISqsService
    {
        private readonly IAmazonSQS _sqsClient;
        private readonly IConfiguration _configuration;

        public SqsService(IConfiguration configuration)
        {
            _configuration = configuration;

           
            var accessKey = _configuration["AWS:AccessKey"];
            var secretKey = _configuration["AWS:SecretKey"];
            var region = Amazon.RegionEndpoint.GetBySystemName(_configuration["AWS:Region"]);

           
            _sqsClient = new AmazonSQSClient(accessKey, secretKey, region);
        }

        public async Task SendMessageAsync(int id, string subject, string priority)
        {
            var queueUrl = _configuration["AWS:SqsQueueUrl"];
            var messagePayload = new
            {
                Id = id,
                Subject = subject,
                Priority = priority,
                Timestamp = DateTime.UtcNow
            };

            var request = new SendMessageRequest
            {
                QueueUrl = queueUrl,
                MessageBody = JsonSerializer.Serialize(messagePayload)
            };

            await _sqsClient.SendMessageAsync(request);
        }
    }
}