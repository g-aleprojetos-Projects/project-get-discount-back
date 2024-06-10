using System.Security.Cryptography;
using System.Text;

namespace project_get_discount_back.Helpers
{
    public class Cryptography
    {
        public string Encrypt(string valor)
        {
            if (string.IsNullOrEmpty(valor))
            {
                throw new ArgumentException("O valor não pode ser nulo ou vazio");
            }

            try
            {
                using (var sha256 = SHA256.Create())
                {
                    var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(valor));
                    var sb = new StringBuilder();
                    foreach (var b in hashedBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
