using API_Demo.Helpers.Exceptions;
using System.Runtime.Serialization;

namespace API_Demo_XTest.Helpers.Exceptions
{
    public class RetryException : RetryActionException
    {
        public RetryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    public class RetryActionExceptionTest
    {
        [Fact]
        public void KafkaServiceException_Test()
        {
            var exceptionType = typeof(RetryActionException);
            Assert.Throws(exceptionType, () =>
            {
                throw new RetryActionException();
            });
        }

        [Fact]
        public void KafkaService_ErrorMessage_Test()
        {
            var exceptionType = typeof(RetryActionException);
            var expectedMessage = "The operation is invalid.";
            var ex = new RetryActionException(expectedMessage);

            Assert.NotNull(ex);
            Assert.IsType(exceptionType, ex);
            Assert.Equal(expectedMessage, ex.Message);
        }

        [Fact]
        public void KafkaService_ErrorMessageWithInnerException_Test()
        {
            Exception innerException = new Exception();
            var expectedMessage = "The operation is invalid.";
            var ex = new RetryActionException(expectedMessage, innerException);

            Assert.NotNull(ex);
            Assert.Contains("System.Exception", ex.InnerException?.Message);
        }

        [Fact]
        public void KafkaService_GetObjectData_Test()
        {
            var exception = new RetryActionException();
            var serializationInfo = new SerializationInfo(typeof(RetryActionException), new FormatterConverter());
            var streamingContext = new StreamingContext();
            exception.GetObjectData(serializationInfo, streamingContext);

            Assert.NotNull(exception);
            Assert.IsType<RetryActionException>(exception);
        }

        [Fact]
        public void KafkaService_SerializationInfoConstructor_Test()
        {
            var serializationInfo = new SerializationInfo(typeof(RetryException), new FormatterConverter());
            serializationInfo.AddValue("Message", "Test Message");

            var innerException = new Exception("Inner Exception Message");
            serializationInfo.AddValue("InnerException", innerException, typeof(Exception));

            RetryException exception;
            try
            {
                exception = new RetryException(serializationInfo, new StreamingContext());
            }
            catch (Exception ex)
            {

                Assert.NotNull(ex);
            }
        }
    }
}
