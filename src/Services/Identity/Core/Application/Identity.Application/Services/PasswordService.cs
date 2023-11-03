using Identity.Application.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Application.Services
{
    public class PasswordService : IPasswordService
    {
        private const int SaltSize = 16;
        private const int keySize = 64;
        private const int iterations = 10000;
        private const char SaltDelimeter = ';';
        private HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                iterations,
                hashAlgorithm,
                keySize);
            return string.Join(SaltDelimeter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string password, string hashPassword)
        {
            var passwordSplits = hashPassword.Split(';');
            var salt = Convert.FromBase64String(passwordSplits[0]);
            var hash = Convert.FromBase64String(passwordSplits[1]);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, hashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, hash);
        }
    }
}
