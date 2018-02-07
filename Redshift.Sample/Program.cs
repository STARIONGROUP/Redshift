#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Sample.
//
//    Redshift.Sample is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Sample is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Sample.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Redshift.Sample.Model;

namespace Redshift.Sample
{
    using System.Collections.Generic;
    using Orm.Database;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var user = new User
            {
                Id = 1,
                Username = "alex",
                Password = "123",
                Email = "bla@bla.com"
            };

            // create the user
            user.Save();

            // update the user
            user.Username = "alexv";
            user.Save();

            // delete the user
            user.Delete();

            // gets all instances of User
            var users = User.All();

            // gets all instances of User ordered
            var usersOrder = User.All(typeof(User).GetProperty("Username"), orderDescending: true);

            // gets user with Id 1
            var user1 = User.Find(1);

            // gets the first 10 users ordered by decending Username
            var usersLimit = User.Subset(10, 0, typeof(User).GetProperty("Username"), true);

            // simple where
            var userWhere1 = User.Where(typeof(User).GetProperty("Username"), "=", "alexv");

            // simple where with limit
            var userWhereLimit = User.Where(typeof(User).GetProperty("Username"), "=", "alexv", limit: 10, offset: 10, orderBy: typeof(User).GetProperty("Email"), orderDescending: false);

            // where clause containers, any number of values can be provided and by default the OR statement is considered between each value. Setting IsUsingAndConditionBetweenValues to true changes the behavior
            var whereClause1 = new WhereQueryContainer()
            {
                Comparer = "=",
                Property = typeof(User).GetProperty("Email"),
                IsUsingAndConditionBetweenValues = false,
                Value = new List<object>() { "bla@bla.com", "some@email.com" }
            };

            var whereClause2 = new WhereQueryContainer()
            {
                Comparer = ">",
                Property = typeof(User).GetProperty("Id"),
                Value = new List<object>() { 1 }
            };

            // complex where, similarly can be used with limits
            var userWhere2 = User.Where(new List<IWhereQueryContainer> { whereClause1, whereClause2 });

            // count all users
            var userCount = User.Count();

            // count number of users where conditions are met
            var userCountWhere = User.CountWhere(new List<IWhereQueryContainer> { whereClause1, whereClause2 });
            
            // gets a clean User object
            var template = User.Template();

        }
    }
}
