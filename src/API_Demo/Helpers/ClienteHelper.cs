using API_Demo.Models.Responses;
using System.Collections.Generic;

namespace API_Demo.Helpers
{
    public static class ClienteHelper
    {
        public static bool EsValidoMensaje(string msg)
        {
            const string errorMsg = "ERROR";
            return !string.IsNullOrEmpty(msg) && msg != errorMsg;
        }

        public static void QuitarEspacio(List<ClienteRes> clientes)
        {
            foreach (var cliente in clientes)
            {
                cliente.clie_codigo = cliente.clie_codigo.Trim();
                cliente.clie_razon_social = cliente.clie_razon_social.Trim();
                cliente.clie_domicilio = cliente.clie_domicilio.Trim();
                cliente.clie_telefono = cliente.clie_telefono != null ? cliente.clie_telefono.Trim() : cliente.clie_telefono;
            }
        }
    }
}
