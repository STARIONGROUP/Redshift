#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationEngine.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using Helpers;
    using NLog;

    /// <summary>
    /// The migration engine.
    /// </summary>
    public static class MigrationEngine
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Migrates the database.
        /// </summary>
        /// <param name="excludes">The list of migration names that should be excluded.</param>
        public static void Migrate(List<string> excludes = null)
        {
            var migrations = GetAllMigrations();

            var migrationResults = new List<Tuple<string, string, string, string, Version>>();

            foreach (var migration in migrations)
            {
                if (excludes != null)
                {
                    if (excludes.Contains(migration.Name))
                    {
                        continue;
                    }
                }

                // perform the necessary check whether the migration should be made.
                if (migration.ShouldMigrate())
                {
                    try
                    {
                        // perform the migration script
                        Stopwatch timer;

                        try
                        {
                            timer = Stopwatch.StartNew();
                            Logger.Info($"Executing migration {migration.Name}...");
                            migration.Migrate();
                            
                            timer.Stop();
                            Logger.Info("Done.");
                            migrationResults.Add(Tuple.Create("Done", "Migration", migration.Name, $"{timer.ElapsedMilliseconds}ms", migration.Version));
                            migration.MigrationExecutionTime = timer.ElapsedMilliseconds;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Migration {1} failed: {0}", ex.Message, migration.Name));
                        }

                        // perform the seed
                        try
                        {
                            timer = Stopwatch.StartNew();
                            Logger.Info($"Executing seed for {migration.Name}...");
                            migration.Seed();

                            timer.Stop();
                            Logger.Info("Done.");
                            migrationResults.Add(Tuple.Create("Done", "Seed", migration.Name, $"{timer.ElapsedMilliseconds}ms", migration.Version));
                            migration.SeedExecutionTime = timer.ElapsedMilliseconds;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Seed {1} failed: {0}", ex.Message, migration.Name));
                        }

                        // same the migration to the table
                        try
                        {
                            migration.Save();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format("Save to migration table {1} failed: {0}", ex.Message, migration.Name));
                        }
                    }
                    catch (Exception)
                    {
                        // on fail run migration reset
                        migrationResults.Add(Tuple.Create("FAIL", "Migration", migration.Name, "0ms", migration.Version));

                        migration.MigrationReset();
                        throw;
                    }
                }
            }

            PrintResultTable(migrationResults);
        }
        
        /// <summary>
        /// Cleans the "public" shema compretely. IMPORTANT: very dangerous function!! Only use in VERY early development!
        /// </summary>
        /// <param name="schemaName">
        /// The schema Name.
        /// </param>
        public static void DropAllTables(string schemaName)
        {
            DatabaseSession.Instance.Connector.DropAllTables(schemaName);
        }

        /// <summary>
        /// Rolls back all found migrations
        /// </summary>
        /// <param name="name">The name of the single rollback to perform. Must be the Name parameter of the <see cref="IMigration"/></param>
        /// <param name="hard">If true, ignores thrown errors and powers through the migration reverse.</param>
        public static void Reset(string name = null, bool hard = false)
        {
            // Get all migrations in reverse
            var migrations = GetAllMigrationsReverse();
            var writtenMigrations = new List<MigrationRecord>();

            // Read migrations table to get all
            var template = new MigrationRecord();
            if (DatabaseSession.Instance.Connector.CheckTableExists(template))
            {
                writtenMigrations = DatabaseSession.Instance.Connector.ReadRecords<MigrationRecord>();
            }

            if (!string.IsNullOrEmpty(name))
            {
                migrations = migrations.Where(m => m.Name == name).ToList();
            }

            // Perform peice by peice rollback
            foreach (var migration in migrations)
            {
                // perform the necessary check whether the migration should be rolled back. Only roll back if it has been applied.
                if (writtenMigrations.Any(m => m.Uuid == migration.Uuid))
                {
                    // perform the migration script
                    try
                    {
                        Logger.Info($"Executing reverse on migration {migration.Name}...");
                        migration.Reverse();
                        migration.Delete();
                        Logger.Info("Done");
                    }
                    catch (Exception ex)
                    {
                        if (!hard)
                        {
                            throw new Exception(string.Format("Migration rollback of {1} failed: {0}", ex.Message, migration.Name)); 
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Rolls back all found migrations to a specified version.
        /// </summary>
        /// <param name="version">The version number up to which to roll back to.</param>
        /// <param name="hard">If true, ignores thrown errors and powers through the migration reverse.</param>
        public static void ResetTo(Version version, bool hard = false)
        {
            // Get all migrations in reverse
            var migrations = GetAllMigrationsReverse();
            var writtenMigrations = new List<MigrationRecord>();

            // Read migrations table to get all
            var template = new MigrationRecord();
            if (DatabaseSession.Instance.Connector.CheckTableExists(template))
            {
                writtenMigrations = DatabaseSession.Instance.Connector.ReadRecords<MigrationRecord>();
            }
            
            // leave only the migrations which are above the specified version
            migrations = migrations.Where(m => m.Version > version).OrderByDescending(x => x.FullName).ToList();

            Logger.Info($"Resetting to database version {version}...");

            // Perform peice by peice rollback
            foreach (var migration in migrations)
            {
                // perform the necessary check whether the migration should be rolled back. Only roll back if it has been applied.
                if (writtenMigrations.Any(m => m.Uuid == migration.Uuid))
                {
                    // perform the migration script
                    try
                    {
                        Logger.Info($"Rolling back migration {migration.Name}...");
                        migration.Reverse();
                        migration.Delete();
                        Logger.Info("Done");
                    }
                    catch (Exception ex)
                    {
                        if (!hard)
                        {
                            throw new Exception(string.Format("Migration rollback of {1} failed: {0}", ex.Message, migration.Name));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Prints a status table to the output.
        /// </summary>
        /// <param name="migrationStatus">
        /// The migration Status.
        /// </param>
        private static void PrintResultTable(IEnumerable<Tuple<string, string, string, string, Version>> migrationStatus)
        {
            Console.WriteLine();
            Console.WriteLine(
                migrationStatus.ToStringTable(
                    new[] { "Status", "Action", "Name", "Time", "Version" },
                    a => a.Item1,
                    a => a.Item2,
                    a => a.Item3,
                    a => a.Item4,
                    a => a.Item5));
        }

        /// <summary>
        /// The get all migrations classes that extend from <see cref="IMigration"/>. Sorted by the
        /// <see cref="IMigration.FullName"/> property in ascending order.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/> of <see cref="IMigration"/> classes.
        /// </returns>
        private static List<IMigration> GetAllMigrations()
        {
            var type = typeof(IMigration);
            var types = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.GetTypeInfo().IsAssignableFrom(p) && !p.GetTypeInfo().IsInterface && !p.GetTypeInfo().IsAbstract);

            return types.Select(Activator.CreateInstance).Select(migration => migration).Cast<IMigration>().OrderBy(x => x.FullName).ToList();
        }

        /// <summary>
        /// The get all migrations classes that extend from <see cref="IMigration"/>. Sorted by the
        /// <see cref="IMigration.FullName"/> property in descending order.
        /// </summary>
        /// <returns>
        /// The <see cref="List{T}"/> of <see cref="IMigration"/> classes.
        /// </returns>
        private static List<IMigration> GetAllMigrationsReverse()
        {
            var type = typeof(IMigration);
            var types = System.AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.GetTypeInfo().IsAssignableFrom(p) && !p.GetTypeInfo().IsInterface && !p.GetTypeInfo().IsAbstract);

            return types.Select(Activator.CreateInstance).Select(migration => migration).Cast<IMigration>().OrderByDescending(x => x.FullName).ToList();
        }
    }
}