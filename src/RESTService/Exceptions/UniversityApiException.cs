using System;

namespace RESTService.Exceptions
{
    public class UniversityApiException : Exception
    {
        public UniversityApiException()
        {
        }

        public UniversityApiException(string message) : base(message)
        {
        }

        public UniversityApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}