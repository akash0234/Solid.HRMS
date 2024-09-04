using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
namespace Solid.Auth
{
    public static class HashingUtility
    {
        private const int SaltSize = 16; // Size of the salt in bytes
        private const int HashSize = 32; // Size of the hash in bytes
        private const int Iterations = 4; // Number of iterations for Argon2

        // Generate a random salt
        public static byte[] GenerateSalt()
        {
            var saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            return saltBytes;
        }

        // Hash a password with the provided salt using Argon2
        public static byte[] HashPassword(string password, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // number of threads to use
                MemorySize = 8192, // memory size in KB
                Iterations = Iterations
            };

            return argon2.GetBytes(HashSize);
        }

        // Validate a password against a hashed password
        public static bool ValidatePassword(string password, byte[] salt, byte[] hashedPassword)
        {
            var hashToCompare = HashPassword(password, salt);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, hashedPassword);
        }
    }
}
