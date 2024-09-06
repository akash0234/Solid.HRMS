using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
namespace Solid.Auth
{
    public static class HashingUtility
    {
        

        // Generate a random salt
        public static PasswordHashModel GenerateHashPassword(string password, byte[] salt = null, bool needsOnlyHash = false)
        {
            if (salt == null || salt.Length != 16)
            {
                // generate a 128-bit salt using a secure PRNG
                salt = new byte[128 / 8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            if (needsOnlyHash) return new PasswordHashModel() { PasswordHash = hashed};
            Console.WriteLine($"salt: {Convert.ToBase64String(salt)}");
            Console.WriteLine($"hashed: {hashed}");
            // password will be concatenated with salt using ':'
            return new PasswordHashModel() { PasswordHash = hashed,PasswordSalt = Convert.ToBase64String(salt) };
        }

        public static bool VerifyPassword(PasswordHashModel hashedPasswordWithSalt, string passwordToCheck)
        {
            // retrieve both salt and password from 'hashedPasswordWithSalt'
           
            
            var salt = Convert.FromBase64String(hashedPasswordWithSalt.PasswordSalt);
            if (salt == null)
                return false;
            // hash the given password
            var hashOfpasswordToCheck = GenerateHashPassword(passwordToCheck, salt, true);
            // compare both hashes
            return String.Compare(hashedPasswordWithSalt.PasswordHash, hashOfpasswordToCheck.PasswordHash) == 0;
        }
    }
    public class PasswordHashModel
    {
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }
}
