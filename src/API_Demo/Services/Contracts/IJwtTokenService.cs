namespace API_Demo.Services.Contracts
{
    public interface IJwtTokenService
    {
        public string Authenticate(UsuarioRes user);
    }
}
