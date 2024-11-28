using System.Text;
using RabbitMQ.Client;
using Users.Domain.Interfaces;

namespace Users.Infrastructure
{
    public sealed class RabbitMQConnection : IAsyncInitialization
    {
        private static readonly Lazy<RabbitMQConnection> _instance =
            new(() => new RabbitMQConnection());
        private IChannel? _channel;
        private IConnection? _connection;
        //TODO: add ENVIRONMENT VARIABLES
        private readonly ConnectionFactory _factory = new();

        public static RabbitMQConnection Instance => _instance.Value;

        public IConnection Connection
        {
            get
            {
                return IsConnectionOpen() ? _connection! : throw new InvalidOperationException("The connection has not been initialized or is closed.");
            }
        }

        private bool IsConnectionOpen() => _connection != null && _connection.IsOpen;

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
            if (_channel == null) throw new InvalidOperationException("The channel has not been initialized.");
            

            // Configura a fila (deve ser idempotente)
            await _channel.QueueDeclareAsync(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            // Converte a mensagem em bytes e envia para a fila
            byte[] body = Encoding.UTF8.GetBytes(message);

            BasicProperties props = new();

            await _channel.BasicPublishAsync(exchange: "",
                                  routingKey: queueName,
                                  mandatory: true,
                                  basicProperties: props,
                                  body: body);
        }

    }
}
