using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Corbis.Public.Entity
{
    [Serializable]
    public class CorbisUser
    {
        /// <summary>
        /// User first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// User last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Full user name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// User token
        /// </summary>
        public UserToken Token { get; set; }
    }
}
