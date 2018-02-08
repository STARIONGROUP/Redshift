namespace Redshift.Seed.Model
{
    using System;
    using Orm.EntityObject;

    /// <summary>
    /// Public base interface for all objects of this project
    /// </summary>
    public interface ISeedObject
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        Guid Uuid { get; set; }

        /// <summary>
        /// Gets or sets the date this object was modified
        /// </summary>
        DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the date this object was created.
        /// </summary>
        DateTime CreatedOn { get; set; }

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
        /// The <see cref="IEntityObject"/> that was saved.
        /// </returns>
        IEntityObject Save(Guid? userUuid = null, bool ignoreNull = false, object transaction = null);

        /// <summary>
        /// The deletes this object from the database.
        /// </summary>
        /// <param name="transaction">The transaction object.</param>
        void Delete(object transaction = null);
    }
}
