namespace API_Demo.Helpers
{
    public static class Consts
    {
        public const string ADMIN = "ADMIN";
        public const string CREATE = "CREATE";

        public static class ConfigKeys
        {
            public const string CONN_DB = "DatabaseConnection";
            public static class URL
            {
                public const string DOLAR_OFICIAL = "URL:dolar_oficial";
                public const string DOLAR_BLUE = "URL:dolar_blue";
                public const string COTIZADOR = "URL:cotizador";
            }
        }

        public static class StoreProcName
        {
            public const string CreateCliente = "SYS_CreateCliente";
            public const string DeleteCliente = "SYS_DeleteCliente";
            public const string GetClientes = "SYS_GetClientes";
            public const string UpdateCliente = "SYS_UpdateCliente";
        }
    }
}
