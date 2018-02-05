using System;
using System.Runtime.Serialization;
using Redshift.Orm.Attributes;
using Redshift.Orm.EntityObject;

namespace Redshift.Sample.Model
{
    public class User : EntityObject<User>
    {
        [IgnoreDataMember]
        public override string TableName => "people";

        [IgnoreDataMember]
        public override string PrimaryKey => "Id";

        public int Id { get; set; }

        public string Username { get; set; }

        [DbIgnore]
        public string Password { get; set; }

        [EntityColumnNameOverride("electronicmail")]
        public string Email { get; set; }
    }
}
