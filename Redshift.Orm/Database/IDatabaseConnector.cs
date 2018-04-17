#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDatabaseConnector.cs" company="RHEA System S.A.">
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

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Redshift.Orm.Tests")]

namespace Redshift.Orm.Database
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using EntityObject;

    /// <summary>
    /// Public interface for a database connector.
    /// </summary>
    public interface IDatabaseConnector
    {
        /// <summary>
        /// Gets the <see cref="ConnectorType"/> which represents the type of database this is.
        /// </summary>
        ConnectorType ConnectorType { get; }

        /// <summary>
        /// Creates a connection object.
        /// </summary>
        /// <param name="credentials">The <see cref="DatabaseCredentials"/> used to connect to the database.</param>
        /// <returns>The connection object.</returns>
        object CreateConnection(DatabaseCredentials credentials);

        /// <summary>
        /// Creates the transaction (and implicitly the associated connection object).
        /// </summary>
        /// <param name="credentials">The <see cref="DatabaseCredentials"/> used to connect to the database.</param>
        /// <returns>The transaction object</returns>
        object CreateTransaction(DatabaseCredentials credentials);

        /// <summary>
        /// Commits the given transaction.
        /// </summary>
        /// <param name="transaction">The transaction object to be committed.</param>
        void CommitTransaction(object transaction);

        /// <summary>
        /// Creates a table from an <see cref="IEntityObject"/>
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateTable(IEntityObject thing, object transaction = null);

        /// <summary>
        /// Create a table with the provided name
        /// </summary>
        /// <param name="tableName">The table name</param>
        /// <param name="transaction">the transaction</param>
        void CreateTable(string tableName, object transaction = null);

        /// <summary>
        /// Create a table from an <see cref="IEntityObject"/> with all columns.
        /// </summary>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to be used as template
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateTableWithColumns(IEntityObject thing, object transaction = null);

        /// <summary>
        /// Create a table from an <see cref="IEntityObject"/> with all columns and a primary key.
        /// </summary>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to be used as template
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateTableWithColumnsAndPrimaryKey(IEntityObject thing, object transaction = null);

        /// <summary>
        /// Deletes a table based on an <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteTable(IEntityObject thing, object transaction = null);

        /// <summary>
        /// Deletes a table
        /// </summary>
        /// <param name="table">The name of teh table to delete</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteTable(string table, object transaction = null);

        /// <summary>
        /// Checks whether the table from a given ORM object exists in the database
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <returns>True if the table exists.</returns>
        bool CheckTableExists(IEntityObject thing);

        /// <summary>
        /// Checks whether the table with a given name exists in the database
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <returns>True if the table exists.</returns>
        bool CheckTableExists(string tableName);

        /// <summary>
        /// Checks whether the column based on <see cref="PropertyInfo"/> exists in the table of a given <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The property representing the column.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <returns>True if the column exists.</returns>
        bool CheckColumnExists(PropertyInfo property, IEntityObject thing);

        /// <summary>
        /// Checks whether the column based on name exists in the table of a given <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <returns>True if the column exists.</returns>
        bool CheckColumnExists(string columnName, IEntityObject thing);

        /// <summary>
        /// Checks whether the column based on name exists in the table of a given name.
        /// </summary>
        /// <param name="columnName">The name of the column.</param>
        /// <param name="tableName">The name of the table.</param>
        /// <returns>True if the column exists.</returns>
        bool CheckColumnExists(string columnName, string tableName);

        /// <summary>
        /// Create a column based on a property of a certain object
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateColumn(PropertyInfo property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Create a column of a specific type with the given name in the given table
        /// </summary>
        /// <param name="tableName">The name of the table</param>
        /// <param name="columnName">The name of the column to create</param>
        /// <param name="type">The .NET type of the column to create</param>
        /// <param name="isNullable">Indicates whether the field if nullable or not</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateColumn(string tableName, string columnName, Type type, bool isNullable = true, object transaction = null);

        /// <summary>
        /// Delete a column based on a property of a certain object
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteColumn(PropertyInfo property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Delete a column from a table
        /// </summary>
        /// <param name="property">The column name.</param>
        /// <param name="table">The table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteColumn(string property, string table, object transaction = null);

        /// <summary>
        /// Drops the default value of a column.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteDefault(PropertyInfo property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Sets the default value of a column.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to base the column on.</param>
        /// <param name="value">The value to set the default to.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void SetDefault(PropertyInfo property, object value, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Creates a constraint of <see cref="PostgresConstraintKind"/> on columns represented by <see cref="PropertyInfo"/>s in
        /// object <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The array of <see cref="PropertyInfo"/> representing the columns.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> represneting the table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateUniquenessConstraint(PropertyInfo[] property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Deletes a constraint of <see cref="PostgresConstraintKind"/> on columns represented by <see cref="PropertyInfo"/>s in
        /// object <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The array of <see cref="PropertyInfo"/> representing the columns.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> represneting the table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteUniquenessConstraint(PropertyInfo[] property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Creates a not null constraint on a given <see cref="PropertyInfo"/> in object <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to apply the Not Null constraint to.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the constraint to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateNotNullConstraint(PropertyInfo property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Deletes a not null constraint on a given <see cref="PropertyInfo"/> in object <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to delete the Not Null constraint from.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the constraint to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteNotNullConstraint(PropertyInfo property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Make the specified column (based on <see cref="PropertyInfo"/>) a primary key.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreatePrimaryKeyConstraint(IEntityObject thing, object transaction = null);

        /// <summary>
        /// Make the specified columns a primary key.
        /// </summary>
        /// <param name="table">The table</param>
        /// <param name="primaryKeyColumns">The columns used for the primary key</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreatePrimaryKeyConstraint(string table, string[] primaryKeyColumns, object transaction = null);

        /// <summary>
        /// Removes the primary key constraint from the specified <see cref="PropertyInfo"/> in the specfied <see cref="IEntityObject"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to remove the primary key from.</param>
        /// <param name="thing">The <see cref="IEntityObject"/> to be used as template.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeletePrimaryKeyConstraint(PropertyInfo property, IEntityObject thing, object transaction = null);

        /// <summary>
        /// Removes the primary key constraint from the specified table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeletePrimaryKeyConstraint(string table, object transaction = null);

        /// <summary>
        /// Creates a foreign key constraint between the column <see pref="fromProperty"/> in <see pref="fromThing"/> table and the <see pref="toProperty"/>
        /// in the <see pref="toThing"/> table.
        /// </summary>
        /// <param name="fromProperty">The property (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained property.</param>
        /// <param name="toProperty">The property that it is constrained to.</param>
        /// <param name="toThing">The table that contains the property that is constrained to.</param>
        /// <param name="onDelete">The behavior on delete</param>
        /// <param name="deferred">A value indicating whether the foreign key constraint should be deferrable.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateForeignKeyConstraint(PropertyInfo fromProperty, IEntityObject fromThing, PropertyInfo toProperty, IEntityObject toThing, FkDeleteBehaviorKind onDelete = FkDeleteBehaviorKind.NO_ACTION, bool deferred = true, object transaction = null);

        /// <summary>
        /// Creates a foreign key constraint between the column in a table and another column from another table
        /// </summary>
        /// <param name="fromProperty">The properties (column) that is constrained.</param>
        /// <param name="fromTable">The table that contains the constrained properties.</param>
        /// <param name="toProperty">The properties that it is constrained to.</param>
        /// <param name="toTable">The table that contains the properties that is constrained to.</param>
        /// <param name="onDelete">The behavior on delete</param>
        /// <param name="deferred">A value indicating whether the foreign key constraint should be deferrable.</param> 
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateForeignKeyConstraint(string fromProperty, string fromTable, string toProperty, string toTable, FkDeleteBehaviorKind onDelete = FkDeleteBehaviorKind.NO_ACTION, bool deferred = true,  object transaction = null);

        /// <summary>
        /// Deletes a foreign key constraint between the column <see pref="fromProperty"/> in <see pref="fromThing"/> table and the <see pref="toProperty"/>
        /// in the <see pref="toThing"/> table.
        /// </summary>
        /// <param name="fromProperty">The property (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained property.</param>
        /// <param name="toProperty">The property that it is constrained to.</param>
        /// <param name="toThing">The table that contains the property that is constrained to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteForeignKeyConstraint(string fromProperty, string fromThing, string toProperty, string toThing, object transaction = null);

        /// <summary>
        /// Deletes a foreign key constraint between the column <see pref="fromProperty"/> in <see pref="fromThing"/> table and the <see pref="toProperty"/>
        /// in the <see pref="toThing"/> table.
        /// </summary>
        /// <param name="fromProperty">The property (column) that is constrained.</param>
        /// <param name="fromThing">The table that contains the constrained property.</param>
        /// <param name="toProperty">The property that it is constrained to.</param>
        /// <param name="toThing">The table that contains the property that is constrained to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteForeignKeyConstraint(PropertyInfo fromProperty, IEntityObject fromThing, PropertyInfo toProperty, IEntityObject toThing, object transaction = null);

        /// <summary>
        /// Deletes a foreign key constraint in a table.
        /// </summary>
        /// <param name="fromTable">The table that contains the constrained properties.</param>
        /// <param name="constraintName">The name of the constraint to drop</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteForeignKeyConstraint(string fromTable, string constraintName, object transaction);

        /// <summary>
        /// Creates a new item of the given <see cref="IEntityObject"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the entity object.</typeparam>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to create a record from.
        /// </param>
        /// <param name="ignoreNull">
        /// If set to true will not submit explicitly set values.
        /// </param>
        /// <param name="transaction">
        /// If command is to be transaction safe you can supply the transaction object here.
        /// </param>
        /// <returns>
        /// The <see cref="T"/> object that was created.
        /// </returns>
        T CreateRecord<T>(T thing, bool ignoreNull = false, object transaction = null) where T : IEntityObject;

        /// <summary>
        /// Create records in a table
        /// </summary>
        /// <param name="table">The name of the table</param>
        /// <param name="columns">The name of the columns</param>
        /// <param name="values">The values to inject</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateRecord(string table, string[] columns, object[] values, object transaction = null);

        /// <summary>
        /// Updates the record of the given <see cref="IEntityObject"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of the entity updated.
        /// </typeparam>
        /// <param name="thing">
        /// The <see cref="IEntityObject"/> to update the record of.
        /// </param>
        /// <param name="ignoreNull">
        /// If set to true will not submit explicitly set values.
        /// </param>
        /// <param name="transaction">
        /// If command is to be transaction safe you can supply the transaction object here.
        /// </param>
        /// <returns>
        /// The <see cref="T"/> entity after the update.
        /// </returns>
        T UpdateRecord<T>(T thing, bool ignoreNull = false, object transaction = null) where T : IEntityObject;

        /// <summary>
        /// Deletes the record of the given <see cref="IEntityObject"/> instance.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to delete the record of.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteRecord(IEntityObject thing, object transaction = null);

        /// <summary>
        /// Deletes the record of the given table
        /// </summary>
        /// <param name="table">The table to delete the record of.</param>
        /// <param name="pkey">The primary key of the table</param>
        /// <param name="value">The value to delete</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteRecord(string table, string pkey, object value, object transaction = null);

        /// <summary>
        /// Read all records of a given <see cref="IEntityObject"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property by which to sort.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// A list of <see cref="IEntityObject"/> objects that were returned from the database.
        /// </returns>
        List<T> ReadRecords<T>(int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject;

        /// <summary>
        /// Read all records of a given <see cref="IEntityObject"/> where conditions are met.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="fromProperty">The property to check.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <param name="value">The value to check against.</param>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property by which to sort.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// A list of <see cref="IEntityObject"/> objects that were returned from the database.
        /// </returns>
        List<T> ReadRecordsWhere<T>(PropertyInfo fromProperty, string comparer, object value, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject;

        /// <summary>
        /// Read all records of a given <see cref="IEntityObject"/> where conditions are met.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="whereQueriesByAnd">A value indicating whether the where queries are joined by AND.</param>
        /// <param name="limit">The limit of number of records to be returned.</param>
        /// <param name="offset">The offset from which to start the records.</param>
        /// <param name="orderBy">The property by which to sort.</param>
        /// <param name="orderDescending">Indicates whether the order should be descending.</param>
        /// <returns>
        /// A list of <see cref="IEntityObject"/> objects that were returned from the database.
        /// </returns>
        List<T> ReadRecordsWhere<T>(IEnumerable<IWhereQueryContainer> queries, bool whereQueriesByAnd = true, int? limit = null, int? offset = null, PropertyInfo orderBy = null, bool orderDescending = false) where T : IEntityObject;

        /// <summary>
        /// Reads a record where the primary key matches the supplied id
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IEntityObject"/> instance</typeparam>
        /// <param name="id">The id to use to look up the primary key,</param>
        /// <returns>The instance of the object if it exists and exists solely. If more matches exist then the first one or nothing found then null is returned.</returns>
        T ReadRecord<T>(object id) where T : IEntityObject;

        /// <summary>
        /// Returns a very close estimate of the number of records of a table based on thing.
        /// </summary>
        /// <typeparam name="T">The type of thing.</typeparam>
        /// <param name="thing">The thing from which to read the number of records of.</param>
        /// <returns>The (approximate) number of records in the table.</returns>
        long CountRecords<T>(T thing) where T : IEntityObject;

        /// <summary>
        /// Count all records of a given <see cref="IEntityObject"/> where conditions are met.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="IEntityObject"/> to get from the database.
        /// </typeparam>
        /// <param name="queries">
        /// The queries.
        /// </param>
        /// <param name="whereQueriesByAnd">A value indicating whether the where queries are joined by AND.</param>
        /// <returns>
        /// The count of the requested query.
        /// </returns>
        long CountRecordsWhere<T>(IEnumerable<IWhereQueryContainer> queries, bool whereQueriesByAnd = true) where T : IEntityObject;

        /// <summary>
        /// Returns a very close estimate of the number of records of a table based on table name.
        /// </summary>
        /// <param name="tableName">The name of the table from which to read the number of records of.</param>
        /// <returns>The (approximate) number of records in the table.</returns>
        long CountRecords(string tableName);

        /// <summary>
        /// Alters the schema owner to current user.
        /// </summary>
        /// <param name="schemaName">The name of the schema to drop.</param>
        /// <param name="transaction">The transaction object.</param>
        void AlterSchemaOwner(string schemaName, object transaction);

        /// <summary>
        /// Drops all tables in the public schema. IMPORTANT!! Very dangerous function and should not be used eva!
        /// </summary>
        /// <param name="schemaName">The name of the schema to drop all tables on.</param>
        void DropAllTables(string schemaName);

        /// <summary>
        /// Drops a schema. IMPORTANT!! Very dangerous function and should not be used eva!
        /// </summary>
        /// <param name="schemaName">The name of the schema to drop.</param>
        /// <param name="transaction">The transaction object.</param>
        void DropSchema(string schemaName, object transaction = null);

        /// <summary>
        /// Creates a schema.
        /// </summary>
        /// <param name="schemaName">The name of the schema to create.</param>
        /// <param name="transaction">The transaction object.</param>
        void CreateSchema(string schemaName, object transaction = null);

        /// <summary>
        /// Creates a replication function and trigger on a given table. The given colums will be added to the replication table on insert and cleaned when deleted.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the function and trigger to.</param>
        /// <param name="replicationTable">The table which to replicate to.</param>
        /// <param name="idColumnName">The name of the id column to be used for replication. Must be the same between target and replication table.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateReplicateFunctionAndTrigger(IEntityObject thing, string replicationTable, string idColumnName, object transaction = null);

        /// <summary>
        /// Drops a replication function and trigger on a given table.
        /// </summary>
        /// <param name="thing">The <see cref="IEntityObject"/> to apply the function and trigger to.</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteReplicateFunctionAndTrigger(IEntityObject thing, object transaction = null);

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
        void CreateFunctionAndTrigger(string table, string functionName, string functionBody, bool isBefore, bool onInsert, bool onUpdate, bool onDelete, object transaction = null);

        /// <summary>
        /// Create a function
        /// </summary>
        /// <param name="functionName">The function name</param>
        /// <param name="functionBody">The function body</param>
        /// <param name="returnType">The function return type</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void CreateFunction(string functionName, string functionBody, string returnType, object transaction = null);

        /// <summary>
        /// Delete a function
        /// </summary>
        /// <param name="functionName">The function name</param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteFunction(string functionName, object transaction = null);

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
        void CreateIndex(IEntityObject thing, List<PropertyInfo> columns, object transaction = null);

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
        void CreateIndex(string tableName, IList<string> columnNames, object transaction = null);

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
        void DeleteIndex(IEntityObject thing, List<PropertyInfo> columns, object transaction = null);

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
        void DeleteIndex(string tableName, IList<string> columnNames, object transaction = null);

        /// <summary>
        /// Deletes an index specified by name.
        /// </summary>
        /// <param name="indexName">
        /// The name of the index.
        /// </param>
        /// <param name="transaction">If command is to be transaction safe you can supply the transaction object here.</param>
        void DeleteIndex(string indexName, object transaction = null);
    }
}