#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InMemoryMockDatabaseConnector.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.
//
//    Redshift.Orm is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Orm.Database
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;

    using Redshift.Orm.EntityObject;

    [ExcludeFromCodeCoverage]
    public class InMemoryMockDatabaseConnector : IDatabaseConnector
    {
        private readonly Dictionary<Guid, IEntityObject> Cache = new Dictionary<Guid, IEntityObject>();
        private readonly Dictionary<Guid, IEntityObject> ThingCache = new Dictionary<Guid, IEntityObject>();
        private readonly Dictionary<Guid, IEntityObject> DeletedThingCache = new Dictionary<Guid, IEntityObject>();

        public ConnectorType ConnectorType => ConnectorType.InMemory;

        /// <summary>
        /// The create connection.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <returns>
        /// The <see cref="object"/>.
        /// </returns>
        public object CreateConnection(DatabaseCredentials credentials)
        {
            return null;
        }

        public object CreateTransaction(DatabaseCredentials credentials)
        {
            return null;
        }

        public void CommitTransaction(object transaction)
        {
            return;
        }

        public void CreateTable(IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreateTable(string tableName, object transaction = null)
        {
            return;
        }

        public void CreateTableWithColumns(IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreateTableWithColumnsAndPrimaryKey(IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void DeleteTable(IEntityObject thing, object transaction = null)
        {
            // should delete instances of type of that table.
            var instances = this.Cache.Where(kv => kv.Value.GetType() == thing.GetType());

            foreach (var instance in instances)
            {
                this.Cache.Remove(instance.Key);
            }
        }

        public void DeleteTable(string table, object transaction = null)
        {
            // should delete instances of type of that table.
            var instances = this.Cache.Where(kv => kv.Value.TableName == table);

            foreach (var instance in instances)
            {
                this.Cache.Remove(instance.Key);
            }
        }

        public bool CheckTableExists(IEntityObject thing)
        {
            return true;
        }

        public bool CheckTableExists(string tableName)
        {
            return true;
        }

        public bool CheckColumnExists(PropertyInfo property, IEntityObject thing)
        {
            return true;
        }

        public bool CheckColumnExists(string columnName, IEntityObject thing)
        {
            return true;
        }

        public bool CheckColumnExists(string columnName, string tableName)
        {
            return true;
        }

        public void CreateColumn(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreateColumn(string tableName, string columnName, Type type, bool isNullable = true, object transaction = null)
        {
            return;
        }

        public void DeleteColumn(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void DeleteColumn(string property, string table, object transaction = null)
        {
            return;
        }

        public void DeleteDefault(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void SetDefault(PropertyInfo property, object value, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreateUniquenessConstraint(PropertyInfo[] property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void DeleteUniquenessConstraint(PropertyInfo[] property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreateNotNullConstraint(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void DeleteNotNullConstraint(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreatePrimaryKeyConstraint(IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreatePrimaryKeyConstraint(string table, string[] primaryKeyColumns, object transaction = null)
        {
            return;
        }

        public void DeletePrimaryKeyConstraint(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void DeletePrimaryKeyConstraint(string table, object transaction = null)
        {
            return;
        }

        public void CreateForeignKeyConstraint(
            PropertyInfo fromProperty,
            IEntityObject fromThing,
            PropertyInfo toProperty,
            IEntityObject toThing,
            FkDeleteBehaviorKind onDelete = FkDeleteBehaviorKind.NO_ACTION,
            bool deferred = true,
            object transaction = null)
        {
            return;
        }

        public void CreateForeignKeyConstraint(
            string fromProperty,
            string fromTable,
            string toProperty,
            string toTable,
            FkDeleteBehaviorKind onDelete = FkDeleteBehaviorKind.NO_ACTION,
            bool deferred = true,
            object transaction = null)
        {
            return;
        }

        public void DeleteForeignKeyConstraint(
            string fromProperty,
            string fromThing,
            string toProperty,
            string toThing,
            object transaction = null)
        {
            return;
        }

        public void DeleteForeignKeyConstraint(
            PropertyInfo fromProperty,
            IEntityObject fromThing,
            PropertyInfo toProperty,
            IEntityObject toThing,
            object transaction = null)
        {
            return;
        }

        public void DeleteForeignKeyConstraint(string fromTable, string constraintName, object transaction)
        {
            return;
        }

        public T CreateRecord<T>(T thing, bool ignoreNull = false, object transaction = null) where T : IEntityObject
        {
            var uuid = thing.GetType().GetProperty(thing.PrimaryKey).GetValue(thing) as Guid?;

            this.Cache.Add((Guid)uuid, thing);

            // replication
            var thingEntity = new Thing()
                                  {
                                      Uuid = (Guid)uuid,
                                      ModifiedOn = DateTime.UtcNow,
                                      ThingType = thing.GetType().Name
                                  };
            this.ThingCache.Add((Guid)uuid, thingEntity);

            return thing;
        }

        public void CreateRecord(string table, string[] columns, object[] values, object transaction = null)
        {
            return;
        }

        public T UpdateRecord<T>(T thing, bool ignoreNull = false, object transaction = null) where T : IEntityObject
        {
            var uuid = thing.GetType().GetProperty(thing.PrimaryKey).GetValue(thing) as Guid?;
            this.Cache.Remove((Guid)uuid);
            this.ThingCache.Remove((Guid)uuid);

            this.Cache.Add((Guid)uuid, thing);

            // replication
            var thingEntity = new Thing()
            {
                Uuid = (Guid)uuid,
                ModifiedOn = DateTime.UtcNow,
                ThingType = thing.GetType().Name
            };
            this.ThingCache.Add((Guid)uuid, thingEntity);

            return thing;
        }

        public void DeleteRecord(IEntityObject thing, object transaction = null)
        {
            var uuid = thing.GetType().GetProperty(thing.PrimaryKey).GetValue(thing) as Guid?;
            this.Cache.Remove((Guid)uuid);
            this.ThingCache.Remove((Guid)uuid);

            var deletedThing = new DeletedThing()
                                   {
                                       Uuid = (Guid)uuid,
                                       ModifiedOn = DateTime.UtcNow
                                   };
            this.DeletedThingCache.Add((Guid)uuid, deletedThing);

            return;
        }

        public void DeleteRecord(string table, string pkey, object value, object transaction = null)
        {
            return;
        }

        public List<T> ReadRecords<T>(int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject
        {
            var dic = typeof(T) == typeof(Thing) ? this.ThingCache : this.Cache;

            dic = typeof(T) == typeof(DeletedThing) ? this.DeletedThingCache : dic;

            if (limit == null)
            {
                return dic.Values.OfType<T>().ToList();
            }
            else
            {
                return dic.Values.OfType<T>().ToList().GetRange(0, (int)limit);
            }
        }

        public List<T> ReadRecordsWhere<T>(PropertyInfo fromProperty, string comparer, object value, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject
        {
            var dic = typeof(T) == typeof(Thing) ? this.ThingCache : this.Cache;
            dic = typeof(T) == typeof(DeletedThing) ? this.DeletedThingCache : dic;

            var all = dic.Values.OfType<T>().ToList();

            if (comparer == ">")
            {
                return all.Where(c => (DateTime)fromProperty.GetValue(c) > (DateTime)value).ToList();
            }
            else if (comparer == "<")
            {
                return all.Where(c => (DateTime)fromProperty.GetValue(c) < (DateTime)value).ToList();
            }
            else if (comparer == "<=")
            {
                return all.Where(c => (DateTime)fromProperty.GetValue(c) <= (DateTime)value).ToList();
            }
            else if (comparer == ">=")
            {
                return all.Where(c => (DateTime)fromProperty.GetValue(c) >= (DateTime)value).ToList();
            }

            return all.Where(c => fromProperty.GetValue(c).Equals(value)).ToList();
        }

        public T ReadRecord<T>(object id) where T : IEntityObject
        {
            var dic = typeof(T) == typeof(Thing) ? this.ThingCache : this.Cache;
            dic = typeof(T) == typeof(DeletedThing) ? this.DeletedThingCache : dic;

            IEntityObject thing;
            dic.TryGetValue((Guid)id, out thing);

            return (T)thing;
        }

        public void AlterSchemaOwner(string schemaName, object transaction)
        {
            return;
        }

        public void DropAllTables(string schemaName)
        {
            this.Cache.Clear();
        }

        public void DropSchema(string schemaName, object transaction = null)
        {
            this.Cache.Clear();
        }

        public void CreateSchema(string schemaName, object transaction = null)
        {
            return;
        }

        public void CreateReplicateFunctionAndTrigger(IEntityObject thing, string replicationTable, object transaction = null)
        {
            return;
        }

        public void DeleteReplicateFunctionAndTrigger(IEntityObject thing, object transaction = null)
        {
            return;
        }

        public void CreateFunctionAndTrigger(
            string table,
            string functionName,
            string functionBody,
            bool isBefore,
            bool onInsert,
            bool onUpdate,
            bool onDelete,
            object transaction = null)
        {
            return;
        }

        public void CreateFunction(string functionName, string functionBody, string returnType, object transaction = null)
        {
            return;
        }

        public void DeleteFunction(string functionName, object transaction = null)
        {
            return;
        }

        public List<T> ReadRecordsWhere<T>(IEnumerable<IWhereQueryContainer> queries, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject
        {
            return this.Cache.Values.OfType<T>().ToList();
        }

        public long CountRecords<T>(T thing) where T : IEntityObject
        {
            return this.Cache.Values.OfType<T>().Count();
        }

        public long CountRecordsWhere<T>(IEnumerable<IWhereQueryContainer> queries) where T : IEntityObject
        {
            return this.Cache.Values.OfType<T>().Count();
        }

        public long CountRecords(string tableName)
        {
            return this.Cache.Values.Count(v => v.GetType().Name.ToLower() == tableName.ToLower());
        }

        public void CreateIndex(IEntityObject thing, List<PropertyInfo> columns, object transaction = null)
        {
            return;
        }

        public void CreateIndex(string tableName, IList<string> columnNames, object transaction = null)
        {
            return;
        }

        public void DeleteIndex(IEntityObject thing, List<PropertyInfo> columns, object transaction = null)
        {
            return;
        }

        public void DeleteIndex(string tableName, IList<string> columnNames, object transaction = null)
        {
            return;
        }

        public void DeleteIndex(string indexName, object transaction = null)
        {
            return;
        }
    }
}
