#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSession.cs" company="RHEA System S.A.">
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

    /// <summary>
    /// The purpose of the <see cref="DatabaseSession"/> is to provide access to the database and the <see cref="IDatabaseConnector"/>.
    /// </summary>
    public class DatabaseSession : IDisposable
    {
        /// <summary>
        /// The instance initiator for the singleton.
        /// </summary>
        private static readonly DatabaseSession instance = new DatabaseSession();

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static DatabaseSession Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="DatabaseSession"/> class from being created.
        /// </summary>
        private DatabaseSession()
        {
        }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        internal DatabaseCredentials Credentials { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDatabaseConnector"/> used to perform actions on the database.
        /// </summary>
        public IDatabaseConnector Connector { get; set; }

        /// <summary>
        /// Creates the connector
        /// </summary>
        /// <param name="host">The host name</param>
        /// <param name="port">The port</param>
        /// <param name="databaseName">The database name</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <param name="connectionType">The connector type</param>
        public void CreateConnector(string host, long port, string databaseName, string username, string password, ConnectorType connectionType = ConnectorType.Postgresql)
        {
            var credentials = new DatabaseCredentials
            {
                Host = host,
                Port = port,
                DatabaseName = databaseName,
                Username = username,
                Password = password
            };

            this.CreateConnector(credentials, connectionType);
        }

        /// <summary>
        /// Creates the connector, with the ability to supply the connector instance.
        /// </summary>
        /// <param name="connector">
        /// The <see cref="IDatabaseConnector"/> to inject.
        /// </param>
        public void CreateConnector(IDatabaseConnector connector)
        {
            this.Connector = connector;
        }

        /// <summary>
        /// Dispose of this session.
        /// </summary>
        public void Dispose()
        {
            this.Connector = null;
        }

        /// <summary>
        /// Creates the connector based on server config.
        /// </summary>
        /// <param name="credentials">
        /// The credentials.
        /// </param>
        /// <param name="connectionType">
        /// The connector type.
        /// </param>
        public void CreateConnector(DatabaseCredentials credentials, ConnectorType connectionType = ConnectorType.Postgresql)
        {
            // setup credentials
            this.Credentials = credentials;

            // create the connector
            switch (connectionType)
            {
                case ConnectorType.Postgresql:
                    this.Connector = new PostgresDatabaseConnector();
                    break;
            }
        }

        /// <summary>
        /// The create connection to the database.
        /// </summary>
        /// <returns>
        /// The <see cref="NpgsqlConnection"/> that can be used to perform transactions.
        /// </returns>
        public object CreateConnection()
        {
            return this.Connector.CreateConnection(this.Credentials);
        }

        /// <summary>
        /// Creates a <see cref="NpgsqlConnection"/> with a <see cref="NpgsqlTransaction"/> attached to it.
        /// </summary>
        /// <returns>The transaction object connected to this connection.</returns>
        public object CreateTransaction()
        {
            return this.Connector.CreateTransaction(this.Credentials);
        }

        /// <summary>
        /// Commits the supplied <see cref="NpgsqlTransaction"/> and closes it's attached <see cref="NpgsqlConnection"/>
        /// </summary>
        /// <param name="transaction">The transaction that needs to be committed.</param>
        public void CommitTransaction(object transaction)
        {
            this.Connector.CommitTransaction(transaction);
        }
    }
}
