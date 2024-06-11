using System.Security.Cryptography;
using System.Text; 

namespace ObligatorioProgram3.Recursos
{
    public class Utilidades
    {
        public static string encriptarClave(string contrasena)
        {
            StringBuilder stringBuilder = new StringBuilder();
            using (SHA256 sha256 = SHA256Managed.Create()) {
                Encoding enc = Encoding.UTF8;

                byte[] result = sha256.ComputeHash(enc.GetBytes(contrasena));

                foreach (byte b in result)
                {
                    stringBuilder.Append(b.ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
