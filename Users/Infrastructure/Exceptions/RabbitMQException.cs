namespace Users.Infrastructure.Exceptions
{
    public class RabbitMQException : Exception
    {
        public RabbitMQException() : base("An error occurred in the user.")
        {
        }

        public RabbitMQException(string message) : base(message)
        {
        }

        public RabbitMQException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
