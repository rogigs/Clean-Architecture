using System.Text;
using RabbitMQ.Client;
using Users.Domain.Enums;
using Users.Domain.Interfaces;
using Users.Infrastructure.Exceptions;

namespace Users.Infrastructure
{
    public sealed class RabbitMQConnection : IRabbitMQConnection 
    {
        private IChannel? _channel;
        private IConnection? _connection;
        private readonly ConnectionFactory _factory = new();

        private static readonly Lazy<RabbitMQConnection> _instance = new(
            () => new RabbitMQConnection()
        );

        public static RabbitMQConnection Instance => _instance.Value;
        public Task<(RabbitMQException?, string)> Initialization { get; private set; }

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

        public RabbitMQConnection()
        {
            Initialization = InitializeAsyncRetryAsync();
        }

        public async Task InitializeAsync()
        {
            if (!IsConnectionOpen())
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
            }
        }

        public async Task<(RabbitMQException?, string)> InitializeAsyncRetryAsync(
            int maxRetries = 0
        )
        {
            try
            {
                await InitializeAsync();

                return (null, Status.Success.ToString());
            }
            catch (RabbitMQException ex)
            {
                if (maxRetries >= 3)
                    return (
                        new RabbitMQException("Failed to create connection with RabbitMQ.", ex),
                        Status.Error.ToString()
                    );

                maxRetries++;

                return await InitializeAsyncRetryAsync(maxRetries);
            }
        }

        public async Task<(RabbitMQException?, string)?> CloseAsync()
        {
            try
            {
                if (IsConnectionOpen())
                {
                    await _channel!.CloseAsync();
                    await _connection!.CloseAsync();
                }

                return (null, Status.Success.ToString());
            }
            catch (Exception ex)
            {
                return (
                    new RabbitMQException("Failed to close connection with RabbitMQ.", ex),
                    Status.Error.ToString()
                );
            }
        }

        public async Task SendMessageAsync(string queueName, string message)
        {
            await Channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            byte[] body = Encoding.UTF8.GetBytes(message);

            BasicProperties props = new();

            await Channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: true,
                basicProperties: props,
                body: body
            );
        }

        private bool IsConnectionOpen() => _connection != null && _connection.IsOpen;
    }
}
