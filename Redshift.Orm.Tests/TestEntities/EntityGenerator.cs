#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityGenerator.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.Tests.
//
//    Redshift.Orm.Tests is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm.Tests is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.Tests.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;


    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="Thing"/> class
    /// </summary>
    ////[ApiDescription("The base class that all Sherlock persistent concepts derive from.")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class Thing<T> : IEntityObject where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Thing"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected Thing(Guid iid)	
        {
            this.Uuid = iid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Thing"/> class.
        /// </summary>
        protected Thing()
        {
        }


        ///<summary>
        /// The unique identifier for this persisted thing.
        ///</summary>
        ////[ApiDescription("The unique identifier for this persisted thing.")]
        public Guid Uuid { get; set; }


        ///<summary>
        /// The date this object was created on
        ///</summary>
        ////[ApiDescription("The date this object was created on")]
        public DateTime CreatedOn { get; set; }


        ///<summary>
        /// The date this object was last modified
        ///</summary>
        ////[ApiDescription("The date this object was last modified")]
        public DateTime ModifiedOn { get; set; }


        ///<summary>
        /// A value indicating whether this Thing is deprecated
        ///</summary>
        ////[ApiDescription("A value indicating whether this Thing is deprecated")]
        public bool IsDeprecated { get; set; }

        public string TableName { get; }

        public string PrimaryKey { get; }

        public IEntityObject Save(Guid? userUuid = null, bool ignoreNull = false, object transaction = null)
        {
            throw new NotImplementedException();
        }

        public void Delete(object transaction = null)
        {
            throw new NotImplementedException();
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NamedThing.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="NamedThing"/> class
    /// </summary>
    //[ApiDescription("an abstract type for Things that have a name")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class NamedThing<T> : UserEditableThing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamedThing"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected NamedThing(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedThing"/> class.
        /// </summary>
        protected NamedThing()
        {
        }


        ///<summary>
        /// The human identifier for this thing
        ///</summary>
        //[ApiDescription("The human identifier for this thing")]
        public string Denomination { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelephoneNumber.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="TelephoneNumber"/> class
    /// </summary>
    //[ApiDescription("Represents a telephone-number")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class TelephoneNumber : UserEditableThing<TelephoneNumber>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneNumber"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public TelephoneNumber(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TelephoneNumber"/> class.
        /// </summary>
        public TelephoneNumber()
        {
        }


        ///<summary>
        /// The area code associated to a specific geographical area. In Canada, they correspond to the first 3-digits of the ten-digit phone number
        ///</summary>
        //[ApiDescription("The area code associated to a specific geographical area. In Canada, they correspond to the first 3-digits of the ten-digit phone number")]
        public string AreaCode { get; set; }


        ///<summary>
        /// This represents the country code used in the telephone number.
        ///</summary>
        //[ApiDescription("This represents the country code used in the telephone number.")]
        public string CountryCode { get; set; }


        ///<summary>
        /// This represents the main part of phone number
        ///</summary>
        //[ApiDescription("This represents the main part of phone number")]
        public string MainNumber { get; set; }


        ///<summary>
        /// Represents the optional extension of a phone number
        ///</summary>
        //[ApiDescription("Represents the optional extension of a phone number")]
        public string Extension { get; set; }


        ///<summary>
        /// Represents the kind of telephone number this is
        ///</summary>
        //[ApiDescription("Represents the kind of telephone number this is")]
        public TelephoneKind TelephoneKind { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="TelephoneNumber"/>
        /// </summary>
        //[ApiDescription("The container of this TelephoneNumber")]
        public Guid Contact { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailAddress.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EmailAddress"/> class
    /// </summary>
    //[ApiDescription("Represents an email address")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EmailAddress : UserEditableThing<EmailAddress>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EmailAddress(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAddress"/> class.
        /// </summary>
        public EmailAddress()
        {
        }


        ///<summary>
        /// Represents the actual email address
        ///</summary>
        //[ApiDescription("Represents the actual email address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="EmailAddress"/>
        /// </summary>
        //[ApiDescription("The container of this EmailAddress")]
        public Guid Contact { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contact.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="Contact"/> class
    /// </summary>
    //[ApiDescription("An abstract type that represents a contact with its name and contact information (email, phone)")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class Contact<T> : NumberThing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected Contact(Guid iid) : base(iid: iid)	
        {
            this.Languages = new List<Guid>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        protected Contact()
        {
            this.Languages = new List<Guid>();

        }


        ///<summary>
        /// The contact's given name
        ///</summary>
        //[ApiDescription("The contact's given name")]
        public string GivenName { get; set; }


        ///<summary>
        /// The contact's last name
        ///</summary>
        //[ApiDescription("The contact's last name")]
        public string LastName { get; set; }


        ///<summary>
        /// The contact's language(s)
        ///</summary>
        //[ApiDescription("The contact's language(s)")]
        [DbIgnore]
        public List<Guid> Languages { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Client.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Client"/> class
    /// </summary>
    //[ApiDescription("Represents a client")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Client : Contact<Client>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Client(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        public Client()
        {
        }


        ///<summary>
        /// The position of the client in its organization
        ///</summary>
        //[ApiDescription("The position of the client in its organization")]
        public Guid Position { get; set; }


        ///<summary>
        /// The site of this client
        ///</summary>
        //[ApiDescription("The site of this client")]
        public Guid? Site { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Resource.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Resource"/> class
    /// </summary>
    //[ApiDescription("Represents a resource that is responsible for performing a task")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Resource : Contact<Resource>, IAddressThing	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Resource(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Resource"/> class.
        /// </summary>
        public Resource()
        {
        }


        ///<summary>
        /// The code for this resource
        ///</summary>
        //[ApiDescription("The code for this resource")]
        public string Code { get; set; }


        ///<summary>
        /// The type of this resource (i.e. The type may be for example Labour, Material or Contract labour)
        ///</summary>
        //[ApiDescription("The type of this resource (i.e. The type may be for example Labour, Material or Contract labour)")]
        public Guid ResourceType { get; set; }


        ///<summary>
        /// The organization of this resource
        ///</summary>
        //[ApiDescription("The organization of this resource")]
        public Guid Organization { get; set; }


        ///<summary>
        /// The replication site for this resource
        ///</summary>
        //[ApiDescription("The replication site for this resource")]
        public Guid ReplicationSite { get; set; }


        #region IAddressThing region

        ///<summary>
        /// The address care of
        ///</summary>
        //[ApiDescription("The address care of")]
        public string AddressCareOf { get; set; }


        ///<summary>
        /// The first line of the address
        ///</summary>
        //[ApiDescription("The first line of the address")]
        public string Line1 { get; set; }


        ///<summary>
        /// The second line of the address
        ///</summary>
        //[ApiDescription("The second line of the address")]
        public string Line2 { get; set; }


        ///<summary>
        /// The municipality of the address
        ///</summary>
        //[ApiDescription("The municipality of the address")]
        public string Municipality { get; set; }


        ///<summary>
        /// The post code of the address
        ///</summary>
        //[ApiDescription("The post code of the address")]
        public string PostCode { get; set; }

        #endregion	

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskEffort.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="TaskEffort"/> class
    /// </summary>
    //[ApiDescription("An abstract type representing a task effort")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class TaskEffort<T> : Thing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEffort"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected TaskEffort(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskEffort"/> class.
        /// </summary>
        protected TaskEffort()
        {
        }


        ///<summary>
        /// The time required in hours to execute the task
        ///</summary>
        //[ApiDescription("The time required in hours to execute the task")]
        public float Hours { get; set; }


        ///<summary>
        /// The number of pieces of equipment that needs work on in a given month.
        ///</summary>
        //[ApiDescription("The number of pieces of equipment that needs work on in a given month.")]
        public int Quantity { get; set; }


        ///<summary>
        /// The date of realization of the task
        ///</summary>
        //[ApiDescription("The date of realization of the task")]
        public DateTime Date { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EstimatedTaskEffort.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EstimatedTaskEffort"/> class
    /// </summary>
    //[ApiDescription("Represents an estimation of a task effort")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EstimatedTaskEffort : TaskEffort<EstimatedTaskEffort>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EstimatedTaskEffort"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EstimatedTaskEffort(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EstimatedTaskEffort"/> class.
        /// </summary>
        public EstimatedTaskEffort()
        {
        }

        /// <summary>
        /// Gets the container of this <see cref="EstimatedTaskEffort"/>
        /// </summary>
        //[ApiDescription("The container of this EstimatedTaskEffort")]
        public Guid WorkOrderTask { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActualTaskEffort.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="ActualTaskEffort"/> class
    /// </summary>
    //[ApiDescription("Represents an actual task effort")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class ActualTaskEffort : TaskEffort<ActualTaskEffort>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActualTaskEffort"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public ActualTaskEffort(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActualTaskEffort"/> class.
        /// </summary>
        public ActualTaskEffort()
        {
        }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public float Cost { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public string InvoiceNumber { get; set; }


        ///<summary>
        /// This defaults to “CSC” and you would normally just go to the Purchase Order field and enter the PO#, but if the repair action for example requires a Motorola Case number then you would select “Other” and then you can enter the case number in the Purchase Order field.
        ///</summary>
        //[ApiDescription("This defaults to “CSC” and you would normally just go to the Purchase Order field and enter the PO#, but if the repair action for example requires a Motorola Case number then you would select “Other” and then you can enter the case number in the Purchase Order field.")]
        public PurchaseOrderType PurchaseOrderType { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public bool IsPaid { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="ActualTaskEffort"/>
        /// </summary>
        //[ApiDescription("The container of this ActualTaskEffort")]
        public Guid WorkOrderTask { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserEditableThing.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="UserEditableThing"/> class
    /// </summary>
    //[ApiDescription("Represents a type that may be edited by a normal user")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class UserEditableThing<T> : DefinedThing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditableThing"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected UserEditableThing(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserEditableThing"/> class.
        /// </summary>
        protected UserEditableThing()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrderTask.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrderTask"/> class
    /// </summary>
    //[ApiDescription("Represents a task of a WorkOrder")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrderTask : NumberThing<WorkOrderTask>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderTask"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrderTask(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderTask"/> class.
        /// </summary>
        public WorkOrderTask()
        {
        }


        ///<summary>
        /// The resource responsible for executing the task
        ///</summary>
        //[ApiDescription("The resource responsible for executing the task")]
        public Guid Resource1 { get; set; }


        ///<summary>
        /// The optional company for the first resource
        ///</summary>
        //[ApiDescription("The optional company for the first resource")]
        public Guid? Resource2 { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="WorkOrderTask"/>
        /// </summary>
        //[ApiDescription("The container of this WorkOrderTask")]
        public Guid WorkOrder { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrder.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrder"/> class
    /// </summary>
    //[ApiDescription("Represents a WorkOrder performed on an Equipment")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrder : NumberThing<WorkOrder>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrder"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrder(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrder"/> class.
        /// </summary>
        public WorkOrder()
        {
        }


        ///<summary>
        /// The short-name for the WorkOrder
        ///</summary>
        //[ApiDescription("The short-name for the WorkOrder")]
        public string ShortName { get; set; }


        ///<summary>
        /// The description of the problem issued in the WorkOrder
        ///</summary>
        //[ApiDescription("The description of the problem issued in the WorkOrder")]
        public string ProblemDescription { get; set; }


        ///<summary>
        /// Represents a work-order that is performed on an equipment
        ///</summary>
        //[ApiDescription("Represents a work-order that is performed on an equipment")]
        public DateTime InitiatedDateTime { get; set; }


        ///<summary>
        /// The required date at chich the WorkOrder shall be completed
        ///</summary>
        //[ApiDescription("The required date at chich the WorkOrder shall be completed")]
        public DateTime RequiredDate { get; set; }


        ///<summary>
        /// The date at which the status of the WorkOrder changed
        ///</summary>
        //[ApiDescription("The date at which the status of the WorkOrder changed")]
        public DateTime StatusChangedDate { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public Guid Client { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public Guid Equipement { get; set; }


        ///<summary>
        /// The type of work order (i.e. Corrective Maintenance, Preventive Maintenance, Emergency call-out, Warranty Repair...)
        ///</summary>
        //[ApiDescription("The type of work order (i.e. Corrective Maintenance, Preventive Maintenance, Emergency call-out, Warranty Repair...)")]
        public Guid WorkOrderType { get; set; }


        ///<summary>
        /// The Work Order priority (i.e. Emergency, Normal, Fast, Shut Down...)
        ///</summary>
        //[ApiDescription("The Work Order priority (i.e. Emergency, Normal, Fast, Shut Down...)")]
        public Guid WorkOrderPriority { get; set; }


        ///<summary>
        /// The status of the WorkOrder (i.e. Active, Closed, Held...)
        ///</summary>
        //[ApiDescription("The status of the WorkOrder (i.e. Active, Closed, Held...)")]
        public Guid WorkOrderStatus { get; set; }


        ///<summary>
        /// The reason this WorkOrder is on hold. This is only applicable when the status is set to be "on hold". This gives the details for this status.
        ///</summary>
        //[ApiDescription("The reason this WorkOrder is on hold. This is only applicable when the status is set to be on hold. This gives the details for this status.")]
        public Guid? HoldReason { get; set; }


        ///<summary>
        /// An option field selected among a list of problem code available
        ///</summary>
        //[ApiDescription("An option field selected among a list of problem code available")]
        public Guid? ProblemCode { get; set; }


        ///<summary>
        /// An optional date and time that correspond to the start of the shutdown of the object if required
        ///</summary>
        //[ApiDescription("An optional date and time that correspond to the start of the shutdown of the object if required")]
        public DateTime? DownFrom { get; set; }


        ///<summary>
        /// An optional date and time that correspond to the end of the shutdown of the object if required
        ///</summary>
        //[ApiDescription("An optional date and time that correspond to the end of the shutdown of the object if required")]
        public DateTime? DownTo { get; set; }


        ///<summary>
        /// A value that specify whether the shutdown of the object under worked is required
        ///</summary>
        //[ApiDescription("A value that specify whether the shutdown of the object under worked is required")]
        public bool IsShutdownRequired { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Equipment.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Equipment"/> class
    /// </summary>
    //[ApiDescription("Represents an equipment that shall be managed")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Equipment : NumberThing<Equipment>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Equipment(Guid iid) : base(iid: iid)	
        {
            this.Part = new List<Guid>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Equipment"/> class.
        /// </summary>
        public Equipment()
        {
            this.Part = new List<Guid>();

        }


        ///<summary>
        /// The serial number of the equipment
        ///</summary>
        //[ApiDescription("The serial number of the equipment")]
        public string SerialNumber { get; set; }


        ///<summary>
        /// The optional installation date of the equipment
        ///</summary>
        //[ApiDescription("The optional installation date of the equipment")]
        public DateTime? InstallationDate { get; set; }


        ///<summary>
        /// The optional expiration date for the warranty of the equipment
        ///</summary>
        //[ApiDescription("The optional expiration date for the warranty of the equipment")]
        public DateTime? WarrantyExpirationDate { get; set; }


        ///<summary>
        /// The number normally used for the unit number assigned to this equipment by the client.
        ///</summary>
        //[ApiDescription("The number normally used for the unit number assigned to this equipment by the client.")]
        public string UnitNumber { get; set; }


        ///<summary>
        /// The location where the Equipment is installed
        ///</summary>
        //[ApiDescription("The location where the Equipment is installed")]
        public string BuildingZoneArea { get; set; }


        ///<summary>
        /// The optional replacement cost for this Equipment
        ///</summary>
        //[ApiDescription("The optional replacement cost for this Equipment")]
        public int? ReplacementCost { get; set; }


        ///<summary>
        /// An optional value that can be used to record the client assigned number or the HQ equipment sticker number
        ///</summary>
        //[ApiDescription("An optional value that can be used to record the client assigned number or the HQ equipment sticker number")]
        public string AssetId { get; set; }


        ///<summary>
        /// The reference drawing number
        ///</summary>
        //[ApiDescription("The reference drawing number")]
        public string ReferenceDrawingNumber { get; set; }


        ///<summary>
        /// The site where the Equipment is installed at
        ///</summary>
        //[ApiDescription("The site where the Equipment is installed at")]
        public Guid Site { get; set; }


        ///<summary>
        /// The category of equipment owner
        ///</summary>
        //[ApiDescription("The category of equipment owner")]
        public Guid Owner { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public EquipmentLevel EquipmentLevel { get; set; }


        ///<summary>
        /// The number of the same equipment being tracked by this object. This defaults to 1 if the EquipmentLevel is set to Single
        ///</summary>
        //[ApiDescription("The number of the same equipment being tracked by this object. This defaults to 1 if the EquipmentLevel is set to Single")]
        public int EquipmentQuantity { get; set; }


        ///<summary>
        /// The model of the equipment
        ///</summary>
        //[ApiDescription("The model of the equipment")]
        public Guid EquipmentModel { get; set; }


        ///<summary>
        /// The sub-system this equipment is a part from
        ///</summary>
        //[ApiDescription("The sub-system this equipment is a part from")]
        public Guid SubSystem { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public Guid? PreventiveMaintenance { get; set; }


        ///<summary>
        /// The installer of the equipment
        ///</summary>
        //[ApiDescription("The installer of the equipment")]
        public string Installer { get; set; }


        ///<summary>
        /// Indicates whether a shutdown is required
        ///</summary>
        //[ApiDescription("Indicates whether a shutdown is required")]
        public bool ShutdownRequired { get; set; }


        ///<summary>
        /// The part of this Equipment. (i.e. For a camera this could be a lens)
        ///</summary>
        //[ApiDescription("The part of this Equipment. (i.e. For a camera this could be a lens)")]
        [DbIgnore]
        public List<Guid> Part { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Organization.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Organization"/> class
    /// </summary>
    //[ApiDescription("Represents an organization")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Organization : NamedThing<Organization>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Organization"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Organization(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Organization"/> class.
        /// </summary>
        public Organization()
        {
        }


        ///<summary>
        /// The AddressRegion for this organization
        ///</summary>
        //[ApiDescription("The AddressRegion for this organization")]
        public Guid Address { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Department.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Department"/> class
    /// </summary>
    //[ApiDescription("Represents a department within an organization")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Department : NamedThing<Department>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Department"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Department(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Department"/> class.
        /// </summary>
        public Department()
        {
        }

        /// <summary>
        /// Gets the container of this <see cref="Department"/>
        /// </summary>
        //[ApiDescription("The container of this Department")]
        public Guid Organization { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Position.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Position"/> class
    /// </summary>
    //[ApiDescription("Represents a position within a department")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Position : NamedThing<Position>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Position(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Position"/> class.
        /// </summary>
        public Position()
        {
        }

        /// <summary>
        /// Gets the container of this <see cref="Position"/>
        /// </summary>
        //[ApiDescription("The container of this Position")]
        public Guid Department { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="User"/> class
    /// </summary>
    //[ApiDescription("Represents a user who has access to the sherlock's server")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class User : Thing<User>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public User(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
        }


        ///<summary>
        /// The username for this account used to log in
        ///</summary>
        //[ApiDescription("The username for this account used to log in")]
        public string Username { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="User"/>
        /// </summary>
        //[ApiDescription("The container of this User")]
        public Guid UserGroup { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminData.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="AdminData"/> class
    /// </summary>
    //[ApiDescription("Represents a super abstract type which all admin-data type shall derive from")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class AdminData<T> : DefinedThing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminData"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected AdminData(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminData"/> class.
        /// </summary>
        protected AdminData()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrderType.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrderType"/> class
    /// </summary>
    //[ApiDescription("")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrderType : AdminData<WorkOrderType>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderType"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrderType(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderType"/> class.
        /// </summary>
        public WorkOrderType()
        {
        }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public int FormSortOrder { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="WorkOrderType"/>
        /// </summary>
        //[ApiDescription("The container of this WorkOrderType")]
        public Guid ActivityClass { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentType.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EquipmentType"/> class
    /// </summary>
    //[ApiDescription("Represents an equipment-type")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EquipmentType : AdminData<EquipmentType>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentType"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EquipmentType(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentType"/> class.
        /// </summary>
        public EquipmentType()
        {
        }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public string EquipmentTypeCode { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="EquipmentType"/>
        /// </summary>
        //[ApiDescription("The container of this EquipmentType")]
        public Guid EquipmentClass { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreventiveMaintenanceType.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="PreventiveMaintenanceType"/> class
    /// </summary>
    //[ApiDescription("Represents a type of PreventiveMaintenance")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class PreventiveMaintenanceType : AdminData<PreventiveMaintenanceType>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreventiveMaintenanceType"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public PreventiveMaintenanceType(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreventiveMaintenanceType"/> class.
        /// </summary>
        public PreventiveMaintenanceType()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Site.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Site"/> class
    /// </summary>
    //[ApiDescription("Describes a Site")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Site : AdminData<Site>, IAddressThing	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Site(Guid iid) : base(iid: iid)	
        {
            this.Language = new List<Guid>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Site"/> class.
        /// </summary>
        public Site()
        {
            this.Language = new List<Guid>();

        }


        ///<summary>
        /// The abbreviation of this Site
        ///</summary>
        //[ApiDescription("The abbreviation of this Site")]
        public string Abbreviation { get; set; }


        ///<summary>
        /// The languages used on this Site
        ///</summary>
        //[ApiDescription("The languages used on this Site")]
        [DbIgnore]
        public List<Guid> Language { get; set; }


        ///<summary>
        /// The replication site for this Site
        ///</summary>
        //[ApiDescription("The replication site for this Site")]
        public Guid ReplicationSite { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="Site"/>
        /// </summary>
        //[ApiDescription("The container of this Site")]
        public Guid AddressRegion { get; set; }

        #region IAddressThing region

        ///<summary>
        /// The address care of
        ///</summary>
        //[ApiDescription("The address care of")]
        public string AddressCareOf { get; set; }


        ///<summary>
        /// The first line of the address
        ///</summary>
        //[ApiDescription("The first line of the address")]
        public string Line1 { get; set; }


        ///<summary>
        /// The second line of the address
        ///</summary>
        //[ApiDescription("The second line of the address")]
        public string Line2 { get; set; }


        ///<summary>
        /// The municipality of the address
        ///</summary>
        //[ApiDescription("The municipality of the address")]
        public string Municipality { get; set; }


        ///<summary>
        /// The post code of the address
        ///</summary>
        //[ApiDescription("The post code of the address")]
        public string PostCode { get; set; }

        #endregion	

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Country"/> class
    /// </summary>
    //[ApiDescription("Represents a country")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Country : TaxThing<Country>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Country(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Country"/> class.
        /// </summary>
        public Country()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddressRegion.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="AddressRegion"/> class
    /// </summary>
    //[ApiDescription("Represents an address region in a country")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class AddressRegion : TaxThing<AddressRegion>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddressRegion"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public AddressRegion(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressRegion"/> class.
        /// </summary>
        public AddressRegion()
        {
        }

        /// <summary>
        /// Gets the container of this <see cref="AddressRegion"/>
        /// </summary>
        //[ApiDescription("The container of this AddressRegion")]
        public Guid Country { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminRegion.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="AdminRegion"/> class
    /// </summary>
    //[ApiDescription("Represents the administrator region")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class AdminRegion : AdminData<AdminRegion>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRegion"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public AdminRegion(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminRegion"/> class.
        /// </summary>
        public AdminRegion()
        {
        }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public int ReportSortOrder { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplicationSite.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="ReplicationSite"/> class
    /// </summary>
    //[ApiDescription("The replication Site")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class ReplicationSite : AdminData<ReplicationSite>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplicationSite"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public ReplicationSite(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplicationSite"/> class.
        /// </summary>
        public ReplicationSite()
        {
        }


        ///<summary>
        /// Represents the type code for this ReplicationSite
        ///</summary>
        //[ApiDescription("Represents the type code for this ReplicationSite")]
        public string TypeCode { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="ReplicationSite"/>
        /// </summary>
        //[ApiDescription("The container of this ReplicationSite")]
        public Guid AdminRegion { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplicationToReplicationRule.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="ReplicationToReplicationRule"/> class
    /// </summary>
    //[ApiDescription("TODO")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class ReplicationToReplicationRule : AdminData<ReplicationToReplicationRule>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplicationToReplicationRule"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public ReplicationToReplicationRule(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplicationToReplicationRule"/> class.
        /// </summary>
        public ReplicationToReplicationRule()
        {
        }


        ///<summary>
        /// The Replication Site from which the transfer is taking place.
        ///</summary>
        //[ApiDescription("The Replication Site from which the transfer is taking place.")]
        public Guid FromReplicationSite { get; set; }


        ///<summary>
        /// The Replication Site to which the transfer is taking place.
        ///</summary>
        //[ApiDescription("The Replication Site to which the transfer is taking place.")]
        public Guid ToReplicationSite { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceClass.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="ResourceClass"/> class
    /// </summary>
    //[ApiDescription("Represents a Resource class.  (i.e. a Labour, a contract labour, a material, labour/material combo...)")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class ResourceClass : AdminData<ResourceClass>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceClass"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public ResourceClass(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceClass"/> class.
        /// </summary>
        public ResourceClass()
        {
        }


        ///<summary>
        /// The first resource type
        ///</summary>
        //[ApiDescription("The first resource type")]
        public Guid ResourceType1 { get; set; }


        ///<summary>
        /// The second resource type
        ///</summary>
        //[ApiDescription("The second resource type")]
        public Guid? ResourceType2 { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemAccount.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="SystemAccount"/> class
    /// </summary>
    //[ApiDescription("Describes a system account")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class SystemAccount : AdminData<SystemAccount>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAccount"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public SystemAccount(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemAccount"/> class.
        /// </summary>
        public SystemAccount()
        {
        }


        ///<summary>
        /// The site of the system-account
        ///</summary>
        //[ApiDescription("The site of the system-account")]
        public Guid Site { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="SystemAccount"/>
        /// </summary>
        //[ApiDescription("The container of this SystemAccount")]
        public Guid TopSystem { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubSystem.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="SubSystem"/> class
    /// </summary>
    //[ApiDescription("Describes a sub-system which is part of a TopSystem")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class SubSystem : AdminData<SubSystem>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubSystem"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public SubSystem(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubSystem"/> class.
        /// </summary>
        public SubSystem()
        {
        }


        ///<summary>
        /// Represents the sub-system code
        ///</summary>
        //[ApiDescription("Represents the sub-system code")]
        public string SubSystemCode { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="SubSystem"/>
        /// </summary>
        //[ApiDescription("The container of this SubSystem")]
        public Guid TopSystem { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopSystem.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="TopSystem"/> class
    /// </summary>
    //[ApiDescription("Describes a top level system")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class TopSystem : AdminData<TopSystem>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopSystem"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public TopSystem(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopSystem"/> class.
        /// </summary>
        public TopSystem()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Comment.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="Comment"/> class
    /// </summary>
    //[ApiDescription("Represents a comment that may be applied on any Thing")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class Comment<T> : Annotation<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected Comment(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Comment"/> class.
        /// </summary>
        protected Comment()
        {
        }

        /// <summary>
        /// Gets the container of this <see cref="Comment"/>
        /// </summary>
        //[ApiDescription("The container of this Comment")]
        public Guid Thing { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrderStatus.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrderStatus"/> class
    /// </summary>
    //[ApiDescription("")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrderStatus : AdminData<WorkOrderStatus>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderStatus"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrderStatus(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderStatus"/> class.
        /// </summary>
        public WorkOrderStatus()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrderHoldReason.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrderHoldReason"/> class
    /// </summary>
    //[ApiDescription("")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrderHoldReason : AdminData<WorkOrderHoldReason>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderHoldReason"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrderHoldReason(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderHoldReason"/> class.
        /// </summary>
        public WorkOrderHoldReason()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActivityClass.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="ActivityClass"/> class
    /// </summary>
    //[ApiDescription("")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class ActivityClass : AdminData<ActivityClass>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityClass"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public ActivityClass(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityClass"/> class.
        /// </summary>
        public ActivityClass()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrderPriority.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrderPriority"/> class
    /// </summary>
    //[ApiDescription("")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrderPriority : AdminData<WorkOrderPriority>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderPriority"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrderPriority(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderPriority"/> class.
        /// </summary>
        public WorkOrderPriority()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkOrderProblemCode.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="WorkOrderProblemCode"/> class
    /// </summary>
    //[ApiDescription("")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class WorkOrderProblemCode : AdminData<WorkOrderProblemCode>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderProblemCode"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public WorkOrderProblemCode(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WorkOrderProblemCode"/> class.
        /// </summary>
        public WorkOrderProblemCode()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentOwner.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EquipmentOwner"/> class
    /// </summary>
    //[ApiDescription("Represents an equipment owner")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EquipmentOwner : AdminData<EquipmentOwner>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentOwner"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EquipmentOwner(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentOwner"/> class.
        /// </summary>
        public EquipmentOwner()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentClass.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EquipmentClass"/> class
    /// </summary>
    //[ApiDescription("Represents an equipment class")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EquipmentClass : AdminData<EquipmentClass>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentClass"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EquipmentClass(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentClass"/> class.
        /// </summary>
        public EquipmentClass()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResourceType.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="ResourceType"/> class
    /// </summary>
    //[ApiDescription("Represents a resource type (i.e. a Labour, a contract labour, a material...)")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class ResourceType : AdminData<ResourceType>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceType"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public ResourceType(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceType"/> class.
        /// </summary>
        public ResourceType()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Language"/> class
    /// </summary>
    //[ApiDescription("Represents a language")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Language : AdminData<Language>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Language(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Maker.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Maker"/> class
    /// </summary>
    //[ApiDescription("Represents a manufacturer for an equipment-model")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Maker : AdminData<Maker>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Maker"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Maker(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Maker"/> class.
        /// </summary>
        public Maker()
        {
        }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Annotation.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="Annotation"/> class
    /// </summary>
    //[ApiDescription("An abstract type for annotations")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class Annotation<T> : UserEditableThing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Annotation"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected Annotation(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Annotation"/> class.
        /// </summary>
        protected Annotation()
        {
        }


        ///<summary>
        /// Represents the content of any annotation
        ///</summary>
        //[ApiDescription("Represents the content of any annotation")]
        public string Content { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Description.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Description"/> class
    /// </summary>
    //[ApiDescription("Represents a description that can be applied on any Thing with optionally the language information")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Description : Annotation<Description>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Description(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Description"/> class.
        /// </summary>
        public Description()
        {
        }


        ///<summary>
        /// Represents the optional language used for a description
        ///</summary>
        //[ApiDescription("Represents the optional language used for a description")]
        public Guid? Language { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="Description"/>
        /// </summary>
        //[ApiDescription("The container of this Description")]
        public Guid Thing { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaxThing.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="TaxThing"/> class
    /// </summary>
    //[ApiDescription("An abstract type for types that have a tax rate")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class TaxThing<T> : AdminData<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaxThing"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected TaxThing(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaxThing"/> class.
        /// </summary>
        protected TaxThing()
        {
        }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public float TaxRate { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentModel.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EquipmentModel"/> class
    /// </summary>
    //[ApiDescription("Represents an equipment-model")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EquipmentModel : AdminData<EquipmentModel>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentModel"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EquipmentModel(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentModel"/> class.
        /// </summary>
        public EquipmentModel()
        {
        }


        ///<summary>
        /// The name of the equipment model
        ///</summary>
        //[ApiDescription("The name of the equipment model")]
        public string Name { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public Guid EquipmentType { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="EquipmentModel"/>
        /// </summary>
        //[ApiDescription("The container of this EquipmentModel")]
        public Guid Maker { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Translation.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Translation"/> class
    /// </summary>
    //[ApiDescription("A translation between two different languages")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Translation : AdminData<Translation>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Translation(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        public Translation()
        {
        }


        ///<summary>
        /// The area where the translation is needed
        ///</summary>
        //[ApiDescription("The area where the translation is needed")]
        public string Area { get; set; }


        ///<summary>
        /// The sub-area where the translation is needed
        ///</summary>
        //[ApiDescription("The sub-area where the translation is needed")]
        public string SubArea { get; set; }


        ///<summary>
        /// The caption to translate
        ///</summary>
        //[ApiDescription("The caption to translate")]
        public string Caption { get; set; }


        ///<summary>
        /// The property to translate
        ///</summary>
        //[ApiDescription("The property to translate")]
        public string Property { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserGroup.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="UserGroup"/> class
    /// </summary>
    //[ApiDescription("Represents a group of users")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class UserGroup : Thing<UserGroup>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroup"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public UserGroup(Guid iid) : base(iid: iid)	
        {
            this.Claims = new List<string>();

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserGroup"/> class.
        /// </summary>
        public UserGroup()
        {
            this.Claims = new List<string>();

        }


        ///<summary>
        /// Represents the permission that a user-group has
        ///</summary>
        //[ApiDescription("Represents the permission that a user-group has")]
        public List<string> Claims { get; set; }


        ///<summary>
        /// The name of the user group
        ///</summary>
        //[ApiDescription("The name of the user group")]
        public string Name { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PreventiveMaintenance.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="PreventiveMaintenance"/> class
    /// </summary>
    //[ApiDescription("Represents a preventive maintenance that shall be performed on an equipment")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class PreventiveMaintenance : UserEditableThing<PreventiveMaintenance>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreventiveMaintenance"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public PreventiveMaintenance(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PreventiveMaintenance"/> class.
        /// </summary>
        public PreventiveMaintenance()
        {
        }


        ///<summary>
        /// The type of PreventiveMaintenance
        ///</summary>
        //[ApiDescription("The type of PreventiveMaintenance")]
        public Guid Type { get; set; }


        ///<summary>
        /// The frequency of the PreventiveMaintenance
        ///</summary>
        //[ApiDescription("The frequency of the PreventiveMaintenance")]
        public int Frequency { get; set; }


        ///<summary>
        /// The quantity of PreventiveMaintenance
        ///</summary>
        //[ApiDescription("The quantity of PreventiveMaintenance")]
        public int Quantity { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Property.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Property"/> class
    /// </summary>
    //[ApiDescription("Represents an extra field a user could input for an equipment")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Property : UserEditableThing<Property>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Property(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Property"/> class.
        /// </summary>
        public Property()
        {
        }


        ///<summary>
        /// The name of the extra optional property
        ///</summary>
        //[ApiDescription("The name of the extra optional property")]
        public string Name { get; set; }


        ///<summary>
        /// The value of the extra optional property
        ///</summary>
        //[ApiDescription("The value of the extra optional property")]
        public string Value { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="Property"/>
        /// </summary>
        //[ApiDescription("The container of this Property")]
        public Guid Equipment { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentTypeProperty.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="EquipmentTypeProperty"/> class
    /// </summary>
    //[ApiDescription("Represents an admin-custom property that may be added to an equipment-type. This is also used to map properties from the former data-model")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class EquipmentTypeProperty : AdminData<EquipmentTypeProperty>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentTypeProperty"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public EquipmentTypeProperty(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentTypeProperty"/> class.
        /// </summary>
        public EquipmentTypeProperty()
        {
        }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public string Name { get; set; }


        ///<summary>
        ///</summary>
        //[ApiDescription("")]
        public string Value { get; set; }

        /// <summary>
        /// Gets the container of this <see cref="EquipmentTypeProperty"/>
        /// </summary>
        //[ApiDescription("The container of this EquipmentTypeProperty")]
        public Guid EquipmentType { get; set; }

    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberThing.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="NumberThing"/> class
    /// </summary>
    //[ApiDescription("Represents a Thing that has a number")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class NumberThing<T> : UserEditableThing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberThing"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected NumberThing(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberThing"/> class.
        /// </summary>
        protected NumberThing()
        {
        }


        ///<summary>
        /// The number of this NumberThing
        ///</summary>
        //[ApiDescription("The number of this NumberThing")]
        public int Number { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Log.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;

    /// <summary>
    /// An Entity representation of the <see cref="Log"/> class
    /// </summary>
    //[ApiDescription("Represents a log of any operations.")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public partial class Log : Thing<Log>	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Log(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {
        }


        ///<summary>
        /// The thing that was created, modified or deleted.
        ///</summary>
        //[ApiDescription("The thing that was created, modified or deleted.")]
        public Guid Thing { get; set; }


        ///<summary>
        /// Represents the action of action that this Log logs
        ///</summary>
        //[ApiDescription("Represents the action of action that this Log logs")]
        public LogActionKind ActionKind { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefinedThing.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.CodeDom.Compiler;
    using System.Runtime.Serialization;

    

    using Redshift.Orm.Attributes;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// An Entity representation of the <see cref="DefinedThing"/> class
    /// </summary>
    //[ApiDescription("A super-type that contains specific properties of the legacy types")]
    [GeneratedCode("xmiparser","0.0.2.0")]
    public abstract partial class DefinedThing<T> : Thing<T> where T: IEntityObject	
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedThing"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        protected DefinedThing(Guid iid) : base(iid: iid)	
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefinedThing"/> class.
        /// </summary>
        protected DefinedThing()
        {
        }


        ///<summary>
        /// The code status of the current object. The status may show for example that an equipment is being in used or not
        ///</summary>
        //[ApiDescription("The code status of the current object. The status may show for example that an equipment is being in used or not")]
        public string CodeStatus { get; set; }


        ///<summary>
        /// The effective date of a Thing. This defaults to the entry date of the object
        ///</summary>
        //[ApiDescription("The effective date of a Thing. This defaults to the entry date of the object")]
        public DateTime? EffectiveDate { get; set; }


        ///<summary>
        /// The optional expiry date of this object
        ///</summary>
        //[ApiDescription("The optional expiry date of this object")]
        public DateTime? ExpiryDate { get; set; }


        ///<summary>
        /// Represents the user who created this IdentifiableThing
        ///</summary>
        //[ApiDescription("Represents the user who created this IdentifiableThing")]
        public Guid? CreatedBy { get; set; }


        ///<summary>
        /// Represents the user who last modified this IdentifiableThing
        ///</summary>
        //[ApiDescription("Represents the user who last modified this IdentifiableThing")]
        public Guid? ModifiedBy { get; set; }


        ///<summary>
        /// The legacy identifier used on the objects
        ///</summary>
        //[ApiDescription("The legacy identifier used on the objects")]
        public string LegacyId { get; set; }


    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAddressThing.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the equivalent interface of <see cref="IAddressThing"/> in the DTO space
    /// </summary>
    public partial interface IAddressThing    
    {
        string AddressCareOf { get; set; }
        string Line1 { get; set; }
        string Line2 { get; set; }
        string Municipality { get; set; }
        string PostCode { get; set; }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TelephoneKind.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    

    using Redshift.Orm.Attributes;

    ///<summary>
    /// Assertion representing the possible kind of telephone numbers
    ///</summary>
    //[ApiDescription("Assertion representing the possible kind of telephone numbers")]
    public enum TelephoneKind
    {

        ///<summary>
        /// Asserts that the telephone number is a work number
        ///</summary>
        //[ApiDescription("Asserts that the telephone number is a work number")]
        WORK,

        ///<summary>
        /// Asserts that the telephone number is a fax one
        ///</summary>
        //[ApiDescription("Asserts that the telephone number is a fax one")]
        FAX,

        ///<summary>
        /// Asserts that the telephone number is a cell number
        ///</summary>
        //[ApiDescription("Asserts that the telephone number is a cell number")]
        CELL,

        ///<summary>
        /// Asserts that the telephone number is a toll free number
        ///</summary>
        //[ApiDescription("Asserts that the telephone number is a toll free number")]
        TOLL_FREE
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PurchaseOrderType.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    

    using Redshift.Orm.Attributes;

    ///<summary>
    /// Assertion defining the purchase order type. This defaults to “CSC” and you would normally just go to the Purchase Order field and enter the PO#, but if the repair action for example requires a Motorola Case number then you would select “Other” and then you can enter the case number in the Purchase Order field.
    ///</summary>
    //[ApiDescription("Assertion defining the purchase order type. This defaults to “CSC” and you would normally just go to the Purchase Order field and enter the PO#, but if the repair action for example requires a Motorola Case number then you would select “Other” and then you can enter the case number in the Purchase Order field.")]
    public enum PurchaseOrderType
    {

        ///<summary>
        /// Asserts that the purchase order only needs a normal purchase order number
        ///</summary>
        //[ApiDescription("Asserts that the purchase order only needs a normal purchase order number")]
        CSC,

        ///<summary>
        /// Asserts that the purchase order requires a special number to be used (i.e. a Motorola Case number)
        ///</summary>
        //[ApiDescription("Asserts that the purchase order requires a special number to be used (i.e. a Motorola Case number)")]
        OTHER
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentLevel.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    

    using Redshift.Orm.Attributes;

    ///<summary>
    /// Assertion that describes how an equipment should be tracked
    ///</summary>
    //[ApiDescription("Assertion that describes how an equipment should be tracked")]
    public enum EquipmentLevel
    {

        ///<summary>
        /// Asserts that an equipment should be tracked individually
        ///</summary>
        //[ApiDescription("Asserts that an equipment should be tracked individually")]
        SINGLE,

        ///<summary>
        /// Asserts that an equipment is tracked as a group (i.e. a group of several speakers)
        ///</summary>
        //[ApiDescription("Asserts that an equipment is tracked as a group (i.e. a group of several speakers)")]
        GROUP,

        ///<summary>
        /// This assertion is reserved for Preventive Maintenance Work Order
        ///</summary>
        //[ApiDescription("This assertion is reserved for Preventive Maintenance Work Order")]
        DUMMY_POOL
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClaimKind.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    

    using Redshift.Orm.Attributes;

    ///<summary>
    /// Assertion that describe the kind of data that a UserGroup can manipulate. Its litterals are composed by all existing concrete Types.
    ///</summary>
    //[ApiDescription("Assertion that describe the kind of data that a UserGroup can manipulate. Its litterals are composed by all existing concrete Types.")]
    public enum ClaimKind
    {

        ///<summary>
        /// Asserts that the write permission is granted for the ActivityClass class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the ActivityClass class")]
        ActivityClassEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the ActivityClass class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the ActivityClass class")]
        ActivityClassDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the ActualTaskEffort class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the ActualTaskEffort class")]
        ActualTaskEffortEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the ActualTaskEffort class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the ActualTaskEffort class")]
        ActualTaskEffortDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the AddressRegion class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the AddressRegion class")]
        AddressRegionEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the AddressRegion class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the AddressRegion class")]
        AddressRegionDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the AdminRegion class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the AdminRegion class")]
        AdminRegionEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the AdminRegion class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the AdminRegion class")]
        AdminRegionDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Client class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Client class")]
        ClientEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Client class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Client class")]
        ClientDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Country class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Country class")]
        CountryEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Country class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Country class")]
        CountryDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Department class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Department class")]
        DepartmentEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Department class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Department class")]
        DepartmentDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Description class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Description class")]
        DescriptionEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Description class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Description class")]
        DescriptionDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EmailAddress class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EmailAddress class")]
        EmailAddressEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EmailAddress class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EmailAddress class")]
        EmailAddressDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Equipment class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Equipment class")]
        EquipmentEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Equipment class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Equipment class")]
        EquipmentDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EquipmentClass class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EquipmentClass class")]
        EquipmentClassEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EquipmentClass class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EquipmentClass class")]
        EquipmentClassDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EquipmentModel class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EquipmentModel class")]
        EquipmentModelEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EquipmentModel class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EquipmentModel class")]
        EquipmentModelDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EquipmentOwner class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EquipmentOwner class")]
        EquipmentOwnerEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EquipmentOwner class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EquipmentOwner class")]
        EquipmentOwnerDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EquipmentType class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EquipmentType class")]
        EquipmentTypeEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EquipmentType class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EquipmentType class")]
        EquipmentTypeDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EquipmentTypeProperty class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EquipmentTypeProperty class")]
        EquipmentTypePropertyEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EquipmentTypeProperty class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EquipmentTypeProperty class")]
        EquipmentTypePropertyDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the EstimatedTaskEffort class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the EstimatedTaskEffort class")]
        EstimatedTaskEffortEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the EstimatedTaskEffort class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the EstimatedTaskEffort class")]
        EstimatedTaskEffortDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Language class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Language class")]
        LanguageEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Language class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Language class")]
        LanguageDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Log class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Log class")]
        LogEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Log class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Log class")]
        LogDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Maker class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Maker class")]
        MakerEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Maker class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Maker class")]
        MakerDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Organization class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Organization class")]
        OrganizationEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Organization class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Organization class")]
        OrganizationDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Position class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Position class")]
        PositionEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Position class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Position class")]
        PositionDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the PreventiveMaintenance class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the PreventiveMaintenance class")]
        PreventiveMaintenanceEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the PreventiveMaintenance class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the PreventiveMaintenance class")]
        PreventiveMaintenanceDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the PreventiveMaintenanceType class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the PreventiveMaintenanceType class")]
        PreventiveMaintenanceTypeEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the PreventiveMaintenanceType class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the PreventiveMaintenanceType class")]
        PreventiveMaintenanceTypeDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Property class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Property class")]
        PropertyEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Property class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Property class")]
        PropertyDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the ReplicationSite class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the ReplicationSite class")]
        ReplicationSiteEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the ReplicationSite class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the ReplicationSite class")]
        ReplicationSiteDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the ReplicationToReplicationRule class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the ReplicationToReplicationRule class")]
        ReplicationToReplicationRuleEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the ReplicationToReplicationRule class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the ReplicationToReplicationRule class")]
        ReplicationToReplicationRuleDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Resource class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Resource class")]
        ResourceEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Resource class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Resource class")]
        ResourceDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the ResourceClass class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the ResourceClass class")]
        ResourceClassEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the ResourceClass class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the ResourceClass class")]
        ResourceClassDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the ResourceType class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the ResourceType class")]
        ResourceTypeEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the ResourceType class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the ResourceType class")]
        ResourceTypeDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Site class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Site class")]
        SiteEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Site class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Site class")]
        SiteDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the SubSystem class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the SubSystem class")]
        SubSystemEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the SubSystem class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the SubSystem class")]
        SubSystemDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the SystemAccount class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the SystemAccount class")]
        SystemAccountEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the SystemAccount class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the SystemAccount class")]
        SystemAccountDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the TelephoneNumber class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the TelephoneNumber class")]
        TelephoneNumberEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the TelephoneNumber class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the TelephoneNumber class")]
        TelephoneNumberDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the TopSystem class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the TopSystem class")]
        TopSystemEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the TopSystem class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the TopSystem class")]
        TopSystemDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the Translation class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the Translation class")]
        TranslationEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the Translation class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the Translation class")]
        TranslationDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the User class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the User class")]
        UserEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the User class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the User class")]
        UserDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the UserGroup class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the UserGroup class")]
        UserGroupEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the UserGroup class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the UserGroup class")]
        UserGroupDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrder class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrder class")]
        WorkOrderEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrder class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrder class")]
        WorkOrderDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrderHoldReason class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrderHoldReason class")]
        WorkOrderHoldReasonEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrderHoldReason class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrderHoldReason class")]
        WorkOrderHoldReasonDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrderPriority class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrderPriority class")]
        WorkOrderPriorityEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrderPriority class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrderPriority class")]
        WorkOrderPriorityDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrderProblemCode class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrderProblemCode class")]
        WorkOrderProblemCodeEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrderProblemCode class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrderProblemCode class")]
        WorkOrderProblemCodeDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrderStatus class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrderStatus class")]
        WorkOrderStatusEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrderStatus class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrderStatus class")]
        WorkOrderStatusDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrderTask class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrderTask class")]
        WorkOrderTaskEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrderTask class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrderTask class")]
        WorkOrderTaskDelete,

        ///<summary>
        /// Asserts that the write permission is granted for the WorkOrderType class
        ///</summary>
        //[ApiDescription("Asserts that the write permission is granted for the WorkOrderType class")]
        WorkOrderTypeEdit,

        ///<summary>
        /// Asserts that hard delete permission is granted for the WorkOrderType class
        ///</summary>
        //[ApiDescription("Asserts that hard delete permission is granted for the WorkOrderType class")]
        WorkOrderTypeDelete
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogActionKind.cs" company="RHEA S.A.">
//   Copyright (c) 2017 RHEA S.A.
// </copyright>
// <summary>
//   This is an auto-generated class. Any manual changes on this file will be overwritten!
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Redshift.Orm.Tests
{
    

    using Redshift.Orm.Attributes;

    ///<summary>
    /// Assertion that represents the type of action that were performed on a Thing
    ///</summary>
    //[ApiDescription("Assertion that represents the type of action that were performed on a Thing")]
    public enum LogActionKind
    {

        ///<summary>
        /// Asserts that a Thing was created
        ///</summary>
        //[ApiDescription("Asserts that a Thing was created")]
        CREATE,

        ///<summary>
        /// Asserts that a Thing was updated
        ///</summary>
        //[ApiDescription("Asserts that a Thing was updated")]
        UPDATE,

        ///<summary>
        /// Asserts that a thing was deleted
        ///</summary>
        //[ApiDescription("Asserts that a thing was deleted")]
        DELETE
    }
}

