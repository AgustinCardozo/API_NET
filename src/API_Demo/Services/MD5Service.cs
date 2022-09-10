using System;
using System.Security.Cryptography;
using System.Text;

namespace API_Demo.Services
{
    public static class MD5Service
    {
        public static string Encrypt(string message, string hash)
        {
            var data = UTF8Encoding.UTF8.GetBytes(message);
            var md5 = MD5.Create();
            var tripleDES = TripleDES.Create();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            var transform = tripleDES.CreateEncryptor();
            var result = transform.TransformFinalBlock(data, 0, data.Length);

            return Convert.ToBase64String(result);
        }

        public static string Decrypt(string message, string hash)
        {
            var data = Convert.FromBase64String(message);
            var md5 = MD5.Create();
            var tripleDES = TripleDES.Create();

            tripleDES.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            tripleDES.Mode = CipherMode.ECB;

            var transform = tripleDES.CreateDecryptor();
            var result = transform.TransformFinalBlock(data, 0, data.Length);

            return UTF8Encoding.UTF8.GetString(result);
        }
    }
}
