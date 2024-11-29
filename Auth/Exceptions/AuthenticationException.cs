namespace Auth.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base("An error occurred in the project.")
        {
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
