namespace Redshift.Seed.Model
{
    using System;
    using System.Collections.Generic;
    using Api.Json;
    using Newtonsoft.Json.Linq;

    public static class UsergroupResolver
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
                "modifiedOn", typeof(DateTime)
            },
            {
                "createdOn", typeof(DateTime)
            },
            {
                "name", typeof(string)
            },
            {
                "permissions", typeof(string)
            }
        };

        /// <summary>
        /// Instantiate and deserialize the properties of a <paramref name="Usergroup"/>
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/> containing the data</param>
        /// <returns>The <see cref="User"/> to instantiate</returns>
        public static Usergroup FromJsonObject(JObject jObject)
        {
            var iid = jObject["uuid"].ToObject<Guid>();

            var usergroup = new Usergroup
            {
                Uuid = iid
            };

            if (!jObject["modifiedOn"].IsNullOrEmpty())
            {
                usergroup.ModifiedOn = jObject["modifiedOn"].ToObject<DateTime>();
            }

            if (!jObject["createdOn"].IsNullOrEmpty())
            {
                usergroup.CreatedOn = jObject["createdOn"].ToObject<DateTime>();
            }

            if (!jObject["name"].IsNullOrEmpty())
            {
                usergroup.Name = jObject["name"].ToObject<string>();
            }

            if (!jObject["permissions"].IsNullOrEmpty())
            {
                usergroup.Permissions.AddRange(jObject["permissions"].ToObject<IEnumerable<string>>());
            }

            After(usergroup, jObject);
            return usergroup;
        }

        /// <summary>
        /// Fills the object with the appropriate extra properties.
        /// </summary>
        /// <param name="usergroup">The user object.</param>
        /// <param name="jObject">The json object to deserialize from.</param>
        public static void After(Usergroup usergroup, JObject jObject)
        {

        }
    }
}
