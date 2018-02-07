namespace Redshift.Sample.Migrations
{
    using System;

    using Redshift.Orm.Database;

    /// <summary>
    /// The purpose of the <see cref="MySecondMigration"/> migration is to ....
    /// </summary>
    internal class MySecondMigration : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Uuid 
        {
            get 
            { 
                return Guid.Parse("99df23d5-ec27-4a50-bbfb-68828b9b6aa8"); 
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public override string Name
        {
            get 
            { 
                return this.GetType().Name; 
            }
        }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long date. This is done for sorting purposes.
        /// </summary>
        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "20180207094932", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description
        {
            get 
            { 
                return "Some description here."; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(2, 4, 3);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            // what to do to migrate
        }

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        public override void MigrationReset()
        {
            // what to do in case of migration fail
        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            // what to do to roll back the migration
        }

        /// <summary>
        /// The seeds the database if needed. This method can be left empty.
        /// </summary>
        public override void Seed()
        {
            // any required seeding
#if DEBUG

#endif
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public override bool ShouldMigrate()
        {
            // migration condition
            return base.ShouldMigrate();
        }
    }
}
