using System;
using System.Collections.Generic;
using System.Text;

namespace Redshift.Sample.Model
{
    public interface IMyProjectBaseObject
    {
        Guid Uuid { get; set; }

        DateTime ModifiedOn { get; set; }
    }
}
