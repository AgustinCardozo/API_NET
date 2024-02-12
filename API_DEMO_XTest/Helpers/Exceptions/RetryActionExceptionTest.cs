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

    public class RetryActionExceptionTest : MyExceptionHelper<RetryActionException, RetryException>
    {
        public override RetryException GetInstance()
        {
            return new RetryException(this.serializationInfo, new StreamingContext());
        }
    }
}
