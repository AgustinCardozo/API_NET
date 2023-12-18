using API_Demo.Helpers.Exceptions;

namespace API_Demo_XTest.Helpers.Exceptions;

public class UsuarioInvalidoExceptionTest
{
    private readonly string expectedMessage;

    public UsuarioInvalidoExceptionTest()
    {
        this.expectedMessage = "The operation is invalid.";
    }

    [Fact]
    public void UsuarioInvalidoException_Test()
    {
        var exceptionType = typeof(UsuarioInvalidoException);
        Assert.Throws(exceptionType, () =>
        {
            throw new UsuarioInvalidoException();
        });
    }

    [Fact]
    public void UsuarioInvalidoException_ErrorMessage_Test()
    {
        var exceptionType = typeof(UsuarioInvalidoException);
        var ex = new UsuarioInvalidoException(expectedMessage);

        Assert.NotNull(ex);
        Assert.IsType(exceptionType, ex);
        Assert.Equal(expectedMessage, ex.Message);
    }

    [Fact]
    public void UsuarioInvalidoException_ErrorMessageWithInnerException_Test()
    {
        Exception innerException = new Exception();
        var ex = new UsuarioInvalidoException(expectedMessage, innerException);

        Assert.NotNull(ex);
        Assert.Contains("System.Exception", ex.InnerException?.Message);
    }
}
