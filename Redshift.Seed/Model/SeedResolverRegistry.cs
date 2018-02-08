namespace Redshift.Seed.Model
{
    using System;
    using System.Collections.Generic;
    using Api;

    /// <summary>
    /// Registry of the entity maps.
    /// </summary>
    public static class SeedResolverRegistry
    {
        /// <summary>
        /// Register the various types.
        /// </summary>
        public static void Register()
        {
            // abstract entity resolver
            EntityResolverMap.AbstractToConcreteMap.TryAdd("Thing", new List<Type> { typeof(User), typeof(Usergroup) });

            // type property resolver
            EntityResolverMap.TypeToPropertyResolverMap.TryAdd(typeof(User), UserResolver.PropertyMap);
            EntityResolverMap.TypeToPropertyResolverMap.TryAdd(typeof(Usergroup), UserResolver.PropertyMap);

            // type property resolver
            EntityResolverMap.ApiRouteToPropertyResolverMap.TryAdd("User", UserResolver.PropertyMap);
            EntityResolverMap.ApiRouteToPropertyResolverMap.TryAdd("Usergroup", UsergroupResolver.PropertyMap);

            // deserialization resolver
            EntityResolverMap.DeserializationMap.TryAdd("user", UserResolver.FromJsonObject);
            EntityResolverMap.DeserializationMap.TryAdd("usergroup", UsergroupResolver.FromJsonObject);
        }
    }
}
