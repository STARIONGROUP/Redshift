#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppDomain.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.
//
//    Redshift.Orm is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Orm.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    /// <summary>
    /// Polyfill for deprecated AppDomain .net framework class
    /// See here: <see href="http://www.michael-whelan.net/replacing-appdomain-in-dotnet-core/"/>
    /// </summary>
    public class AppDomain
    {
        /// <summary>
        /// Initializes static members of the <see cref="AppDomain"/> class. 
        /// </summary>
        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        /// <summary>
        /// Gets the current application domain.
        /// </summary>
        public static AppDomain CurrentDomain { get; private set; }

        /// <summary>
        /// Gets the directory of the entry assembly.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/> path to the entry assembly.
        /// </returns>
        public static string AssemblyDirectory()
        {
            var codeBase = typeof(AppDomain).GetTypeInfo().Assembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Gets all the assemblies of the application domain.
        /// </summary>
        /// <param name="applicationNamespace">
        /// The application Namespace.
        /// </param>
        /// <returns>
        /// An array of <see cref="Assembly"/>.
        /// </returns>
        public Assembly[] GetAssemblies(string applicationNamespace)
        {
            var assemblies = new List<Assembly>();

            foreach (var library in System.AppDomain.CurrentDomain.GetAssemblies())
            {
                if (IsCandidateAssembly(library, applicationNamespace))
                {
                    assemblies.Add(library);
                }
            }

            return assemblies.ToArray();
        }

        /// <summary>
        /// Filters only the assemblies that are directly related to the application.
        /// </summary>
        /// <param name="assembly">
        /// The Assembly being filtered.
        /// </param>
        /// <param name="applicationNamespace">
        /// The application Namespace.
        /// </param>
        /// <returns>
        /// True if the <see cref="Assembly"/> should be kept.
        /// </returns>
        private static bool IsCandidateAssembly(Assembly assembly, string applicationNamespace)
        {
            return assembly.FullName == applicationNamespace
                || assembly.FullName.StartsWith(applicationNamespace);
        }
    }
}
