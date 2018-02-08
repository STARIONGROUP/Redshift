namespace Redshift.Seed.Model
{
    using System;
    using Orm.EntityObject;

    public abstract class Thing<T> : EntityObject<T>, ISeedObject where T : IEntityObject
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or sets the date this object was modified
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the date this object was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
