#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CryptographyHelper.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Api is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Api is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Api.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Api.Helpers
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Helper methods to help with password encoding and checking.
    /// </summary>
    public static class CryptographyHelper
    {
        /// <summary>
        /// Generates a random salt of defined length.
        /// </summary>
        /// <returns>A randomly generated salt.</returns>
        public static string GetSalt()
        {
            const int saltLength = 32;

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[saltLength];
                randomNumberGenerator.GetBytes(randomNumber);

                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Encodes the password and salt to form a hash.
        /// </summary>
        /// <param name="password">The plain password to be encoded.</param>
        /// <param name="salt">The salt string.</param>
        /// <returns>The hashed string.</returns>
        public static string Encrypt(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedHash = MergeBytes(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));

                return Convert.ToBase64String(sha256.ComputeHash(combinedHash));
            }
        }

        /// <summary>
        /// Combines the two byte arrays into one.
        /// </summary>
        /// <param name="lhs">The first array.</param>
        /// <param name="rhs">The second array.</param>
        /// <returns>The combined byte array.</returns>
        private static byte[] MergeBytes(byte[] lhs, byte[] rhs)
        {
            var ret = new byte[lhs.Length + rhs.Length];

            Buffer.BlockCopy(lhs, 0, ret, 0, lhs.Length);
            Buffer.BlockCopy(rhs, 0, ret, lhs.Length, rhs.Length);

            return ret;
        }
    }
}
