using System.Security.Cryptography;
using System.Text;

namespace MyBankApi.Utilities
{
    public class Utility
    {
        public static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
        }
    }
}