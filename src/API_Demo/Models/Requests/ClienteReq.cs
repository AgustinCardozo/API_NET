using System.ComponentModel.DataAnnotations;

namespace API_Demo.Models.Requests
{
    public class ClienteReq
    {
        [Required(ErrorMessage = "Debe ingresar el id")]
        public string idCliente { get; set; }
        [Required(ErrorMessage = "Debe ingresar la razon social")]
        public string razonSocial { get; set; }
        public string telefono { get; set; }
        public string domicilio { get; set; }
        public float limite { get; set; }
        public int? idVendedor { get; set; }
    }
}
