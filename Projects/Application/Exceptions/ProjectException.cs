using System;

namespace Projects.Application.Exceptions
{
    public class ProjectException : Exception
    {
        public ProjectException() : base("An error occurred in the project.")
        {
        }

        public ProjectException(string message) : base(message)
        {
        }

        public ProjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
