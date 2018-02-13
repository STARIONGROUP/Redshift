#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResponseContainer.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Api is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Api is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Api.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Api.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Helpers;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// Convenience container for response structures.
    /// </summary>
    public class ResponseContainer : Dictionary<string, List<object>>
    {
        /// <summary>
        /// The string name of the collection of deleted objects.
        /// </summary>
        private static string deletedCollectionName = "deleted";

        /// <summary>
        /// Gets the list of all non-deleted things in the container.
        /// </summary>
        public List<IEntityObject> Things => this.Values.SelectMany(o => o).OfType<IEntityObject>().ToList();

        /// <summary>
        /// Gets the list of uuids marked to be deleted.
        /// </summary>
        public List<Guid> DeletedThings => !this.ContainsKey(deletedCollectionName) ? new List<Guid>() : this[deletedCollectionName].OfType<Guid>().ToList();

        /// <summary>
        /// Adds an object to the response collection.
        /// </summary>
        /// <param name="thing">The thing thats needs to be added.</param>
        /// <param name="cullValues">
        /// The boolean indicating whether to cull Values.
        /// </param>
        public void AddToResponse(IEntityObject thing, bool cullValues = true)
        {
            if (cullValues)
            {
                ApiHelper.CullApiNullValues(thing);
            }

            var type = thing.GetType();

            List<object> collection;

            // try to get the collection if it exists already
            if (this.TryGetValue(type.Name, out collection))
            {
                // skip object if it is already in
                if (!collection.Contains(thing))
                {
                    collection.Add(thing);
                }
            }
            else
            {
                this.Add(type.Name, new List<object> { thing });
            }
        }

        /// <summary>
        /// Adds a collection of objects to the response collection.
        /// </summary>
        /// <typeparam name="T">
        /// The type of objects being added.
        /// </typeparam>
        /// <param name="things">
        /// The list of entities to add to the response.
        /// </param>
        /// <param name="cullValues">
        /// The boolean indicating whether to cull Values.
        /// </param>
        public void AddToResponse<T>(List<T> things, bool cullValues = true)
        {
            if (!things.Any())
            {
                return;
            }

            var type = things.First().GetType();

            List<object> collection;

            if (cullValues)
            {
                ApiHelper.CullApiNullValues(things.OfType<IEntityObject>().ToList());
            }
 
            // try to get the collection if it exists already
            if (this.TryGetValue(type.Name, out collection))
            {
                foreach (var entityObject in things)
                {
                    if (!collection.Contains(entityObject))
                    {
                        collection.Add(entityObject);
                    }
                }
            }
            else
            {
                var newCollection = new List<object>();
                newCollection.AddRange(things.OfType<object>().ToList());

                this.Add(type.Name, newCollection);
            }
        }

        /// <summary>
        /// Adds a <see cref="Guid"/> to the collection of deleted objects.
        /// </summary>
        /// <param name="uuid">The <see cref="Guid"/> to be added to the collection.</param>
        public void AddDeleted(Guid uuid)
        {
            if (!this.ContainsKey(deletedCollectionName))
            {
                this.Add(deletedCollectionName, new List<object>());
            }

            // the deleted collection is always there.
            var collection = this[deletedCollectionName];

            // if it exists in the collection already do not bother.
            if (!collection.Any(o => o.Equals(uuid)))
            {
                collection.Add(uuid);
            }
        }

        /// <summary>
        /// Adds a <see cref="DeletedThing"/>s to the collection of deleted objects.
        /// </summary>
        /// <param name="things">The list of deleted things.</param>
        public void AddDeleted(List<DeletedThing> things)
        {
            if (!this.ContainsKey(deletedCollectionName))
            {
                this.Add(deletedCollectionName, new List<object>());
            }

            // the deleted collection is always there.
            var collection = this[deletedCollectionName];

            collection.AddRange(things.Where(t => !collection.Contains(t.Uuid)).Select(thing => thing.Uuid).OfType<object>());
        }

        /// <summary>
        /// Removes a <see cref="Guid"/> from the collection of deleted objects.
        /// </summary>
        /// <param name="uuid">The <see cref="Guid"/> to be removed from the collection.</param>
        public void RemoveDeleted(Guid uuid)
        {
            if (!this.ContainsKey(deletedCollectionName))
            {
                return;
            }

            // the deleted collection is always there.
            var collection = this[deletedCollectionName];

            // if it exists in the collection already do not bother.
            if (collection.Any(o => o.Equals(uuid)))
            {
                collection.Remove(uuid);
            }

            // if collection is empty then remove it
            if (!collection.Any())
            {
                this.Remove(deletedCollectionName);
            }
        }
    }
}
