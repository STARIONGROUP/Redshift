using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Redshift.Api;
using Redshift.Orm.EntityObject;

namespace Redshift.Sample.Model
{
    public static class MyProjectEntityResolverMapRegistrar
    {
        public static void Register()
        {
            // abstract entity resolver
            EntityResolverMap.AbstractToConcreteMap.TryAdd("BaseThing", new List<Type> { typeof(User) });

            // type property resolver
            EntityResolverMap.TypeToPropertyResolverMap.TryAdd(typeof(User), UserResolver.PropertyMap);

            // type property resolver
            EntityResolverMap.ApiRouteToPropertyResolverMap.TryAdd("User", UserResolver.PropertyMap);

            // deserialization resolver
            EntityResolverMap.DeserializationMap.TryAdd("user", UserResolver.FromJsonObject);
        }
    }
}
