using System;

namespace API_Demo.Helpers.Exceptions
{
    public class UsuarioInvalidoException : Exception
    {
        public UsuarioInvalidoException() : base() { }
        public UsuarioInvalidoException(string message) : base(message) { }
        public UsuarioInvalidoException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class PasswordInvalidoException : Exception
    {
        public PasswordInvalidoException() : base() { }
        public PasswordInvalidoException(string message) : base(message) { }
        public PasswordInvalidoException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class LogginInvalidoException : Exception
    {
        public LogginInvalidoException() : base() { }
        public LogginInvalidoException(string message) : base(message) { }
        public LogginInvalidoException(string message, Exception innerException) : base(message, innerException) { }
    }

    public static class ErrorMessage
    {
        public static string GetException(Exception ex)
        {
            return $"Error: {ex.Message} {(ex.InnerException != null ? $" - InnerException: " + ex.InnerException.Message : "")} - StackTrace: {ex.StackTrace}";
        }
    }
}
