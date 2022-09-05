using System.Security.Cryptography;
using System.Text;

namespace MyBankApi.Utilities
{
    public class Utility
    {
        /// <summary>
        /// This method is used to create the encoded pinSalt and pinHash
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="pinHash"></param>
        /// <param name="pinSalt"></param>
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