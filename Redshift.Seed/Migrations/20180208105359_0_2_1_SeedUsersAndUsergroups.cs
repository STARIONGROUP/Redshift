namespace Redshift.Seed.Migrations
{
    using System;
    using System.Collections.Generic;
    using Api.Helpers;
    using Model;
    using Redshift.Orm.Database;

    /// <summary>
    /// The purpose of the <see cref="SeedUsersAndUsergroups"/> migration is to ....
    /// </summary>
    internal class SeedUsersAndUsergroups : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Uuid 
        {
            get 
            { 
                return Guid.Parse("b9ddb6b2-9268-4977-bb41-f2b0e14856e2"); 
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
                return string.Format("{0}_{1}", "20180208105359", this.Name);
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
                return new Version(0, 2, 1);
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
            var adminUsergroup = new Usergroup
            {
                Uuid = Guid.NewGuid(),
                Name = "Administrator",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                Permissions = new List<string> { "CanAll" }
            };

            adminUsergroup.Save();

            // default admin passowrd
            var adminpassword = "vsylsHVjk93";

#if DEBUG
            var testUsergroup = new Usergroup
            {
                Uuid = Guid.NewGuid(),
                Name = "User",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                Permissions = new List<string> { "CanViewSome", "CanWriteSome" }
            };

            testUsergroup.Save();

            // for easy testing seed a simple password
            adminpassword = "pass";
#endif
            var adminUser = new User
            {
                Uuid = Guid.NewGuid(),
                Username = "admin",
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                Email = "bla@bla.com",
                Usergroup = adminUsergroup.Uuid
            };

            adminUser.Salt = CryptographyHelper.GetSalt();
            adminUser.Password = CryptographyHelper.Encrypt(adminpassword, adminUser.Salt);

            adminUser.Save();

#if DEBUG
            // seed some dummy users in debug
            for (var i = 0; i < 10; i++)
            {
                var user = new User
                {
                    Uuid = Guid.NewGuid(),
                    Username = $"user{i}",
                    CreatedOn = DateTime.UtcNow,
                    ModifiedOn = DateTime.UtcNow,
                    Email = $"bla{i}@bla.com",
                    Usergroup = testUsergroup.Uuid
                };

                user.Salt = CryptographyHelper.GetSalt();
                user.Password = CryptographyHelper.Encrypt(adminpassword, user.Salt);

                user.Save();
            }
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
