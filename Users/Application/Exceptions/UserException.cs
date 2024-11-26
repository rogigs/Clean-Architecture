namespace Users.Application.Exceptions
{
    public class UserException : Exception
    {
        public UserException() : base("An error occurred in the user.")
        {
        }

        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
