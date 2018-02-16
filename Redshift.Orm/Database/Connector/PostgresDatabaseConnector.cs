#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostgresDatabaseConnector.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Database
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security.Authentication;

    using Attributes;
    using EntityObject;
    using Helpers;
    using Npgsql;
    using NpgsqlTypes;

    /// <summary>
    /// The postgres database connector.
    /// </summary>
    internal class PostgresDatabaseConnector : IDatabaseConnector
    {
        /// <summary>
        /// Gets the <see cref="IDatabaseConnector.ConnectorType"/> which represents the type of database this is.
        /// </summary>
        public ConnectorType ConnectorType => ConnectorType.Postgresql;

        /// <summary>
        /// Creates a table from an <see cref="IEntityObject"/>
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateTable(IEntityObject thing, object transaction = null)
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            this.CreateTable(thing.TableName, transaction);
        }

        /// <summary>
        /// Create a table with the provided name
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="transaction">the <paramref name="transaction"/></param>
        public void CreateTable(string tableName, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // IF NOT EXISTS removed for security
            var sql = $"CREATE TABLE {tableName.MakePostgreSqlSafe()} ();";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Creating table {tableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Create a table from an <see cref="IEntityObject"/> with all columns.
        /// </summary>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to be used as template
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateTableWithColumns(IEntityObject thing, object transaction = null)
        {
            DatabaseSession.Instance.Connector.CreateTable(thing, transaction);

            foreach (var property in
                thing.GetType()
                         .GetProperties()
                         .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                         .ToList())
            {
                DatabaseSession.Instance.Connector.CreateColumn(property, thing, transaction);
            }
        }

        /// <summary>
        /// Create a table from an <see cref="IEntityObject"/> with all columns and a primary key.
        /// </summary>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to be used as template
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateTableWithColumnsAndPrimaryKey(IEntityObject thing, object transaction = null)
        {
            DatabaseSession.Instance.Connector.CreateTableWithColumns(thing, transaction);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(thing, transaction);
        }

        /// <summary>
        /// Deletes a table based on an <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteTable(IEntityObject thing, object transaction = null)
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            this.DeleteTable(thing.TableName, transaction);
        }

        /// <summary>
        /// Deletes a <paramref name="table"/>
        /// </summary>
        /// <param name="table">The name of the table to delete.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteTable(string table, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // IF EXISTS removed to ensure safety
            var sql = $"DROP TABLE {table.MakePostgreSqlSafe()} CASCADE;";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Dropping table {table} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Checks whether the table from a given ORM object exists in the database
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <returns>True if the table exists.</returns>
        public bool CheckTableExists(IEntityObject thing)
        {
            var strippedTableName = thing.TableName.Replace("\"", string.Empty);

            return this.CheckTableExists(strippedTableName);
        }

        /// <summary>
        /// Checks whether the table with a given name exists in the database
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <returns>True if the table exists.</returns>
        public bool CheckTableExists(string tableName)
        {
            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;

            var sql = "SELECT EXISTS (";
            sql += " SELECT 1";
            sql += " FROM   information_schema.tables";
            sql += " WHERE  table_schema = 'public'";
            sql += $" AND table_name = '{tableName}'";
            sql += ");";

            var result = false;

            var cmd = new NpgsqlCommand(sql, con);

            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                result = bool.Parse(dr[0].ToString());
            }

            dr.Close();

            con?.Close();

            return result;
        }

        /// <summary>
        /// Checks whether the column based on <see cref="PropertyInfo"/> exists in the table of a given <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The properties representing the column.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <returns>True if the column exists.</returns>
        public bool CheckColumnExists(PropertyInfo property, IEntityObject thing)
        {
            var columnName = EntityHelper.GetColumnNameFromProperty(property);
            var strippedName = columnName.Replace("\"", string.Empty);

            return this.CheckColumnExists(strippedName, thing);
        }

        /// <summary>
        /// Checks whether the column based on name exists in the table of a given <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <returns>True if the column exists.</returns>
        public bool CheckColumnExists(string columnName, IEntityObject thing)
        {
            var strippedTableName = thing.TableName.Replace("\"", string.Empty);
            return this.CheckColumnExists(columnName, strippedTableName);
        }

        /// <summary>
        /// Checks whether the column based on name exists in the table of a given name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>True if the column exists.</returns>
        public bool CheckColumnExists(string columnName, string tableName)
        {
            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;

            var sql = "SELECT EXISTS (";
            sql += " SELECT 1";
            sql += " FROM   information_schema.columns";
            sql += $" WHERE  table_name = '{tableName}'";
            sql += $" AND column_name = '{columnName}'";
            sql += ");";

            var result = false;

            var cmd = new NpgsqlCommand(sql, con);

            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                result = bool.Parse(dr[0].ToString());
            }

            dr.Close();

            con?.Close();

            return result;
        }

        /// <summary>
        /// Create a column based on a properties of a certain object.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateColumn(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            if (property.IsDefined(typeof(DbIgnoreAttribute)) || property.IsDefined(typeof(IgnoreDataMemberAttribute)))
            {
                throw new ArgumentException($"Can't create column marked as ignored. {EntityHelper.GetColumnNameFromProperty(property)}", nameof(property));
            }

            var name = EntityHelper.GetColumnNameFromProperty(property);
            this.CreateColumn(thing.TableName, name, property.PropertyType, true, transaction);
        }

        /// <summary>
        /// Create a column of a specific type with the given name in the given table
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="columnName">The name of the column to create</param>
        /// <param name="type">The .NET <see cref="Type"/> of the column to create</param>
        /// <param name="isNullable">Indicates whether the field if nullable or not</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateColumn(string tableName, string columnName, Type type, bool isNullable = true, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var typeName = GetTypeStringFromProperty(type);
            var sql = $"ALTER TABLE {tableName.MakePostgreSqlSafe()} ADD COLUMN {columnName.MakePostgreSqlSafe()} {typeName}{(isNullable ? " " : " NOT ")}NULL;";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Create column {columnName} on table {tableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Delete a column based on a properties of a certain object
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteColumn(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            this.DeleteColumn(EntityHelper.GetColumnNameFromProperty(property), thing.TableName, transaction);
        }

        /// <summary>
        /// Delete a column from a <paramref name="table"/>
        /// </summary>
        /// <param name="property">The column name.</param>
        /// <param name="table">The table name.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteColumn(string property, string table, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // IF EXISTS properties removed to create safety
            var sql = $"ALTER TABLE {table.MakePostgreSqlSafe()} DROP COLUMN {property.MakePostgreSqlSafe()};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Dropping column {property} on table {table} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Make the specified column (based on <see cref="PropertyInfo"/>) a primary key.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreatePrimaryKeyConstraint(IEntityObject thing, object transaction = null)
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            var name = EntityHelper.GetColumnNameFromProperty(thing.GetType().GetProperty(thing.PrimaryKey));
            this.CreatePrimaryKeyConstraint(thing.TableName, new[] { name }, transaction);
        }

        /// <summary>
        /// Make the specified columns a primary key.
        /// </summary>
        /// <param name="table">The <paramref name="table"/> name.</param>
        /// <param name="primaryKeyColumns">The columns used for the primary key</param>
        /// <param name="transaction">If command is to be <paramref name="transaction"/> safe you can supply the transaction object here.</param>
        public void CreatePrimaryKeyConstraint(string table, string[] primaryKeyColumns, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var sql = $"ALTER TABLE {table.MakePostgreSqlSafe()} ADD PRIMARY KEY ({string.Join(", ", primaryKeyColumns.Select(x => x.MakePostgreSqlSafe()))});";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Creating primary key {table}_pkey on table {table} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Removes the primary key constraint from the specified <see cref="PropertyInfo"/> in the specified <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to remove the primary key from.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeletePrimaryKeyConstraint(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            this.DeletePrimaryKeyConstraint(thing.TableName, transaction);
        }

        /// <summary>
        /// Removes the primary key constraint from the specified <paramref name="table"/>
        /// </summary>
        /// <param name="table">The table name.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeletePrimaryKeyConstraint(string table, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var sql = $"ALTER TABLE {table.MakePostgreSqlSafe()} DROP CONSTRAINT {(table + "_pkey").MakePostgreSqlSafe()};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Dropping primary key {table}_pkey on table {table} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Creates a foreign key constraint between the column <see aref="fromProperty"/> in <see aref="fromThing"/> table and the <see aref="toProperty"/>
        /// in the <see aref="toThing"/> table.
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toThing">The table that contains the properties that is constrained to.</param>
        /// <param name="onDelete">The behavior on delete</param>
        /// <param name="deferred">A value indicating whether the foreign key constraint should be deferrable.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateForeignKeyConstraint(
            PropertyInfo fromProperty,
            IEntityObject fromThing,
            PropertyInfo toProperty,
            IEntityObject toThing,
            FkDeleteBehaviorKind onDelete = FkDeleteBehaviorKind.NO_ACTION,
            bool deferred = true,
            object transaction = null)
        {
            var fromPropertyName = EntityHelper.GetColumnNameFromProperty(fromProperty);
            var toPropertyName = EntityHelper.GetColumnNameFromProperty(toProperty);

            this.CreateForeignKeyConstraint(fromPropertyName, fromThing.TableName, toPropertyName, toThing.TableName, onDelete, deferred, transaction);
        }

        /// <summary>
        /// Creates a foreign key constraint between the column in a table and another column from another table
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromTable">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toTable">The table that contains the properties that is constrained to.</param>
        /// <param name="onDelete">The behavior on delete</param>
        /// <param name="deferred">A value indicating whether the foreign key constraint should be deferrable.</param>
        /// <param name="transaction">If command is to be <paramref name="transaction"/> safe you can supply the transaction object here.</param>
        public void CreateForeignKeyConstraint(string fromProperty, string fromTable, string toProperty, string toTable, FkDeleteBehaviorKind onDelete = FkDeleteBehaviorKind.NO_ACTION, bool deferred = true, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);
            var constraintName = this.CreateForeignKeyConstraintName(fromProperty, fromTable, toProperty, toTable);

            var onDeleteBehavior = string.Empty;
            switch (onDelete)
            {
                case FkDeleteBehaviorKind.CASCADE:
                    onDeleteBehavior = " ON DELETE CASCADE";
                    break;
                case FkDeleteBehaviorKind.RESTRICT:
                    onDeleteBehavior = " ON DELETE RESTRICT";
                    break;
                case FkDeleteBehaviorKind.SET_NULL:
                    onDeleteBehavior = " ON DELETE SET NULL";
                    break;
                case FkDeleteBehaviorKind.SET_DEFAULT:
                    onDeleteBehavior = " ON DELETE SET DEFAULT";
                    break;
            }

            var deferrable = string.Empty;

            if (deferred)
            {
                deferrable = " DEFERRABLE INITIALLY DEFERRED";
            }

            var sql = $"ALTER TABLE {fromTable.MakePostgreSqlSafe()} ADD CONSTRAINT {constraintName} FOREIGN KEY ({fromProperty.MakePostgreSqlSafe()}) REFERENCES {toTable.MakePostgreSqlSafe()} ({toProperty.MakePostgreSqlSafe()}){onDeleteBehavior}{deferrable};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage =
                $"Creating foreign key {constraintName} on table {fromTable} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Deletes a foreign key constraint between the column <see aref="fromProperty"/> in <see aref="fromThing"/> table and the <see aref="toProperty"/>
        /// in the <see aref="toThing"/> table.
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toThing">The table that contains the properties that is constrained to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteForeignKeyConstraint(
            PropertyInfo fromProperty,
            IEntityObject fromThing,
            PropertyInfo toProperty,
            IEntityObject toThing,
            object transaction = null)
        {
            if (fromProperty == null)
            {
                throw new ArgumentNullException(nameof(fromProperty));
            }

            if (fromThing == null)
            {
                throw new ArgumentNullException(nameof(fromThing));
            }

            if (toProperty == null)
            {
                throw new ArgumentNullException(nameof(toThing));
            }

            var constraintName = this.CreateForeignKeyConstraintName(fromProperty, fromThing, toProperty, toThing);
            this.DeleteForeignKeyConstraint(fromThing.TableName, constraintName, transaction);
        }

        /// <summary>
        /// Deletes a foreign key constraint between the column <see aref="fromProperty"/> in <see aref="fromThing"/> table and the <see aref="toProperty"/>
        /// in the <see aref="toThing"/> table.
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toThing">The table that contains the properties that is constrained to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteForeignKeyConstraint(
            string fromProperty,
            string fromThing,
            string toProperty,
            string toThing,
            object transaction = null)
        {
            var constraintName = this.CreateForeignKeyConstraintName(fromProperty, fromThing, toProperty, toThing);
            this.DeleteForeignKeyConstraint(fromThing, constraintName, transaction);
        }

        /// <summary>
        /// Deletes a foreign key constraint in a table.
        /// </summary>
        /// <param name="fromTable">The table that contains the constrained properties.</param>
        /// <param name="constraintName">The name of the constraint to drop</param>
        /// <param name="transaction">If command is to be <paramref name="transaction"/> safe you can supply the transaction object here.</param>
        public void DeleteForeignKeyConstraint(string fromTable, string constraintName, object transaction)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var sql = $"ALTER TABLE ONLY {fromTable.MakePostgreSqlSafe()} DROP CONSTRAINT {constraintName};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage =
                $"Dropping foreign key {constraintName} on table {fromTable} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Creates the foreign key constraint name from the required parts
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toThing">The table that contains the properties that is constrained to.</param>
        /// <returns>The string representation of the foreign key constraint name.</returns>
        public string CreateForeignKeyConstraintName(PropertyInfo fromProperty, IEntityObject fromThing, PropertyInfo toProperty, IEntityObject toThing)
        {
            if (fromProperty == null)
            {
                throw new ArgumentNullException(nameof(fromProperty));
            }

            if (fromThing == null)
            {
                throw new ArgumentNullException(nameof(fromThing));
            }

            if (toProperty == null)
            {
                throw new ArgumentNullException(nameof(toThing));
            }

            var tableName = fromThing.TableName;
            var tableToName = toThing.TableName;

            var fromPropertyName = EntityHelper.GetColumnNameFromProperty(fromProperty);
            var toPropertyName = EntityHelper.GetColumnNameFromProperty(toProperty);

            return this.CreateForeignKeyConstraintName(fromPropertyName, tableName, toPropertyName, tableToName);
        }

        /// <summary>
        /// Creates the foreign key constraint name from the required parts
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromTable">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toTable">The table that contains the properties that is constrained to.</param>
        /// <returns>The string representation of the foreign key constraint name.</returns>
        public string CreateForeignKeyConstraintName(string fromProperty, string fromTable, string toProperty, string toTable)
        {
            return $"{fromTable}_{fromProperty}_{toTable}_{toProperty}_fk";
        }

        /// <summary>
        /// Creates the index name from the required parts. List of columns is automatically sorted by alphabet.
        /// </summary>
        /// <param name="tableName">
        /// The table Name.
        /// </param>
        /// <param name="columnNames">
        /// The column Names.
        /// </param>
        /// <returns>
        /// The <see cref="string"/> index name.
        /// </returns>
        public string CreateIndexName(string tableName, IList<string> columnNames)
        {
            return $"idx_{tableName}_{string.Join("_", columnNames.OrderBy(c => c))}";
        }

        /// <summary>
        /// Creates a new item of the given <see cref="IEntityObject"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to be created.
        /// </typeparam>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to create a record from.
        /// </param>
        /// <param name="ignoreNull">
        /// Ignore saving of values that are explicitly set to null.
        /// </param>
        /// <param name="transaction">
        /// If command is to be transaction safe you can supply the transaction object here.
        /// </param>
        /// <returns>
        /// The created record of type <see cref="T"/>.
        /// </returns>
        public T CreateRecord<T>(T thing, bool ignoreNull = false, object transaction = null) where T : IEntityObject
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            var propertyInfos =
                thing.GetType()
                     .GetProperties()
                     .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                     .ToList();

            if (ignoreNull)
            {
                propertyInfos = propertyInfos.Where(x => x.GetValue(thing) != null).ToList(); 
            }

            var columns = propertyInfos.Select(x => EntityHelper.GetColumnNameFromProperty(x)).ToArray();

            // lists of enums should also be handled
            var values = propertyInfos.Select(x =>
            {
                if (x.PropertyType.GetTypeInfo().IsEnum)
                {
                    return x.GetValue(thing).ToString();
                }

                if (this.IsEnumerableOfEnum(x.PropertyType))
                {
                    return ((IList)x.GetValue(thing)).OfType<object>().Select(e => e.ToString()).ToList();
                }

                return x.GetValue(thing);
            }).ToArray();

            this.CreateRecord(thing.TableName, columns, values, transaction);
            return thing;
        }

        /// <summary>
        /// Create records in a <paramref name="table"/>
        /// </summary>
        /// <param name="table">The name of the table</param>
        /// <param name="columns">The name of the <paramref name="columns"/></param>
        /// <param name="values">The values to inject</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateRecord(string table, string[] columns, object[] values, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var properties = columns.Select(x => x.Insert(0, "@")).ToList();
            var propertiesAsString = string.Join(",", columns.Select(x => x.Insert(0, "@")));
            var propertyNames = string.Join(",", columns.Select(x => x.MakePostgreSqlSafe()));

            var sql = $"INSERT INTO {table.MakePostgreSqlSafe()}({propertyNames}) VALUES ({propertiesAsString});";

            var cmd = new NpgsqlCommand(sql, con, tran);

            for (var i = 0; i < properties.Count; i++)
            {
                if (values[i] != null)
                {
                    if (values[i] is DateTime)
                    {
                        cmd.Parameters.AddWithValue(properties[i], NpgsqlDbType.Timestamp, values[i]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(properties[i], values[i]);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue(properties[i], DBNull.Value);
                }
            }

            var errorMessage =
                $"Creating record on table {table} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Updates the record of the given<see cref="IEntityObject"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the object to update.
        /// </typeparam>
        /// <param name="thing">
        /// The<see cref="IEntityObject"/>to update the record of.
        /// </param>
        /// <param name="ignoreNull">
        /// Ignore saving of values that are explicitly set to null.
        /// </param>
        /// <param name="transaction">
        /// If command is to be transaction safe you can supply the transaction object here.
        /// </param>
        /// <returns>
        /// The updated record of type<see cref="T"/>.
        /// </returns>
        public T UpdateRecord<T>(T thing, bool ignoreNull = false, object transaction = null) where T : IEntityObject
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var propertyInfos =
                thing.GetType()
                     .GetProperties()
                     .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                     .ToList();

            var properties = propertyInfos.Select(x => EntityHelper.GetColumnNameFromProperty(x).Insert(0, "@")).ToList();

            if (ignoreNull)
            {
                propertyInfos = propertyInfos.Where(x => x.GetValue(thing) != null).ToList(); 
            }

            var values = propertyInfos.Select(x => x.GetValue(thing)).ToList();
            var propertiesAsString = string.Join(
                ", ",
                propertyInfos.Select(
                    x =>
                        $"{EntityHelper.GetColumnNameFromProperty(x).MakePostgreSqlSafe()} = {EntityHelper.GetColumnNameFromProperty(x).Insert(0, "@")}"));

            var primaryKeyProperty = thing.GetType().GetProperty(thing.PrimaryKey);
            var name = EntityHelper.GetColumnNameFromProperty(primaryKeyProperty);
            var parameter = name.Insert(0, "@");

            var sql = $"UPDATE {thing.TableName.MakePostgreSqlSafe()} SET {propertiesAsString} WHERE {name.MakePostgreSqlSafe()} = {parameter};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            for (var i = 0; i < properties.Count; i++)
            {
                if (values[i] != null)
                {
                    if (values[i] is DateTime)
                    {
                        cmd.Parameters.AddWithValue(properties[i], NpgsqlDbType.Timestamp, values[i]);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue(properties[i], propertyInfos[i].PropertyType.GetTypeInfo().IsEnum ? values[i].ToString() : values[i]);
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue(properties[i], DBNull.Value);
                }
            }

            var errorMessage =
                $"Updating record {name} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);

            return thing;
        }

        /// <summary>
        /// Deletes the record of the given <see cref="IEntityObject"/> instance.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to delete the record of.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteRecord(IEntityObject thing, object transaction = null)
        {
            if (thing == null)
            {
                throw new ArgumentNullException(nameof(thing));
            }

            var primaryKeyProperty = thing.GetType().GetProperty(thing.PrimaryKey);
            var name = EntityHelper.GetColumnNameFromProperty(primaryKeyProperty);

            if(primaryKeyProperty == null)
            {
                throw new InvalidOperationException("Property indicated as prinary key does not exist.");
                
            }

            var value = primaryKeyProperty.GetValue(thing);
            
            this.DeleteRecord(thing.TableName, name, value, transaction);
        }

        /// <summary>
        /// Deletes the record of the given <paramref name="table"/>
        /// </summary>
        /// <param name="table">The table to delete the record of.</param>
        /// <param name="pkey">The primary key of the table</param>
        /// <param name="value">The value to delete</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteRecord(string table, string pkey, object value, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var parameter = pkey.Insert(0, "@");

            var sql = $"DELETE FROM {table.MakePostgreSqlSafe()} WHERE {pkey.MakePostgreSqlSafe()} = {parameter};";

            var cmd = new NpgsqlCommand(sql, con, tran);
            cmd.Parameters.AddWithValue(parameter, value);

            var errorMessage =
                $"Deleting record {pkey} on table {table} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Read all records of a given <see cref="IEntityObject"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property to order records by.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// A list of <see cref="IEntityObject"/> objects that were returned from the database.
        /// </returns>
        public List<T> ReadRecords<T>(int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject
        {
            var result = new List<T>();

            var clone = Activator.CreateInstance<T>();

            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;
            var sql = $"SELECT * FROM {clone.TableName.MakePostgreSqlSafe()}";

            if (orderBy != null)
            {
                var orderColumn = EntityHelper.GetColumnNameFromProperty(orderBy);

                var desc = string.Empty;

                if (orderDescending)
                {
                    desc = " DESC";
                }

                sql += $" ORDER BY {orderColumn.MakePostgreSqlSafe()}{desc}";
            }

            if (limit != null && limit > 0)
            {
                sql += $" LIMIT {limit}";
            }

            if (offset != null && offset > 0)
            {
                sql += $" OFFSET {offset}";
            }

            sql += ";";

            var cmd = new NpgsqlCommand(sql, con);

            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance<T>();

                    foreach (
                        var property in
                            obj.GetType()
                               .GetProperties()
                               .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                               .ToList())
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        if (converter is CollectionConverter)
                        {
                            // this outputs an array
                            var rawValue = dr[EntityHelper.GetColumnNameFromProperty(property)];

                            if (property.PropertyType.IsArray)
                            {
                                property.SetValue(obj, rawValue);
                            }
                            else
                            {
                                var value = Activator.CreateInstance(property.PropertyType, rawValue);

                                property.SetValue(obj, value);
                            }
                        }
                        else
                        {
                            var rawValue =
                                converter.ConvertFrom(dr[EntityHelper.GetColumnNameFromProperty(property)].ToString());

                            property.SetValue(obj, rawValue);
                        }
                    }

                    result.Add(obj);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Getting all records from table {clone.TableName} failed. Error: {ex.Message}");
            }
            finally
            {
                con?.Close();
            }

            return result;
        }

        /// <summary>
        /// Read all records of a given <see cref="IEntityObject"/> where conditions are met.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="fromProperty">The property to check.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <param name="val">The value to check against.</param>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property by which to sort.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// A list of <see cref="IEntityObject"/> objects that were returned from the database.
        /// </returns>
        public List<T> ReadRecordsWhere<T>(PropertyInfo fromProperty, string comparer, object val, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject
        {
            var result = new List<T>();

            var clone = Activator.CreateInstance<T>();

            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;
            var sql = $"SELECT * FROM {clone.TableName.MakePostgreSqlSafe()} WHERE {EntityHelper.GetColumnNameFromProperty(fromProperty).MakePostgreSqlSafe()} {comparer} @val";

            if (orderBy != null)
            {
                var orderColumn = EntityHelper.GetColumnNameFromProperty(orderBy);

                var desc = string.Empty;

                if (orderDescending)
                {
                    desc = " DESC";
                }

                sql += $" ORDER BY {orderColumn.MakePostgreSqlSafe()}{desc}";
            }

            if (limit != null && limit > 0)
            {
                sql += $" LIMIT {limit}";
            }

            if (offset != null && offset > 0)
            {
                sql += $" OFFSET {offset}";
            }

            sql += ";";

            var cmd = new NpgsqlCommand(sql, con);

            if (val is DateTime)
            {
                cmd.Parameters.AddWithValue("val", NpgsqlDbType.Timestamp, ((DateTime)val).ToUniversalTime());
            }
            else
            {
                cmd.Parameters.AddWithValue("val", val.GetType().GetTypeInfo().IsEnum ? val.ToString() : val);
            }
            
            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance<T>();

                    foreach (
                        var property in
                            obj.GetType()
                               .GetProperties()
                               .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                               .ToList())
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        if (converter is CollectionConverter)
                        {
                            // this outputs an array
                            var rawValue = dr[EntityHelper.GetColumnNameFromProperty(property)];

                            if (property.PropertyType.IsArray)
                            {
                                property.SetValue(obj, rawValue);
                            }
                            else
                            {
                                var value = Activator.CreateInstance(property.PropertyType, rawValue);

                                property.SetValue(obj, value);
                            }
                        }
                        else
                        {
                            var rawValue =
                                converter.ConvertFrom(dr[EntityHelper.GetColumnNameFromProperty(property)].ToString());

                            property.SetValue(obj, rawValue);
                        }
                    }

                    result.Add(obj);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Getting all records from table {clone.TableName} failed. Error: {ex.Message}");
            }
            finally
            {
                con?.Close();
            }

            return result;
        }

        /// <summary>
        /// Decompose where statements into the command.
        /// </summary>
        /// <param name="con">
        /// The connection.
        /// </param>
        /// <param name="sql">
        /// The sql.
        /// </param>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property by which to sort.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// The <see cref="NpgsqlCommand"/>.
        /// </returns>
        public NpgsqlCommand DecomposeWhereStatements(ref NpgsqlConnection con, ref string sql, List<IWhereQueryContainer> queries, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false)
        {
            if (queries.Any())
            {
                sql += " WHERE ";

                var queryList = new List<string>();

                foreach (var query in queries)
                {
                    queryList.Add(query.GetSqlString());
                }

                sql += string.Join(" AND ", queryList);
            }

            if (orderBy != null)
            {
                var orderColumn = EntityHelper.GetColumnNameFromProperty(orderBy);

                var desc = string.Empty;

                if (orderDescending)
                {
                    desc = " DESC";
                }

                sql += $" ORDER BY {orderColumn.MakePostgreSqlSafe()}{desc}";
            }

            if (limit != null && limit > 0)
            {
                sql += $" LIMIT {limit}";
            }

            if (offset != null && offset > 0)
            {
                sql += $" OFFSET {offset}";
            }

            sql += ";";

            var cmd = new NpgsqlCommand(sql, con);

            foreach (var query in queries)
            {
                query.InsertParameterValues(ref cmd);
            }

            return cmd;
        }

        /// <summary>
        /// Read all records of a given <see cref="IEntityObject"/> where conditions are met.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property by which to sort.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// A list of <see cref="IEntityObject"/> objects that were returned from the database.
        /// </returns>
        public List<T> ReadRecordsWhere<T>(IEnumerable<IWhereQueryContainer> queries, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject
        {
            var result = new List<T>();

            var clone = Activator.CreateInstance<T>();

            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;
            var sql = $"SELECT * FROM {clone.TableName.MakePostgreSqlSafe()}";

            var cmd = this.DecomposeWhereStatements(ref con, ref sql, queries.ToList(), limit, offset, orderBy, orderDescending);

            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance<T>();

                    foreach (
                        var property in
                            obj.GetType()
                               .GetProperties()
                               .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                               .ToList())
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        if (converter is CollectionConverter)
                        {
                            // this outputs an array
                            var rawValue = dr[EntityHelper.GetColumnNameFromProperty(property)];

                            if (property.PropertyType.IsArray)
                            {
                                property.SetValue(obj, rawValue);
                            }
                            else
                            {
                                var value = Activator.CreateInstance(property.PropertyType, rawValue);

                                property.SetValue(obj, value);
                            }
                        }
                        else
                        {
                            var rawValue =
                                converter.ConvertFrom(dr[EntityHelper.GetColumnNameFromProperty(property)].ToString());

                            property.SetValue(obj, rawValue);
                        }
                    }

                    result.Add(obj);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Getting filtered records from table {clone.TableName} failed. Error: {ex.Message}");
            }
            finally
            {
                con?.Close();
            }

            return result;
        }

        /// <summary>
        /// Counts all records of a given <see cref="IEntityObject"/> where conditions are met.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <returns>
        /// The count of rows in the query. -1 if something went wrong.
        /// </returns>
        public long CountRecordsWhere<T>(IEnumerable<IWhereQueryContainer> queries) where T : IEntityObject
        {
            var result = -1L;

            var clone = Activator.CreateInstance<T>();

            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;
            var sql = $"SELECT count(*) FROM {clone.TableName.MakePostgreSqlSafe()}";

            var cmd = this.DecomposeWhereStatements(ref con, ref sql, queries.ToList());

            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = long.Parse(dr[0].ToString());
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Getting count of filtered records from table {clone.TableName} failed. Error: {ex.Message}");
            }
            finally
            {
                con?.Close();
            }

            return result;
        }

        /// <summary>
        /// Returns a very close estimate of the number of records of a table based on thing.
        /// </summary>
        /// <typeparam name="T">The type of thing.</typeparam>
        /// <param name="thing">The thing from which to read the number of records of.</param>
        /// <returns>The (approximate) number of records in the table.</returns>
        public long CountRecords<T>(T thing) where T : IEntityObject
        {
            return this.CountRecords(thing.TableName);
        }

        /// <summary>
        /// Returns a very close estimate of the number of records of a table based on table name.
        /// </summary>
        /// <param name="tableName">The name of the table from which to read the number of records of.</param>
        /// <returns>The (approximate) number of records in the table.</returns>
        public long CountRecords(string tableName)
        {
            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;

            var strippedTableName = tableName.Replace("\"", string.Empty);

            var sql = "SELECT reltuples::bigint AS count";
            sql += " FROM pg_class";
            sql += $" WHERE oid = '{strippedTableName}'::regclass;";

            var result = -1L;

            var cmd = new NpgsqlCommand(sql, con);

            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    result = long.Parse(dr[0].ToString());
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Getting count of records from table {strippedTableName} failed. Error: {ex.Message}");
            }
            finally
            {
                con?.Close();
            }

            return result;
        }

        /// <summary>
        /// Retrieve all <see cref="Type"/>s from a generic collection <see cref="Type"/>.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> of the generic collection.</param>
        /// <returns>The collection of <see cref="Type"/>s present in the generic collection.</returns>
        public IEnumerable<Type> GetEnumerableTypes(Type type)
        {
            if (type.GetTypeInfo().IsInterface)
            {
                if (type.GetTypeInfo().IsGenericType
                    && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    yield return type.GetGenericArguments()[0];
                }
            }

            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.GetTypeInfo().IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    yield return intType.GetGenericArguments()[0];
                }
            }
        }

        /// <summary>
        /// Determines whether the provided <see cref="Type"/> is a IEnumerable with a Enum generic argument.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to test.</param>
        /// <returns>True if the supplied <see cref="Type"/> is an IEnumerable of Enum.</returns>
        public bool IsEnumerableOfEnum(Type type)
        {
            return this.GetEnumerableTypes(type).Any(t => t.GetTypeInfo().IsEnum);
        }

        /// <summary>
        /// Reads a record where the primary key matches the supplied <paramref name="id"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEntityObject"/> instance</typeparam>
        /// <param name="id">The id to use to look up the primary key,</param>
        /// <exception cref="InvalidDataException"></exception>
        /// <returns>The instance of the object if it exists and exists solely. If more matches exist then the first one or nothing found then null is returned.</returns>
        public T ReadRecord<T>(object id) where T : IEntityObject
        {
            var result = new List<T>();

            var clone = Activator.CreateInstance<T>();

            var con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;
            var sql = $"SELECT * FROM {clone.TableName.MakePostgreSqlSafe()} WHERE {EntityHelper.GetColumnNameFromProperty(clone.GetType().GetProperty(clone.PrimaryKey)).MakePostgreSqlSafe()} = '{id}';";

            var cmd = new NpgsqlCommand(sql, con);

            try
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance<T>();

                    foreach (
                        var property in
                            obj.GetType()
                               .GetProperties()
                               .Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(DbIgnoreAttribute)))
                               .ToList())
                    {
                        var converter = TypeDescriptor.GetConverter(property.PropertyType);
                        if (converter is CollectionConverter)
                        {
                            // this outputs an array
                            var rawValue = dr[EntityHelper.GetColumnNameFromProperty(property)];

                            if (property.PropertyType.IsArray)
                            {
                                property.SetValue(obj, rawValue);
                            }
                            else
                            {
                                var value = Activator.CreateInstance(property.PropertyType, rawValue);

                                property.SetValue(obj, value);
                            }
                        }
                        else
                        {
                            var rawValue =
                                converter.ConvertFrom(dr[EntityHelper.GetColumnNameFromProperty(property)].ToString());

                            property.SetValue(obj, rawValue);
                        }
                    }

                    result.Add(obj);
                }

                dr.Close();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException(
                    $"Getting record {id} from table {clone.TableName} failed. Error: {ex.Message}");
            }
            finally
            {
                con?.Close();
            }

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Gets the database <paramref name="type"/> as a string literal based on the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The properties.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// An exception is thrown if the supplied properties does not have a database equivalent.
        /// </exception>
        public static string GetTypeStringFromProperty(Type type)
        {
            /*
                        Postgresql  NpgsqlDbType System.DbType Enum .Net System Type
                        ----------  ------------ ------------------ ----------------
                        +int8        Bigint       Int64              Int64
                        +bool        Boolean      Boolean            Boolean
                        -bytea       Bytea        Binary             Byte[]
                        +date        Date         Date               DateTime
                        +float8      Double       Double             Double
                        +int4        Integer      Int32              Int32
                        -money       Money        Decimal            Decimal
                        +numeric     Numeric      Decimal            Decimal
                        +float4      Real         Single             Single
                        +int2        Smallint     Int16              Int16
                        +text        Text         String             String
                        -time        Time         Time               DateTime
                        -timetz      Time         Time               DateTime
                        +timestamp   Timestamp    DateTime           DateTime
                        -timestamptz TimestampTZ  DateTime           DateTime
                        +interval    Interval     Object             TimeSpan
                        -varchar     Varchar      String             String
                        -inet        Inet         Object             IPAddress
                        -bit         Bit          Boolean            Boolean
                        +uuid        Uuid         Guid               Guid
                        -array       Array        Object             Array
            
                        */

            if (type.GetTypeInfo().IsEnum)
            {
                return "varchar";
            }
            else if (type == typeof(long))
            {
                return "int8";
            }
            else if (type == typeof(int))
            {
                return "int4";
            }
            else if (type == typeof(short))
            {
                return "int2";
            }
            else if (type == typeof(long?))
            {
                return "int8";
            }
            else if (type == typeof(int?))
            {
                return "int4";
            }
            else if (type == typeof(short?))
            {
                return "int2";
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return "numeric";
            }
            else if (type == typeof(string))
            {
                return "text";
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                return "bool";
            }
            else if (type == typeof(DateTime))
            {
                return "timestamp";
            }
            else if (type == typeof(DateTime?))
            {
                return "timestamp";
            }
            else if (type == typeof(TimeSpan) || type == typeof(TimeSpan?))
            {
                return "interval";
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                return "float4";
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                return "float8";
            }
            else if (type == typeof(Guid))
            {
                return "uuid";
            }
            else if (type == typeof(Guid?))
            {
                return "uuid";
            }
            else if (
                type.GetInterfaces()
                    .Any(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                var propertyType =
                    type.GetInterfaces()
                        .Where(t => t.GetTypeInfo().IsGenericType &&
                                    t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                        .Select(t => t.GetGenericArguments()[0])
                        .FirstOrDefault();
                return $"{GetTypeStringFromProperty(propertyType)}[]";
            }
            else
            {
                throw new InvalidDataException(
                    string.Format("The type is not supported by this DatabaseConnector. Type: {0}", type.Name));
            }
        }

        /// <summary>
        /// Commits <paramref name="transaction"/>ransaction. In this implementation the object will be recast to <see cref="NpgsqlTransaction"/>
        /// and will throw an exception is any other type is received.
        /// </summary>
        /// <param name="transaction">The transaction object to be committed.</param>
        public void CommitTransaction(object transaction)
        {
            var castTransaction = transaction as NpgsqlTransaction;

            if (castTransaction != null)
            {
                var connection = castTransaction.Connection;

                // commit transaction
                castTransaction.Commit();
                connection?.Close();
            }
            else
            {
                throw new ArgumentException("The provided transaction is not of the right type.", nameof(transaction));
            }
        }

        /// <summary>
        /// Creates the transaction (and implicitly the associated connection object).
        /// </summary>
        /// <param name="credentials">The <see cref="DatabaseCredentials"/> used to connect to the database.</param>
        /// <returns>The transaction object</returns>
        public object CreateTransaction(DatabaseCredentials credentials)
        {
            string connstring = $"Server={credentials.Host};Port={credentials.Port};User Id={credentials.Username};Password={credentials.Password};Database={credentials.DatabaseName};";

            // Making connection with Npgsql provider
            var con = new NpgsqlConnection(connstring);

            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                throw new AuthenticationException($"The connection could not be opened with the provided credentials. Error: {ex.Message}");
            }

            return con.BeginTransaction();
        }

        /// <summary>
        /// Creates a connection.
        /// </summary>
        /// <param name="credentials">The <see cref="DatabaseCredentials"/> used to connect to the database.</param>
        /// <returns>The connection object.</returns>
        public object CreateConnection(DatabaseCredentials credentials)
        {
            var connstring = $"Server={credentials.Host};Port={credentials.Port};User Id={credentials.Username};Password={credentials.Password};Database={credentials.DatabaseName};";

            // Making connection with Npgsql provider
            var con = new NpgsqlConnection(connstring);

            try
            {
                con.Open();
            }
            catch (Exception ex)
            {
                throw new AuthenticationException($"The connection could not be opened with the provided credentials. Error: {ex.Message}");
            }

            return con;
        }

        /// <summary>
        /// Creates a uniqueness constraint on columns represented by <see cref="PropertyInfo"/>s in
        /// object <see cref="IEntityObject"/>. Multiple columns means a combined constraint.
        /// </summary>
        /// <param name="properties">The array of <see cref="PropertyInfo"/> representing the columns that should have thew constraint applied.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> representing the table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateUniquenessConstraint(PropertyInfo[] properties, IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // if list is null or empty then throw
            if (properties == null || !properties.Any())
            {
                throw new ArgumentException("The provided list of properties is null or empty. The uniqueness constraint cannot be made.");
            }

            var constraintName = this.CreateUniquenessConstraintName(thing.TableName, properties);
            var columnList = string.Join(", ", properties.Select(x => EntityHelper.GetColumnNameFromProperty(x).MakePostgreSqlSafe()));

            var sql = $"ALTER TABLE {thing.TableName.MakePostgreSqlSafe()} ADD CONSTRAINT {constraintName} UNIQUE ({columnList});";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Adding uniqueness constraint on column(s) {columnList} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Deletes a uniqueness constraint on columns represented by <see cref="PropertyInfo"/>s in
        /// object <see cref="IEntityObject"/>. Multiple columns means a combined constraint.
        /// </summary>
        /// <param name="properties">The array of <see cref="PropertyInfo"/> representing the columns that should have thew constraint applied.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> represneting the table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteUniquenessConstraint(PropertyInfo[] properties, IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // if list is null or empty then throw
            if (properties == null || !properties.Any())
            {
                throw new ArgumentException("The provided list of properties is null or empty. The uniqueness constraint cannot be made.");
            }

            var constraintName = this.CreateUniquenessConstraintName(thing.TableName, properties);

            var sql = $"ALTER TABLE {thing.TableName.MakePostgreSqlSafe()} DROP CONSTRAINT {constraintName};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage =
                $"Removing uniqueness constraint {constraintName} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Creates the uniqueness constraint name from the required parts
        /// </summary>
        /// <param name="tableName">The table name.</param>
        /// <param name="properties">The list of properties to impose the uniqueness on.</param>
        /// <returns>The string representation of the uniqueness constraint name.</returns>
        private string CreateUniquenessConstraintName(string tableName, IEnumerable<PropertyInfo> properties)
        {
            return string.Format("{0}_{1}_uc", tableName, string.Join("_", properties.Select(x => EntityHelper.GetColumnNameFromProperty(x))));
        }

        /// <summary>
        /// Returns the correct connection and transaction objects and whether the transaction safety is on.
        /// </summary>
        /// <param name="transaction">The initial transaction object.</param>
        /// <param name="con">The connection object to be returned.</param>
        /// <param name="tran">The final transaction object</param>
        /// <returns>True if transaction safety is turned on.</returns>
        private bool ManageTransaction(object transaction, out NpgsqlConnection con, out NpgsqlTransaction tran)
        {
            var transactionSafe = transaction != null;

            if (transactionSafe)
            {
                tran = transaction as NpgsqlTransaction;
                if (tran == null)
                {
                    throw new Exception("The supplied transaction object is not of the correct type.");
                }

                con = tran.Connection;
            }
            else
            {
                con = this.CreateConnection(DatabaseSession.Instance.Credentials) as NpgsqlConnection;
                tran = con?.BeginTransaction();
            }

            return transactionSafe;
        }

        /// <summary>
        /// Creates a not null constraint on a given <see cref="PropertyInfo"/> in object <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to apply the Not Null constraint to.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the constraint to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateNotNullConstraint(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // if list is null or empty then throw
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var columnName = EntityHelper.GetColumnNameFromProperty(property);

            var sql = $"ALTER TABLE {thing.TableName.MakePostgreSqlSafe()} ALTER COLUMN {columnName.MakePostgreSqlSafe()} SET NOT NULL;";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage =
                $"Adding not null constraint on column {columnName} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Deletes a not null constraint on a given <see cref="PropertyInfo"/> in object <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to delete the Not Null constraint from.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the constraint to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteNotNullConstraint(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // if list is null or empty then throw
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var columnName = EntityHelper.GetColumnNameFromProperty(property);

            var sql = $"ALTER TABLE {thing.TableName.MakePostgreSqlSafe()} ALTER COLUMN {columnName.MakePostgreSqlSafe()} DROP NOT NULL;";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Deleting not null constraint on column {columnName} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Executes the command with a non query.
        /// </summary>
        /// <param name="command">The <see cref="NpgsqlCommand"/> to execute.</param>
        /// <param name="errorMessage">The error message to display in case of failure.</param>
        /// <param name="transactionSafe">Boolean to indicate whether transaction safety is on.</param>
        /// <param name="transaction">The transaction object.</param>
        /// <param name="connection">The connection object.</param>
        /// <exception cref="InvalidDataException"></exception>
        public static void ExecuteNonQuery(NpgsqlCommand command, string errorMessage, bool transactionSafe, NpgsqlTransaction transaction, NpgsqlConnection connection)
        {
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"{errorMessage} Error: {ex.Message}");
            }
            finally
            {
                if (!transactionSafe)
                {
                    transaction.Commit();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Drops the default value of a column.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteDefault(PropertyInfo property, IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // if list is null or empty then throw
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var columnName = EntityHelper.GetColumnNameFromProperty(property);

            var sql = $"ALTER TABLE {thing.TableName.MakePostgreSqlSafe()} ALTER COLUMN {columnName.MakePostgreSqlSafe()} DROP DEFAULT;";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Deleting default on column {columnName} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Sets the default value of a column.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="value">The value to set the default to.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void SetDefault(PropertyInfo property, object value, IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            // if list is null or empty then throw
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var columnName = EntityHelper.GetColumnNameFromProperty(property);

            var sql = $"ALTER TABLE {thing.TableName.MakePostgreSqlSafe()} ALTER COLUMN {columnName.MakePostgreSqlSafe()} SET DEFAULT '{value}';";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Creating default ({value}) on column {columnName} on table {thing.TableName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Drops all tables in the public schema. IMPORTANT!! Very dangerous function and should not be used eva!
        /// </summary>
        /// <param name="schemaName">The name of the schema to drop all tables on.</param>
        public void DropAllTables(string schemaName)
        {
            // this function always creates own transaction.
            var transaction = DatabaseSession.Instance.CreateTransaction();

            DatabaseSession.Instance.Connector.AlterSchemaOwner(schemaName, transaction);
            DatabaseSession.Instance.Connector.DropSchema(schemaName, transaction);
            DatabaseSession.Instance.Connector.CreateSchema(schemaName, transaction);

            DatabaseSession.Instance.CommitTransaction(transaction);
        }

        /// <summary>
        /// Alters the schema owner to current user.
        /// </summary>
        /// <param name="schemaName">The name of the schema to drop.</param>
        /// <param name="transaction">The transaction object.</param>
        public void AlterSchemaOwner(string schemaName, object transaction)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(null, out con, out tran);

            // if schemaname is empty
            if (string.IsNullOrWhiteSpace(schemaName))
            {
                throw new ArgumentException("Schema name must not be empty!", nameof(schemaName));
            }

            var sql = $"ALTER SCHEMA {schemaName.MakePostgreSqlSafe()} OWNER to {DatabaseSession.Instance.Credentials.Username};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Altering schema {schemaName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Drops a schema. IMPORTANT!! Very dangerous function and should not be used eva!
        /// </summary>
        /// <param name="schemaName">The name of the schema to drop.</param>
        /// <param name="transaction">The transaction object.</param>
        public void DropSchema(string schemaName, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(null, out con, out tran);

            // if schemaname is empty
            if (string.IsNullOrWhiteSpace(schemaName))
            {
                throw new ArgumentException("Schema name must not be empty!", nameof(schemaName));
            }

            var sql = $"ALTER SCHEMA {schemaName.MakePostgreSqlSafe()} OWNER to {DatabaseSession.Instance.Credentials.Username};";
            sql += $"DROP SCHEMA {schemaName.MakePostgreSqlSafe()} CASCADE;";
            
            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = string.Format("Dropping schema {0} failed.", schemaName);

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Creates a schema.
        /// </summary>
        /// <param name="schemaName">The name of the schema to create.</param>
        /// <param name="transaction">The transaction object.</param>
        public void CreateSchema(string schemaName, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(null, out con, out tran);

            // if schemaname is empty
            if (string.IsNullOrWhiteSpace(schemaName))
            {
                throw new ArgumentException("Schema name must not be empty!", nameof(schemaName));
            }

            var sql = string.Format("CREATE SCHEMA {0};", schemaName.MakePostgreSqlSafe());
            sql += string.Format("ALTER SCHEMA {0} OWNER to {1};", schemaName.MakePostgreSqlSafe(), DatabaseSession.Instance.Credentials.Username);
            sql += string.Format("GRANT ALL ON SCHEMA {0} TO public;", schemaName.MakePostgreSqlSafe());
            sql += string.Format("COMMENT ON SCHEMA {0} IS 'standard public schema';", schemaName.MakePostgreSqlSafe());

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = string.Format("Creating schema {0} failed.", schemaName);

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Creates a replication function and trigger on a given table. The given colums will be added to the replication table on insert and cleaned when deleted.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the function and trigger to.</param>
        /// <param name="replicationTable">The table which to replicate to.</param>
        /// <param name="idColumnName">The name of the id column to be used for replication. Must be the same between target and replication table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateReplicateFunctionAndTrigger(IEntityObject thing, string replicationTable, string idColumnName, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var functionName = $"trigger_on_{thing.TableName}";
            
            var columnListNew = $"NEW.{idColumnName},'{thing.TableName}',NEW.modifiedon";         
            var columnListOld = $"{idColumnName}=OLD.{idColumnName}";

            var sql = $"CREATE OR REPLACE FUNCTION {functionName}() RETURNS TRIGGER";
            sql += " AS $function$";
            sql += " BEGIN";
            sql += " IF (TG_OP = 'INSERT') THEN";
            sql += " NEW.createdon := utc_now();";
            sql += " NEW.modifiedon := utc_now();";
            sql += $" INSERT INTO {replicationTable.MakePostgreSqlSafe()} SELECT {columnListNew}; RETURN NEW;";
            sql += " ELSEIF (TG_OP = 'DELETE') THEN";
            sql += $" DELETE FROM {replicationTable.MakePostgreSqlSafe()} WHERE {columnListOld}; RETURN OLD;";
            sql += " ELSEIF (TG_OP = 'UPDATE') THEN";
            sql += " NEW.modifiedon := utc_now();";
            sql += $" UPDATE {replicationTable.MakePostgreSqlSafe()} SET modifiedon=NEW.modifiedon WHERE {columnListOld}; RETURN NEW;";
            sql += " END IF;";
            sql += " END; $function$ LANGUAGE plpgsql VOLATILE COST 100;";

            sql += $" CREATE TRIGGER {functionName} BEFORE INSERT OR UPDATE OR DELETE ON {thing.TableName.MakePostgreSqlSafe()} FOR EACH ROW EXECUTE PROCEDURE {functionName}();";
            
            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Adding replication function {functionName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Create a function and a trigger for a specific table
        /// </summary>
        /// <param name="table">The table</param>
        /// <param name="functionName">The function name</param>
        /// <param name="functionBody">The function body</param>
        /// <param name="isBefore">Whether the trigger is happening before or after an operation</param>
        /// <param name="onInsert">Whether the trigger is executed on insert</param>
        /// <param name="onUpdate">Whether the trigger is executed on update</param>
        /// <param name="onDelete">Whether the trigger is executed on delete</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateFunctionAndTrigger(string table, string functionName, string functionBody, bool isBefore, bool onInsert, bool onUpdate, bool onDelete, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);
            var onOperations = new List<string>();

            if (onInsert)
            {
                onOperations.Add("INSERT");
            }

            if (onUpdate)
            {
                onOperations.Add("UPDATE");
            }

            if (onDelete)
            {
                onOperations.Add("DELETE");
            }

            var sql = $"CREATE OR REPLACE FUNCTION {functionName}() RETURNS TRIGGER";
            sql += " AS $function$";
            sql += " BEGIN ";
            sql += functionBody;
            sql += " END; $function$ LANGUAGE plpgsql VOLATILE COST 100;";

            sql += $" CREATE TRIGGER {functionName} {(isBefore ? "BEFORE" : "AFTER")} {string.Join(" OR ", onOperations)} ON {table.MakePostgreSqlSafe()} FOR EACH ROW EXECUTE PROCEDURE {functionName}();";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Adding function and trigger {functionName} failed.";
            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Create a function
        /// </summary>
        /// <param name="functionName">The function name</param>
        /// <param name="functionBody">The function body</param>
        /// <param name="returnType">The function return type</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateFunction(string functionName, string functionBody, string returnType, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);
           
            var sql = $"CREATE OR REPLACE FUNCTION {functionName}() RETURNS {returnType}";
            sql += " AS $function$";
            sql += " BEGIN ";
            sql += functionBody;
            sql += " END; $function$ LANGUAGE plpgsql VOLATILE COST 100;";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Adding function {functionName} failed.";
            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Delete a function
        /// </summary>
        /// <param name="functionName">The function name</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteFunction(string functionName, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var sql = $"DROP FUNCTION {functionName}();";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Deleting function {functionName} failed.";
            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Drops a replication function and trigger on a given table.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the function and trigger to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteReplicateFunctionAndTrigger(IEntityObject thing, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var functionName = $"trigger_on_{thing.TableName}";
            
            var sql = $"DROP TRIGGER {functionName} ON {thing.TableName.MakePostgreSqlSafe()};";

            sql += $" DROP FUNCTION {functionName}();";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Dropping replication function {functionName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Creates an index on the specified columns in the specified thing.
        /// </summary>
        /// <param name="thing">
        /// The thing.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateIndex(IEntityObject thing, List<PropertyInfo> columns, object transaction = null)
        {
            var tableName = thing.TableName;

            var columnNames = columns.Select(c => EntityHelper.GetColumnNameFromProperty(c)).ToList();

            this.CreateIndex(tableName, columnNames, transaction);
        }

        /// <summary>
        /// Creates an index on the specified columns in the specified table.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="columnNames">
        /// The column names.
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void CreateIndex(string tableName, IList<string> columnNames, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var indexNameColumnsSql = string.Join(",", columnNames.Select(c => c.MakePostgreSqlSafe()));

            var indexName = this.CreateIndexName(tableName, columnNames);

            var sql = $"CREATE INDEX {indexName} ON {tableName.MakePostgreSqlSafe()}({indexNameColumnsSql});";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Creating index {indexName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }

        /// <summary>
        /// Deletes an index on the specified columns in the specified thing.
        /// </summary>
        /// <param name="thing">
        /// The thing.
        /// </param>
        /// <param name="columns">
        /// The columns.
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteIndex(IEntityObject thing, List<PropertyInfo> columns, object transaction = null)
        {
            var tableName = thing.TableName;

            var columnNames = columns.Select(c => EntityHelper.GetColumnNameFromProperty(c)).ToList();

            this.DeleteIndex(tableName, columnNames, transaction);
        }

        /// <summary>
        /// Deletes an index on the specified columns in the specified table.
        /// </summary>
        /// <param name="tableName">
        /// The table name.
        /// </param>
        /// <param name="columnNames">
        /// The column names.
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteIndex(string tableName, IList<string> columnNames, object transaction = null)
        {
            var indexName = this.CreateIndexName(tableName, columnNames);
            this.DeleteIndex(indexName, transaction);
        }

        /// <summary>
        /// Deletes an index specified by name.
        /// </summary>
        /// <param name="indexName">
        /// The name of the index.
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        public void DeleteIndex(string indexName, object transaction = null)
        {
            NpgsqlConnection con;
            NpgsqlTransaction tran;

            var transactionSafe = this.ManageTransaction(transaction, out con, out tran);

            var sql = $"DROP INDEX {indexName};";

            var cmd = new NpgsqlCommand(sql, con, tran);

            var errorMessage = $"Delete index {indexName} failed.";

            ExecuteNonQuery(cmd, errorMessage, transactionSafe, tran, con);
        }
    }
}
