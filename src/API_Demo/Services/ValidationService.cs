using System.IO;

namespace API_Demo.Services
{
    public static class ValidationService
    {
        public static void ValidacionDeSeguridad(string password)
        {
            if (!(password.Length >= 8 && password.Length <= 64))
                throw new PasswordInvalidoException("Contraseña invalida");
            FileStream fileStream = new FileStream("Assets/10k-most-common-passwords.txt", FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = string.Empty;
                //line = reader.ReadLine() -> lee linea por linea
                while ((line = reader.ReadLine()) != null)
                {
                    if (password.Equals(line))
                        throw new PasswordInvalidoException("Contraseña invalida");
                }
            }
        }

        public static void ValidacionDeUsuarioYPassword(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new UsuarioInvalidoException("Nombre de Usuario o Contraseña incorrectos.");
            }
        }
    }
}
