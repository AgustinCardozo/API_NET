namespace API_Demo.Models.Requests
{
    public class UsuarioLogginReq
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class ReestrablecerPassReq : UsuarioLogginReq
    {

    }
}
