#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiBaseModule.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Seed.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Seed.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Api;
    using Api.Helpers;
    using Api.Json;
    using Model;
    using Nancy;
    using Nancy.Responses.Negotiation;
    using Newtonsoft.Json;
    using Orm.Database;
    using Orm.EntityObject;

    /// <summary>
    /// Api base module, handles gets on abstract types
    /// </summary>
    public class ApiBaseModule : NancyModule
    {
        /// <summary>
        /// API base module
        /// </summary>
        public ApiBaseModule() : base("/v1")
        {
            this.Get(
                "/",
                x => { return "Hello API!"; });

            this.Get(
                "/Thing",
                x => { return this.ResolveAbstractGet("Thing"); });

            // an example of generalized DELETE
            this.Delete(
                "/{entity}/{uuid:guid}"
                ,
                x =>
                {
                    // parse id
                    var uuid = ApiHelper.GetIdFromString(x.uuid.ToString());

                    // parse entity
                    var entity = x.entity.ToString();
                    var type = ApiHelper.GetEntityTypeFromName(@"Redshift.Seed.Model", entity, "Redshift.Seed");

                    var typeErrors = ApiHelper.GetTypeErrors(
                        this.Negotiate,
                        entity,
                        type,
                        out List<Attribute> attributes,
                        this.Context);

                    if (typeErrors != null)
                    {
                        return typeErrors;
                    }

                    // acquire the object to delete
                    dynamic objectToDelete;

                    try
                    {
                        objectToDelete = (ISeedObject)
                            type.GetMethod(
                                    "Find",
                                    BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                                .Invoke(null, new[] {uuid});
                    }
                    catch (Exception e)
                    {
                        return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                    }

                    if (objectToDelete == null)
                    {
                        return ApiHelper.ConstructFailResponse(
                            this.Negotiate,
                            "uuid",
                            string.Format("The requested {1} with id {0} does not exist.", uuid, entity),
                            this.Context);
                    }

                    // object gets deleted
                    try
                    {
                        objectToDelete.Delete();
                    }
                    catch (Exception e)
                    {
                        return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                    }

                    return ApiHelper.ConstructSuccessResponse(this.Negotiate, new ResponseContainer(), this.Context);
                });
        }

        /// <summary>
        /// Processes the query parameters.
        /// </summary>
        /// <param name="request">
        /// The request.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="QueryParameterContainer"/>.
        /// </returns>
        protected QueryParameterContainer ProcessQueryParameters(Request request, Type type)
        {
            var result = new QueryParameterContainer();

            result.Process(request, type);

            return result;
        }

        /// <summary>
        /// Resolves and delivers a GET request on an abstract supertype.
        /// </summary>
        /// <param name="entity">The abstract entity.</param>
        /// <returns>The response with all concrete entities.</returns>
        private Negotiator ResolveAbstractGet(string entity)
        {
            var types = EntityResolverMap.AbstractToConcreteMap[entity];
            var response = new ResponseContainer();

            var filterDictionary = new Dictionary<Type, QueryParameterContainer>();

            foreach (var type in types)
            {
                try
                {
                    var queryParams = this.ProcessQueryParameters(this.Request, type);
                    filterDictionary.Add(type, queryParams);
                }
                catch (Exception)
                {
                    return ApiHelper.ConstructFailResponse(
                        this.Negotiate,
                        entity,
                        "The query parameters are badly formatted and cannot be parsed.",
                        this.Context,
                        HttpStatusCode.BadRequest);
                }
            }

            foreach (var type in types)
            {
                if (filterDictionary.Values.Any(x => x.IsFiltered) && !filterDictionary[type].IsFiltered)
                {
                    // if some typpe is filtered but this type is not, exclude it
                    continue;
                }

                var typeErrors = ApiHelper.GetTypeErrors(this.Negotiate, entity, type, out var attributes, this.Context);

                if (typeErrors != null)
                {
                    continue;
                }

                List<IEntityObject> resp;

                try
                {
                    resp =
                    ((IEnumerable<IEntityObject>)
                        type.GetMethod(
                                "Where",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy,
                                null,
                                new[]
                                {
                                    typeof(List<IWhereQueryContainer>), typeof(int?), typeof(int?),
                                    typeof(PropertyInfo), typeof(bool)
                                },
                                null)
                            .Invoke(
                                null,
                                new object[]
                                {
                                    filterDictionary[type].FilterList, filterDictionary[type].Limit, filterDictionary[type].Offset,
                                    filterDictionary[type].OrderProperty, filterDictionary[type].IsDescending
                                })).ToList();
                }
                catch (Exception e)
                {
                    return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                }

                response.AddToResponse(resp);
            }

            return ApiHelper.ConstructSuccessResponse(this.Negotiate, response, this.Context);
        }

        /// <summary>
        /// Performs a simple POST operation composed of one object corresponding to the entity type that the object was sent to.
        /// </summary>
        /// <param name="type">The type of object that needs to be resolved.</param>
        /// <param name="postBody">The POST body to be processed.</param>
        /// <returns>The response <see cref="Negotiator"/> with either a success or failure.</returns>
        protected Negotiator PerformSimplePost(dynamic type, string postBody)
        {
            var instance = Activator.CreateInstance(type);
            IEntityObject savedThing;

            try
            {
                JsonConvert.PopulateObject(postBody, instance);
            }
            catch (Exception e)
            {
                return ApiHelper.ConstructFailResponse(this.Negotiate, "create",
                    "The request was not properly formatted and could not be used to create the object!", this.Context,
                    HttpStatusCode.BadRequest, e);
            }

            // give a new uuid to the instance
            instance.Uuid = Guid.NewGuid();

            try
            {
                // acquire the response
                IEntityObject resp;

                try
                {
                    resp =
                        (IEntityObject) type.GetMethod("Find",
                                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                            .Invoke(null, new[] {instance.Uuid});

                    if (resp != null)
                    {
                        // if there is an object of this type in the database then this POST is invalid
                        return ApiHelper.ConstructFailResponse(this.Negotiate, type.Name,
                            "An object with this Uuid already exists and cannot be created!", this.Context,
                            HttpStatusCode.Conflict);
                    }
                }
                catch (Exception e)
                {
                    return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                }

                var transaction = DatabaseSession.Instance.CreateTransaction();
                savedThing = instance.Save(transaction: transaction);
                DatabaseSession.Instance.CommitTransaction(transaction);
            }
            catch (Exception e)
            {
                return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
            }

            var container = new ResponseContainer();
            container.AddToResponse(savedThing);

            return ApiHelper.ConstructSuccessResponse(this.Negotiate, container, this.Context,
                HttpStatusCode.Created);
        }
    }
}
