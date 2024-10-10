namespace API_Demo.Configurations
{
    public class ApiDemoOptions
    {
        public const string Key = "API_DEMO";
        public ApiOptions Api { get; set; }
        public ConnStrOptions ConnectionStrings { get; set; }
        public string Hash { get; set; }
    }

    public class ApiOptions
    {
        public UrlOptions URL { get; set; }
        public int MaxRetries { get; set; } = 1;
    }
}