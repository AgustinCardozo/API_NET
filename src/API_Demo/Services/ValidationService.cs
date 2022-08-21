using API_Demo.Exceptions;
using System.IO;
using System.Linq;

namespace API_Demo.Services
{
    public class ValidationService
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
    }
}
