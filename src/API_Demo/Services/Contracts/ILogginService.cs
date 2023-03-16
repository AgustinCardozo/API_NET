namespace API_Demo.Services.Contracts
{
    public interface ILogginService
    {
        string RegistrarUsuario(RegistrarUsuarioReq usuario);
        string IniciarSeccion(UsuarioLogginReq usuario);
        void RestaurarPassword(string username, string nuevoPass);
    }
}
