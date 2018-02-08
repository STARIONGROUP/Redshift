namespace Redshift.Seed
{
    using System;
    using System.Net.Http;
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Responses.Negotiation;
    using Nancy.TinyIoc;
    using NLog;
    using Redshift.Api;
    using Redshift.Api.Json;
    using Redshift.Orm.Database;

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
                    typeof(Redshift.Api.Json.JsonProcessor),
                    typeof(ViewProcessor),
                    typeof(XmlProcessor)
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
            //Logger.Info("Bootstrapping Application");

            //Logger.Info("Setting up Pipelines");
            base.ApplicationStartup(container, pipelines);

            //Logger.Info("Setting up Serialization Resolver");

            // register serializer
            container.Register<ISerializer, JsonApiSerializer>().AsSingleton();

            var serializer = container.Resolve<ISerializer>() as JsonApiSerializer;

            if (serializer != null)
            {
                serializer.DeserializationMap = EntityResolverMap.DeserializationMap;
            }

            try
            {
                //Logger.Info("Initializing database connection...");

                DatabaseSession.Instance.CreateConnector("localhost", 5432, "redshiftseed", "redshiftseed", "redshift", ConnectorType.Postgresql);

                //Logger.Info("Database connected...");
            }
            catch (Exception ex)
            {
                //Logger.Info("Failed to establish database connected...");
                throw new HttpRequestException(string.Format("The connection to database could not be made: {0}", ex.Message));
            }

#if DEBUG
            //Logger.Info("DEBUG Detected. Reseting database.");

            MigrationEngine.DropAllTables("public");

            //Logger.Info("Reseting database complete...");
#endif
            //Logger.Info("Initializing migration engine...");

            MigrationEngine.Migrate();

            //Logger.Info("Migrations done...");

            //Logger.Info("Application Finished Bootstrapping");
        }
    }
}
