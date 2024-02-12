using API_DEMO_XTest.Helpers;

namespace API_Demo_XTest.Helpers
{
    public class CommonHelper<T> where T : class
    {
        public T service { get; }
        public ServiceProviderHelper serviceHelper { get; }

        public CommonHelper()
        {
            serviceHelper = new();
            service = serviceHelper.GetRequiredService<T>();
        }
    }
}
