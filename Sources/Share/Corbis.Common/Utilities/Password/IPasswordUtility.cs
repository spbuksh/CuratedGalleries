using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Common.Utilities.Password
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPasswordUtility
    {
        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="clearPassword">The clear password.</param>
        /// <param name="hashKey">Key to generate password hash</param>      
        /// <returns>A string that contains the hashed password.</returns>
        string HashPassword(string clearPassword, string hashKey);

        /// <summary>
        /// Generates a random password with at least one uppercase character, lowercase character, and number.
        /// </summary>
        /// <returns></returns>
        string GeneratePassword();

        /// <summary>
        /// Generates a random password with at least one uppercase character, lowercase character, and number.
        /// </summary>
        /// <param name="length">The password length.</param>
        /// <returns></returns>
        string GeneratePassword(int length);

        /// <summary>
        /// Generates a random password with at least one uppercase character, lowercase character, and number.
        /// </summary>
        /// <param name="minLength">Minimal possible password length.</param>
        /// <param name="maxLength">Maximal possible password length.</param>
        /// <returns></returns>
        string GeneratePassword(int minLength, int maxLength);
    }
}
