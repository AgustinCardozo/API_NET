using System;

namespace API_Demo.Exceptions
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
}
