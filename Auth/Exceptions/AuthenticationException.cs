namespace Auth.Exceptions
{
    public class AutheticationException : Exception
    {
        public AutheticationException() : base("An error occurred in the project.")
        {
        }

        public AutheticationException(string message) : base(message)
        {
        }

        public AutheticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
