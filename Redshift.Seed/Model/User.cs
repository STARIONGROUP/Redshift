namespace Redshift.Seed.Model
{
    using System;
    using System.Runtime.Serialization;
    using Api.Attributes;
    using Orm.Attributes;

    [ApiDescription("A normal physical person.")]
    [ApiIgnoreMethod(RestMethods.DELETE, "Users cannot be DELETED.")]
    public class User : Thing<User>
    {
        [IgnoreDataMember]
        public override string TableName => "people";

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [ApiDescription("A unique username used for authenticaltion.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password. Nulled by the api.
        /// </summary>
        [ApiSerializeNull]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt. Completely ignored by the api.
        /// </summary>
        [ApiIgnore]
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [EntityColumnNameOverride("electronicmail")]
        [ApiWarning("The email should follow a specific regex pattern.")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user group uuid.
        /// </summary>
        public Guid Usergroup { get; set; }

        /// <summary>
        /// Gets the usergroup entity.
        /// </summary>
        [IgnoreDataMember]
        public Usergroup UsergroupEntity => Model.Usergroup.Find(this.Usergroup);
    }
}
