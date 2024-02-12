using API_Demo.Helpers.Exceptions;
using System.Runtime.Serialization;

namespace API_Demo_XTest.Helpers
{
    public abstract class MyExceptionHelper<TException, TOtherException> where TException : Exception where TOtherException : RetryActionException
    {
        protected SerializationInfo serializationInfo { get; set; }

        [Fact]
        public void TestClassException()
        {
            var exceptionType = typeof(TException);
            Assert.Throws(exceptionType, () =>
            {
                throw (TException)Activator.CreateInstance(typeof(TException));
            });
        }

        [Fact]
        public void TestClassExceptionWithMessage()
        {
            var exceptionType = typeof(TException);
            var expectedMessage = "The operation is invalid.";
            var ex = (TException)Activator.CreateInstance(typeof(TException), expectedMessage);

            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public void TestClassExceptionWithMessageAndInnerException()
        {
            Exception innerException = new Exception();
            var expectedMessage = "The operation is invalid.";
            var ex = (TException)Activator.CreateInstance(typeof(TException), expectedMessage, innerException);

            Assert.NotNull(ex);
            Assert.Contains("System.Exception", ex.InnerException?.Message);
        }

        [Fact]
        public void TestClassExceptionGetObjectData()
        {
            var exception = (TException)Activator.CreateInstance(typeof(TException));
            this.serializationInfo = new SerializationInfo(typeof(TException), new FormatterConverter());
            var streamingContext = new StreamingContext();
            exception.GetObjectData(serializationInfo, streamingContext);

            Assert.NotNull(exception);
            Assert.IsType<TException>(exception);
        }

        [Fact]
        public void TestClassExceptionSerializationInfoConstructor()
        {
            this.serializationInfo = new SerializationInfo(typeof(TOtherException), new FormatterConverter());
            this.serializationInfo.AddValue("Message", "Test Message");

            var innerException = new Exception("Inner Exception Message");
            serializationInfo.AddValue("InnerException", innerException, typeof(Exception));

            TOtherException exception;
            try
            {
                exception = GetInstance();
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex);
            }
        }

        public abstract TOtherException GetInstance();
    }
}
