using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MoBot.Helpers
{
    internal static class HashHelper
    {
        public static string GetServerIdHash(string serverId, byte[] secretKey, byte[] publicKey)
        {
            var idBytes = Encoding.ASCII.GetBytes(serverId);
            var data = new byte[idBytes.Length + secretKey.Length + publicKey.Length];

            idBytes.CopyTo(data, 0);
            secretKey.CopyTo(data, idBytes.Length);
            publicKey.CopyTo(data, idBytes.Length + secretKey.Length);

            return JavaHexDigest(data);
        }

        private static string JavaHexDigest(byte[] data)
        {
            var sha1 = SHA1.Create();
            var hash = sha1.ComputeHash(data);
            var negative = (hash[0] & 0x80) == 0x80;
            if (negative) // check for negative hashes
                hash = TwosCompliment(hash);
            // Create the string and trim away the zeroes
            var digest = GetHexString(hash).TrimStart('0');
            if (negative)
                digest = "-" + digest;
            return digest;
        }

        private static string GetHexString(IEnumerable<byte> p)
        {
            return p.Aggregate(string.Empty, (current, t) => current + t.ToString("x2"));
        }

        private static byte[] TwosCompliment(byte[] p) // little endian
        {
            int i;
            var carry = true;
            for (i = p.Length - 1; i >= 0; i--)
            {
                p[i] = (byte)~p[i];
                if (!carry) continue;
                carry = p[i] == 0xFF;
                p[i]++;
            }
            return p;
        }
    }
}
