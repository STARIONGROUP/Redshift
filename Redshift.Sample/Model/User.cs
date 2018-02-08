using System;
using System.Runtime.Serialization;
using Redshift.Orm.Attributes;
using Redshift.Orm.EntityObject;

namespace Redshift.Sample.Model
{
    using Api.Attributes;

    [ApiDescription("A normal physical person.")]
    [ApiIgnoreMethod(RestMethods.PATCH | RestMethods.DELETE, "Users cannot be PATCHED or DELETED.")]
    public class User : EntityObject<User>
    {
        [IgnoreDataMember]
        public override string TableName => "people";

        [IgnoreDataMember]
        public override string PrimaryKey => "Id";

        public Guid Id { get; set; }

        [ApiDescription("A unique username used for authenticaltion.")]
        public string Username { get; set; }

        [ApiSerializeNull]
        public string Password { get; set; }

        [EntityColumnNameOverride("electronicmail")]
        [ApiWarning("The email should follow a specific regex pattern.")]
        public string Email { get; set; }
    }
}
