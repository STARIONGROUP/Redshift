namespace Redshift.Seed.Model
{
    using System.Collections.Generic;

    public class Usergroup : Thing<Usergroup>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Usergroup"/>
        /// </summary>
        public Usergroup()
        {
            this.Permissions = new List<string>();
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the permissions.
        /// </summary>
        public List<string> Permissions { get; set; }
    }
}
