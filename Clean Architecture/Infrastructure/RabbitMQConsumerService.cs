using System.Text;
using Clean_Architecture.Application.UseCases.DTO;
using Clean_Architecture.Domain.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Clean_Architecture.Infrastructure
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

        protected override async Task<Task> ExecuteAsync(CancellationToken stoppingToken)
        {
            await Initialization;

            AsyncEventingBasicConsumer consumer = new(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var jsonString = Encoding.UTF8.GetString(body);

                JsonSerializerSettings settings = new()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                };

                Message message = JsonConvert.DeserializeObject<Message>(jsonString, settings);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var updateProject = scope.ServiceProvider.GetRequiredService<IUpdateProject>();

                    await ProcessMessagesAsync(updateProject, stoppingToken, message);
                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            _channel.BasicConsumeAsync(
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
