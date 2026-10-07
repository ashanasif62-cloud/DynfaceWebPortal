using System;
using System.Security.Cryptography;

namespace AuthenticationHelper
{
    public class SecurePassword
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;

        #region Example01
        private string Hash(string _userId, string _password, int _iterations)
        {
            //create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            //create hash
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(_password, salt, _iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //combine salt and hash
            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            //convert to base64
            string base64Hash = Convert.ToBase64String(hashBytes);

            //format hash with extra information
            return string.Format("$({0})$vMs#56D@&*(6KGH$MaisonPortal${1}", _userId, base64Hash);
        }

        public static string Hash(string _userId, string _password)
        {
            string userId = _userId;
            string password = _password;
            string results;

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(password))
            {
                SecurePassword securePassword = new SecurePassword();
                results = securePassword.Hash(userId, password, 10000);
            }
            else
                results = "Required Data is Missing.";

            return results;
        }

        private bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$");
        }

        private bool Verify(string password, string hashedPassword)
        {
            //check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            //extract iteration and Base64 string
            var splittedHashString = hashedPassword.Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            //get hashbytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            //get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            //create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            //get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Example02 SHA-512
        public static string example02()
        {
            string message1 = "The quick brown fox jumps over the lazy dog";
            string results;

            var md5HashedMessage = ComputeHashCode(System.Text.Encoding.UTF8.GetBytes(message1));
            results = Convert.ToBase64String(md5HashedMessage);

            return results;
        }

        private static byte[] ComputeHashCode(byte[] toBeHashed)
        {
            using (var md5 = System.Security.Cryptography.SHA512.Create())//TripleDES
            {
                return md5.ComputeHash(toBeHashed);
            }
        }
        #endregion

        #region Example03 Cryptographic Hash-based Message Authentication Code (HMAC)
        public static string securePassword(string _userId, string _password)
        {
            //byte[] key = GenerateKey();
            byte[] key = new byte[] { 213, 232, 07, 11, 106, 45, 89, 79, 219, 45, 89, 79, 219, 213, 232, 240, 213, 232, 240, 255, 106, 45, 89, 79, 219 };
            string userId = _userId.ToLower();
            string password = _password;
            string results;

            password = string.Format("$({0})$vMs#56D@&*(6KGH$MaisonPortal${1}", userId, password);
            byte[] hmacMessage = ComputeHmacHash(System.Text.Encoding.UTF8.GetBytes(password), key);

            results = Convert.ToBase64String(hmacMessage);

            return results;
        }

        private static byte[] ComputeHmacHash(byte[] toBeHashed, byte[] key)
        {
            using (HMACSHA512 hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }

        private static byte[] GenerateKey()
        {
            const int KeySize = 32;

            using (RNGCryptoServiceProvider randomNumberGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[KeySize];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        #endregion

        #region MyRegion


        #endregion

    }
}