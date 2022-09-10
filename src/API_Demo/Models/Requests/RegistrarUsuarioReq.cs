namespace API_Demo.Models.Requests
{
    public class RegistrarUsuarioReq
    {
        public string usuario { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
        public string nombre { get; set; }
        public bool esAdmin { get; set; }
    }
}
