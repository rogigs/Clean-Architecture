using System.Text;
using Clean_Architecture.Application.Exceptions;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using JsonNet = Newtonsoft.Json;
using Newtonsoft.Json;
using SystemTextJson = System.Text.Json;

namespace Clean_Architecture.Infrastructure.MessageQueue
{
    public record Message
    {
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }

    public class RabbitMQConsumerService : BackgroundService, IDisposable
    {
        private IChannel? _channel;
        private IConnection? _connection;
        private readonly ConnectionFactory _factory = new();
        private readonly IServiceProvider _serviceProvider;

        public Task Initialization { get; private set; }

        public IChannel Channel
        {
            get
            {
                bool isChannelOpen = _channel != null && _channel.IsOpen;
                return isChannelOpen
                    ? _channel!
                    : throw new InvalidOperationException(
                        "The channel has not been initialized or is closed."
                    );
            }
        }

        public IConnection Connection
        {
            get
            {
                return IsConnectionOpen()
                    ? _connection!
                    : throw new InvalidOperationException(
                        "The connection has not been initialized or is closed."
                    );
            }
        }

        public RabbitMQConsumerService(IServiceProvider serviceProvider)
        {
            Initialization = InitializeAsync();
            _serviceProvider = serviceProvider;
        }

        public async Task InitializeAsync()
        {
            if (!IsConnectionOpen())
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }

        private async Task ProcessMessagesAsync(
            IUpdateProject updateProject,
            CancellationToken stoppingToken,
            Message message
        )
        {
            ProjectUpdateDTO projectUpdateDTO = new(
                Name: null,
                Description: null,
                EndDate: null,
                UsersId: [message.UserId]
            );

            await updateProject.ExecuteAsync(projectUpdateDTO, message.ProjectId);
        }

        private async Task ProcessMessagesWithRetryAsync(
            IUpdateProject updateProject,
            CancellationToken stoppingToken,
            Message message,
            int maxRetries = 0
        )
        {
            try
            {
                await ProcessMessagesAsync(updateProject, stoppingToken, message);
            }
            catch (ProjectException ex)
            {
                if (maxRetries >= 3)
                {
                    await sendMessageToDLQAsync(message, ex);
                    return;
                }


                maxRetries++;

                await ProcessMessagesWithRetryAsync(
                    updateProject,
                    stoppingToken,
                    message,
                    maxRetries
                );
            }
        }

        private static async Task sendMessageToDLQAsync(Message message, ProjectException exception)
        {
            try
            {

                RabbitMQConnection rabbitMqConnection = RabbitMQConnection.Instance;
                await rabbitMqConnection.Initialization;
                await rabbitMqConnection.SendMessageAsync("sendUserIdToProjectDLQ", SystemTextJson.JsonSerializer.Serialize(new { message, exception }));
                await rabbitMqConnection.CloseAsync();
            }
            catch (Exception ex)
            {
                // TODO: add Observability hire
                Console.WriteLine(ex);

            }
        }

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            await Initialization;

            AsyncEventingBasicConsumer consumer = new(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);

                JsonNet.JsonSerializerSettings settings = new()
                {
                    NullValueHandling = JsonNet.NullValueHandling.Ignore,
                    MissingMemberHandling = JsonNet.MissingMemberHandling.Ignore,
                };

                Message message = JsonNet.JsonConvert.DeserializeObject<Message>(jsonString, settings);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var updateProject = scope.ServiceProvider.GetRequiredService<IUpdateProject>();

                    await ProcessMessagesWithRetryAsync(updateProject, stoppingToken, message);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await _channel.BasicConsumeAsync(
                queue: "sendUserIdToProject",
                autoAck: false,
                consumer: consumer
            );

            return Task.CompletedTask;
        }

        public async Task CloseAsync()
        {
            if (IsConnectionOpen())
            {
                await _channel!.CloseAsync();
                await _connection!.CloseAsync();
                base.Dispose();
            }
        }

        private bool IsConnectionOpen() => _connection != null && _connection.IsOpen;
    }
}
