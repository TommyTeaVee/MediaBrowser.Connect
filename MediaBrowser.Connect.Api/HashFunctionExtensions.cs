using System.Security.Cryptography;
using System.Text;

namespace MediaBrowser.Connect.Interfaces
{
    public static class HashFunctionExtensions
    {
        public static string Md5(this string input)
        {
            Encoder enc = Encoding.Unicode.GetEncoder();

            var rawBytes = new byte[input.Length*2];
            enc.GetBytes(input.ToCharArray(), 0, input.Length, rawBytes, 0, true);

            var md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(rawBytes);

            var sb = new StringBuilder();
            foreach (byte b in result) {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}