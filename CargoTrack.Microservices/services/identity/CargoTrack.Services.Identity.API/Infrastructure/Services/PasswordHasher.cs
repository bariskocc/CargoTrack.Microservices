using System;
using System.Security.Cryptography;
using CargoTrack.Services.Identity.API.Domain.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CargoTrack.Services.Identity.API.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 128 / 8; // 128 bit
        private const int HashSize = 256 / 8; // 256 bit
        private const int Iterations = 10000;
        private const KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;
        private const int IterationIndex = 0;
        private const int PrfIndex = 1;
        private const int SaltIndex = 2;
        private const int HashIndex = 3;

        public string HashPassword(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            byte[] hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: Prf,
                iterationCount: Iterations,
                numBytesRequested: HashSize);

            var outputBytes = new byte[4 + SaltSize + HashSize];
            outputBytes[IterationIndex] = (byte)(Iterations >> 8);
            outputBytes[PrfIndex] = (byte)Prf;
            Buffer.BlockCopy(salt, 0, outputBytes, SaltIndex, SaltSize);
            Buffer.BlockCopy(hash, 0, outputBytes, HashIndex, HashSize);

            return Convert.ToBase64String(outputBytes);
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (hashedPassword == null)
                throw new ArgumentNullException(nameof(hashedPassword));

            byte[] decodedHashedPassword;
            try
            {
                decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            }
            catch (FormatException)
            {
                return false;
            }

            if (decodedHashedPassword.Length < 4 + SaltSize + HashSize)
                return false;

            int iterations = decodedHashedPassword[IterationIndex] << 8;
            KeyDerivationPrf prf = (KeyDerivationPrf)decodedHashedPassword[PrfIndex];

            if (prf != Prf)
                return false;

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(decodedHashedPassword, SaltIndex, salt, 0, SaltSize);

            byte[] expectedHash = new byte[HashSize];
            Buffer.BlockCopy(decodedHashedPassword, HashIndex, expectedHash, 0, HashSize);

            byte[] actualHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: prf,
                iterationCount: iterations,
                numBytesRequested: HashSize);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }

        public bool NeedsRehash(string hashedPassword)
        {
            if (hashedPassword == null)
                throw new ArgumentNullException(nameof(hashedPassword));

            byte[] decodedHashedPassword;
            try
            {
                decodedHashedPassword = Convert.FromBase64String(hashedPassword);
            }
            catch (FormatException)
            {
                return true;
            }

            if (decodedHashedPassword.Length < 4 + SaltSize + HashSize)
                return true;

            int iterations = decodedHashedPassword[IterationIndex] << 8;
            KeyDerivationPrf prf = (KeyDerivationPrf)decodedHashedPassword[PrfIndex];

            return iterations != Iterations || prf != Prf;
        }
    }
} 