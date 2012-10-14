﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Corbis.Common.Utilities.Password
{
    /// <summary>
    /// This class is staless therefore it is threadsafe
    /// </summary>
    public class PasswordUtility : IPasswordUtility
    {
        /// <summary>
        /// Singleton
        /// </summary>
        public static PasswordUtility Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (typeof(PasswordUtility))
                    {
                        if (m_Instance == null)
                            m_Instance = new PasswordUtility();
                    }
                }
                return m_Instance;
            }
        }
        private static PasswordUtility m_Instance = null;

        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="clearPassword">The clear password.</param>
        /// <param name="hashKey">Key to generate password hash</param>
        /// <returns>A string that contains the hashed password.</returns>
        public string HashPassword(string clearPassword, string hashKey)
        {
            HMACSHA256 hash = new HMACSHA256 { Key = HexToByte(hashKey) };

            byte[] hashedPassword = hash.ComputeHash(Encoding.Unicode.GetBytes(clearPassword));
            string hashedPasswordBase64 = Convert.ToBase64String(hashedPassword);

            return hashedPasswordBase64;
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption 
        /// key values from the configuration.
        /// </summary>
        /// <param name="hexString">String representing the long hexademical value</param>
        /// <returns>Array of bytes of the input string</returns>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        #region Password generation

        // You may use code samples copyrighted by Obviex™ without asking for explicit permission, provided that You leave the original copyright notice in the source code. 
        // Obviex™ is not responsible, accountable, or liable for any damages, problems, 
        // or difficulties resulting from use of the code samples available or referenced on this Site. 
        //
        // THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
        // EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
        // WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
        // 
        // Copyright (C) 2004 Obviex(TM). All rights reserved.

        private static readonly string PasswordCharsLowerCase = "abcdefgijkmnopqrstwxyz";
        private static readonly string PasswordCharsUpperCase = "ABCDEFGHJKLMNPQRSTWXYZ";
        private static readonly string PasswordCharsNumeric = "123456789";
        //private static string PASSWORD_CHARS_SPECIAL = "*$-+?_&=!%{}/";

        private static readonly int DefaultMinPasswordLength = 8;
        private static readonly int DefaultMaxPasswordLength = 12;

        /// <summary>
        /// Generates a random password with at least one uppercase character, lowercase character, and number.
        /// </summary>
        /// <returns></returns>
        public string GeneratePassword()
        {
            return GeneratePassword(DefaultMinPasswordLength, DefaultMaxPasswordLength);
        }

        /// <summary>
        /// Generates a random password with at least one uppercase character, lowercase character, and number.
        /// </summary>
        /// <param name="length">The password length.</param>
        /// <returns></returns>
        public string GeneratePassword(int length)
        {
            return GeneratePassword(length, length);
        }

        /// <summary>
        /// Generates a random password with at least one uppercase character, lowercase character, and number.
        /// </summary>
        /// <param name="minLength">Minimal possible password length.</param>
        /// <param name="maxLength">Maximal possible password length.</param>
        /// <returns></returns>
        public string GeneratePassword(int minLength, int maxLength)
        {
            // Make sure that input parameters are valid.
            if (minLength <= 0 || maxLength <= 0 || minLength > maxLength)
                return null;

            // Create a local array containing supported password characters
            // grouped by types. You can remove character groups from this
            // array, but doing so will weaken the password strength.
            char[][] charGroups = new char[][]
                                      {
                                          PasswordCharsLowerCase.ToCharArray(),
                                          PasswordCharsUpperCase.ToCharArray(),
                                          PasswordCharsNumeric.ToCharArray()
                                      };

            // Use this array to track the number of unused characters in each
            // character group.
            int[] charsLeftInGroup = new int[charGroups.Length];

            // Initially, all characters in each group are not used.
            for (int i = 0; i < charsLeftInGroup.Length; i++)
                charsLeftInGroup[i] = charGroups[i].Length;

            // Use this array to track (iterate through) unused character groups.
            int[] leftGroupsOrder = new int[charGroups.Length];

            // Initially, all character groups are not used.
            for (int i = 0; i < leftGroupsOrder.Length; i++)
                leftGroupsOrder[i] = i;

            // Because we cannot use the default randomizer, which is based on the
            // current time (it will produce the same "random" number within a
            // second), we will use a random number generator to seed the
            // randomizer.

            // Use a 4-byte array to fill it with random bytes and convert it then
            // to an integer value.
            byte[] randomBytes = new byte[4];

            // Generate 4 random bytes.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomBytes);

            // Convert 4 bytes into a 32-bit integer value.
            int seed = (randomBytes[0] & 0x7f) << 24 |
                        randomBytes[1] << 16 |
                        randomBytes[2] << 8 |
                        randomBytes[3];

            // Now, this is real randomization.
            Random random = new Random(seed);

            // This array will hold password characters.
            char[] password = null;

            // Allocate appropriate memory for the password.
            if (minLength < maxLength)
                password = new char[random.Next(minLength, maxLength + 1)];
            else
                password = new char[minLength];

            // Index of the next character to be added to password.
            int nextCharIdx;

            // Index of the next character group to be processed.
            int nextGroupIdx;

            // Index which will be used to track not processed character groups.
            int nextLeftGroupsOrderIdx;

            // Index of the last non-processed character in a group.
            int lastCharIdx;

            // Index of the last non-processed group.
            int lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

            // Generate password characters one at a time.
            for (int i = 0; i < password.Length; i++)
            {
                // If only one character group remained unprocessed, process it;
                // otherwise, pick a random character group from the unprocessed
                // group list. To allow a special character to appear in the
                // first position, increment the second parameter of the Next
                // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
                if (lastLeftGroupsOrderIdx == 0)
                    nextLeftGroupsOrderIdx = 0;
                else
                    nextLeftGroupsOrderIdx = random.Next(0, lastLeftGroupsOrderIdx + 1);

                // Get the actual index of the character group, from which we will
                // pick the next character.
                nextGroupIdx = leftGroupsOrder[nextLeftGroupsOrderIdx];

                // Get the index of the last unprocessed characters in this group.
                lastCharIdx = charsLeftInGroup[nextGroupIdx] - 1;

                // If only one unprocessed character is left, pick it; otherwise,
                // get a random character from the unused character list.
                if (lastCharIdx == 0)
                    nextCharIdx = 0;
                else
                    nextCharIdx = random.Next(0, lastCharIdx + 1);

                // Add this character to the password.
                password[i] = charGroups[nextGroupIdx][nextCharIdx];

                // If we processed the last character in this group, start over.
                if (lastCharIdx == 0)
                    charsLeftInGroup[nextGroupIdx] =
                                              charGroups[nextGroupIdx].Length;
                // There are more unprocessed characters left.
                else
                {
                    // Swap processed character with the last unprocessed character
                    // so that we don't pick it until we process all characters in
                    // this group.
                    if (lastCharIdx != nextCharIdx)
                    {
                        char temp = charGroups[nextGroupIdx][lastCharIdx];
                        charGroups[nextGroupIdx][lastCharIdx] =
                                    charGroups[nextGroupIdx][nextCharIdx];
                        charGroups[nextGroupIdx][nextCharIdx] = temp;
                    }
                    // Decrement the number of unprocessed characters in
                    // this group.
                    charsLeftInGroup[nextGroupIdx]--;
                }

                // If we processed the last group, start all over.
                if (lastLeftGroupsOrderIdx == 0)
                    lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
                // There are more unprocessed groups left.
                else
                {
                    // Swap processed group with the last unprocessed group
                    // so that we don't pick it until we process all groups.
                    if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                    {
                        int temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                        leftGroupsOrder[lastLeftGroupsOrderIdx] =
                                    leftGroupsOrder[nextLeftGroupsOrderIdx];
                        leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                    }
                    // Decrement the number of unprocessed groups.
                    lastLeftGroupsOrderIdx--;
                }
            }

            // Convert password characters into a string and return the result.
            return new string(password);
        }

        #endregion
    }
}
