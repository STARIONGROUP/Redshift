using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Redshift.Api.Helpers;
using Redshift.Api.Json;

namespace Redshift.Sample.Model
{
    public static class UserResolver
    {
        /// <summary>
        /// Map of property names and types.
        /// </summary>
        public static readonly Dictionary<string, Type> PropertyMap = new Dictionary<string, Type>
        {
            {
                "uuid", typeof(Guid)
            },
            {
                "username", typeof(string)
            },
            {
                "email", typeof(string)
            }
        };

        /// <summary>
        /// Instantiate and deserialize the properties of a <paramref name="User"/>
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/> containing the data</param>
        /// <returns>The <see cref="User"/> to instantiate</returns>
        public static User FromJsonObject(JObject jObject)
        {
            var iid = jObject["uuid"].ToObject<Guid>();

            var user = new User();
            
            if (!jObject["username"].IsNullOrEmpty())
            {
                user.Username = jObject["username"].ToObject<string>();
            }

            if (!jObject["email"].IsNullOrEmpty())
            {
                user.Email = jObject["email"].ToObject<string>();
            }

            After(user, jObject);
            return user;
        }

        /// <summary>
        /// Fills the object with the appropriate extra properties.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <param name="jObject">The json object to deserialize from.</param>
        public static void After(User user, JObject jObject)
        {
            if (!jObject["password"].IsNullOrEmpty())
            {
                user.Password = jObject["password"].ToObject<string>();
            }
        }
    }
}
