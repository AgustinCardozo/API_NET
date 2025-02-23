namespace API_Demo.Helpers
{
    public static class Consts
    {
        public const string ADMIN = "ADMIN";
        public const string CREATE = "CREATE";
        public const string DATABASE = "DatabaseName";
        public const string ERROR_MSG = "ERROR";
        public const string PATH = "Assets/10k-most-common-passwords.txt";

        public static class StartupConfig
        {
            public const string ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
            public const string JWT_KEY = "JWT:key";
        }

        public static class HttpMethods
        {
            public const string POST = "POST";
            public const string PUT = "PUT";
        }

        public static class ConfigKeys
        {
            public const string CONN_DB = "DatabaseConnection";
            public static class URL
            {
                public const string COTIZADOR = "URL:cotizador";
                public const string DOLAR_BLUE = "URL:dolar_blue";
                public const string DOLAR_OFICIAL = "URL:dolar_oficial";
            }
        }

        public static class StoreProcName
        {
            public const string CreateCliente = "SYS_CreateCliente";
            public const string DeleteCliente = "SYS_DeleteCliente";
            public const string GetClientes = "SYS_GetClientes";
            public const string UpdateCliente = "SYS_UpdateCliente";
        }

        public static class Version
        {
            public const string V1 = "v1";
            public const string V2 = "v2";
        }
    }
}
