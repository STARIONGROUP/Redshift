#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityObject.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.
//
//    Redshift.Orm is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Orm.EntityObject
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Database;

    /// <summary>
    /// The purpose of the <see cref="EntityObject{TObject}"/> is to provide base methods and properties to model
    /// objects. Standardized methods for CRUD operations are provided.
    /// </summary>
    /// <typeparam name="TObject">
    /// The type of object contained within this ORM
    /// </typeparam>
    public abstract class EntityObject<TObject> : IEntityObject where TObject : IEntityObject
    {
        /// <summary>
        /// Gets the SQL table name that contains this concept. The default convention is the
        /// Type name, in all lowercase.
        /// </summary>
        [IgnoreDataMember]
        public virtual string TableName => $"{this.GetType().Name.ToLower()}";

        /// <summary>
        /// Gets the literal string name of the property that is used as the primary unique key.
        /// </summary>
        [IgnoreDataMember]
        public virtual string PrimaryKey => "Uuid";

        /// <summary>
        /// Gets the object of type <see tref="TObject"/> from the database, based on the supplied Id. The <see cref="PrimaryKey"/>
        /// property defines where the <see pref="id"/> parameter is looked for.
        /// </summary>
        /// <param name="id">
        /// The id of the object. The value is looked for in the column defined by the <see cref="PrimaryKey"/>
        /// property.
        /// </param>
        /// <returns>
        /// The <see tref="TObject"/> identified by the <see cref="PrimaryKey"/>. Null if not found.
        /// </returns>
        public static TObject Find(object id)
        {
            return DatabaseSession.Instance.Connector.ReadRecord<TObject>(id);
        }

        /// <summary>
        /// Returns the (approximate) number of records from this Entity's table.
        /// </summary>
        /// <returns>The number of records. -1 if failed.</returns>
        public static long Count()
        {
            return DatabaseSession.Instance.Connector.CountRecordsWhere<TObject>(new List<IWhereQueryContainer>());
        }

        /// <summary>
        /// Returns the count of filtered rows. Generally slower.
        /// </summary>
        /// <param name="whereQueries">
        /// The where Queries.
        /// </param>
        /// <param name="whereQueriesByAnd">A value indicating whether the where queries are joined by AND.</param>
        /// <returns>
        /// Number of rows.
        /// </returns>
        public static long CountWhere(List<IWhereQueryContainer> whereQueries, bool whereQueriesByAnd = true)
        {
            return DatabaseSession.Instance.Connector.CountRecordsWhere<TObject>(whereQueries, whereQueriesByAnd);
        }

        /// <summary>
        /// Creates an empty instance of this type.
        /// </summary>
        /// <returns>A new empty instance of an object of this type.</returns>
        public static TObject Template()
        {
            return (TObject)Activator.CreateInstance(typeof(TObject));
        }

        /// <summary>
        /// Gets all records of the provided <see cref="IEntityObject"/>
        /// </summary>
        /// <param name="orderBy">
        /// The order By.
        /// </param>
        /// <param name="orderDescending">
        /// The order Descending.
        /// </param>
        /// <returns>
        /// The <see cref="List{T}"/> of returned records.
        /// </returns>
        public static List<TObject> All(PropertyInfo orderBy = null, bool orderDescending = false)
        {
            return DatabaseSession.Instance.Connector.ReadRecords<TObject>(orderBy: orderBy, orderDescending: orderDescending);
        }

        /// <summary>
        /// Gets some records of the provided <see cref="IEntityObject"/>, with a specified limit, offset and order.
        /// </summary>
        /// <param name="limit">The limit of records to return.</param>
        /// <param name="offset">The offset of records to provide from.</param>
        /// <param name="orderBy">The property to order the records by.</param>
        /// <param name="orderDescending">Indicates whether to sort descending.</param>
        /// <returns>
        /// The <see cref="List{T}"/> of returned records.
        /// </returns>
        public static List<TObject> Subset(int limit, int offset, PropertyInfo orderBy, bool orderDescending = false)
        {
            return DatabaseSession.Instance.Connector.ReadRecords<TObject>(limit: limit, offset: offset, orderBy: orderBy, orderDescending: orderDescending);
        }

        /// <summary>
        /// Gets all records of the provided <see cref="IEntityObject"/> that match the where clause.
        /// </summary>
        /// <param name="property">The property to match.</param>
        /// <param name="condition">The condition for matching.</param>
        /// <param name="value">The value to match.</param>
        /// <param name="limit">The limit of records to return.</param>
        /// <param name="offset">The offset of records to provide from.</param>
        /// <param name="orderBy">The property to order the records by.</param>
        /// <param name="orderDescending">Indicates whether to sort descending.</param>
        /// <returns>
        /// The <see cref="List{T}"/> of returned records.
        /// </returns>
        public static List<TObject> Where(PropertyInfo property, string condition, object value, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false)
        {
            return DatabaseSession.Instance.Connector.ReadRecordsWhere<TObject>(property, condition, value, limit, offset, orderBy, orderDescending);
        }

        /// <summary>
        /// Gets all records of the provided <see cref="IEntityObject"/> that match the where clause.
        /// </summary>
        /// <param name="whereQueries">
        /// The where Queries.
        /// </param>
        /// <param name="whereQueriesByAnd">A value indicating whether the where queries are joined by AND.</param>
        /// <param name="limit">The limit of records to return.</param>
        /// <param name="offset">The offset of records to provide from.</param>
        /// <param name="orderBy">The property to order the records by.</param>
        /// <param name="orderDescending">Indicates whether to sort descending.</param>
        /// <returns>
        /// The <see cref="List{T}"/> of returned records.
        /// </returns>
        public static List<TObject> Where(List<IWhereQueryContainer> whereQueries, bool whereQueriesByAnd = true, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false)
        {
            return DatabaseSession.Instance.Connector.ReadRecordsWhere<TObject>(whereQueries, whereQueriesByAnd, limit, offset, orderBy, orderDescending);
        }

        /// <summary>
        /// Creates or update an instance of this object in the database. All properties are written.
        /// </summary>
        /// <param name="userUuid">
        /// The uuid of the user that has called this method.
        /// </param>
        /// <param name="ignoreNull">
        /// If set to true will not submit explicitly set values.
        /// </param>
        /// <param name="transaction">
        /// The transaction object.
        /// </param>
        /// <returns>
        /// The <see cref="IEntityObject"/> that is saved.
        /// </returns>
        public virtual IEntityObject Save(Guid? userUuid = null, bool ignoreNull = false, object transaction = null)
        {
            // find a record if it exists
            var existingRecord = Find(this.GetType().GetProperty(this.PrimaryKey).GetValue(this));

            if (existingRecord == null)
            {
                // if a record doesn't exist then write it to the database
                return DatabaseSession.Instance.Connector.CreateRecord(this, ignoreNull, transaction);
            }

            // if it does then update it
            return DatabaseSession.Instance.Connector.UpdateRecord(this, ignoreNull, transaction);
        }
        
        /// <summary>
        /// The deletes this object from the database.
        /// </summary>
        /// <param name="transaction">The transaction object.</param>
        public virtual void Delete(object transaction = null)
        {
            DatabaseSession.Instance.Connector.DeleteRecord(this, transaction);
        }
    }
}