using System.Text;

namespace Dictum.Data
{
    internal static class HashGenerator
    {
        public static string Sha256(string text)
        {
            using (var sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));
                var builder = new StringBuilder();

                foreach (var t in bytes) builder.Append(t.ToString("x2"));

                return builder.ToString();
            }
        }
    }
}