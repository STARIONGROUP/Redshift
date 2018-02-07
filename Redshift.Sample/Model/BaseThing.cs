namespace Redshift.Sample.Model
{
    using System;
    using Orm.EntityObject;

    public abstract class BaseThing<T> : EntityObject<T>, IMyProjectBaseObject where T: IEntityObject
    {
        public Guid Uuid { get; set; }

        public DateTime ModifiedOn { get; set; }
    }

    public class MyObjectA : BaseThing<MyObjectA>
    {
        // ...
    }
}
