using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Common;

namespace API_Demo.Helpers
{
    public class PasswordHelper
    {
        public static string HideConnectionString(string connectionString)
        {
            var builder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            if (builder.ContainsKey("password"))
            {
                builder["password"] = "*****";
            }
            return builder.ToString();
        }

        public static string HideUserPassword(UsuarioRes user)
        {
            string requestBody = JsonConvert.SerializeObject(user, Formatting.Indented);
            JObject requestJson = JObject.Parse(requestBody);

            if (requestJson["password"] != null)
            {
                requestJson["password"] = "xxx";
            }

            requestBody = requestJson.ToString(Formatting.Indented);

            return requestBody;
        }
    }
}
