using System;
using System.Security.Cryptography;
using System.Text;

namespace MyBankApi.Validations
{
    public class Validation
    {
        public static bool VerifyPinHash(string pin, byte[] pinHash, byte[] pinSalt)
        {
            if (string.IsNullOrEmpty(pin)) throw new ArgumentNullException(nameof(pin));

            // pin verification
            using (var hmac = new HMACSHA512(pinSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != pinHash[i]) return false;
                }
            }
            return true;
        }
    }
}
