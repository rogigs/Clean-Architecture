using System.Text;
using RabbitMQ.Client;
using Users.Domain.Interfaces;

namespace Users.Infrastructure
{
    public sealed class RabbitMQConnection : IAsyncInitialization
    {
        private IChannel? _channel;
        private IConnection? _connection;
        private readonly ConnectionFactory _factory = new();

        private static readonly Lazy<RabbitMQConnection> _instance =
            new(() => new RabbitMQConnection());

        public static RabbitMQConnection Instance => _instance.Value;

        public Task Initialization { get; private set; }

        public IChannel Channel
        {
            get
            {
                bool isChannelOpen = _channel != null && _channel.IsOpen;
                return isChannelOpen ? _channel! : throw new InvalidOperationException("The channel has not been initialized or is closed.");
            }
        }

        public IConnection Connection
        {
            get
            {
                return IsConnectionOpen() ? _connection! : throw new InvalidOperationException("The connection has not been initialized or is closed.");
            }
        }

        private RabbitMQConnection()
        {
            Initialization = InitializeAsync();
        }

        public async Task InitializeAsync()
        {
            if (!IsConnectionOpen())
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }

        public async Task CloseAsync()
        {
            if (IsConnectionOpen())
            {
                await _channel!.CloseAsync();
                await _connection!.CloseAsync();
            }
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            await Channel.QueueDeclareAsync(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            byte[] body = Encoding.UTF8.GetBytes(message);

            BasicProperties props = new();

            await Channel.BasicPublishAsync(exchange: "",
                                  routingKey: queueName,
                                  mandatory: true,
                                  basicProperties: props,
                                  body: body);
        }

        private bool IsConnectionOpen() => _connection != null && _connection.IsOpen;
    }
}
