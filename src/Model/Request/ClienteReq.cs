namespace Model.Request
{
    public class ClienteReq
    {
        public string idCliente { get; set; }
        public string razonSocial { get; set; }
        public string telefono { get; set; }
        public string domicilio { get; set; }
        public float limite { get; set; }
        public int? idVendedor { get; set; }
    }
}
