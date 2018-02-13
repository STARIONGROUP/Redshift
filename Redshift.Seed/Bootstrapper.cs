#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bootstrapper.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Seed.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Seed
{
    using System;
    using System.Net.Http;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Responses.Negotiation;
    using Nancy.TinyIoc;
    using NLog;
    using Api;
    using Api.Json;
    using Model;
    using Orm.Database;

    /// <summary>
    /// Application bootstrapper.
    /// </summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Nancy internal configuration. Used for registering content negotiators and processors.
        /// </summary>
        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get
            {
                //Logger.Info("Setting up response processors...");

                var processors = new[]
                {
                    typeof(Api.Json.JsonProcessor)
                };

                return NancyInternalConfiguration.WithOverrides(x => x.ResponseProcessors = processors);
            }
        }

        /// <summary>
        /// reinitialize the bootstrapper - can be used for adding pre-/post- hooks and
        ///             any other initialization tasks that aren't specifically container setup
        ///             related
        /// </summary>
        /// <param name="container">Container instance for resolving types if required.</param>
        /// <param name="pipelines">The pipelines used in this application.</param>
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Logger.Info("Bootstrapping Application");

            Logger.Info("Setting up Pipelines");
            base.ApplicationStartup(container, pipelines);

            Logger.Info("Setting up Serialization Resolver");

            // register model resolvers
            SeedResolverRegistry.Register();

            // register serializer
            container.Register<ISerializer, JsonApiSerializer>().AsSingleton();

            var serializer = container.Resolve<ISerializer>() as JsonApiSerializer;

            if (serializer != null)
            {
                serializer.DeserializationMap = EntityResolverMap.DeserializationMap;
            }

            try
            {
                Logger.Info("Initializing database connection...");

                DatabaseSession.Instance.CreateConnector("localhost", 5432, "redshiftseed", "redshiftseed", "redshift", ConnectorType.Postgresql);

                Logger.Info("Database connected...");
            }
            catch (Exception ex)
            {
                Logger.Info("Failed to establish database connected...");
                throw new HttpRequestException(string.Format("The connection to database could not be made: {0}", ex.Message));
            }

#if DEBUG

            Logger.Info("DEBUG Detected. Reseting database.");

            // dev mode only! wipe the db to remigrate and reseed, should always delete this once the first few migrations are finalized.
            MigrationEngine.DropAllTables("public");

            Logger.Info("Reseting database complete...");

#endif

            Logger.Info("Initializing migration engine...");

            MigrationEngine.Migrate();

            Logger.Info("Migrations done...");

            Logger.Info("Application Finished Bootstrapping");
        }
    }
}
