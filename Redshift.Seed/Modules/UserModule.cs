#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserModule.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
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
    using Api.Helpers;
    using Api.Json;
    using Model;
    using Nancy;
    using Nancy.Extensions;
    using Nancy.IO;
    using Newtonsoft.Json;
    using Orm.Database;
    using Orm.EntityObject;

    /// <summary>
    /// User user
    /// </summary>
    public class UserModule : ApiBaseModule
    {
        /// <summary>
        /// Usergroup module
        /// </summary>
        public UserModule()
        {
            // get
            this.Get(
                "/User",
                x =>
                {
                    var entity = "User";

                    var type = ApiHelper.GetEntityTypeFromName(@"Redshift.Seed.Model", entity, "Redshift.Seed");

                    List<Attribute> attributes;

                    var typeErrors =
                        ApiHelper.GetTypeErrors(this.Negotiate, entity, type, out attributes, this.Context);

                    if (typeErrors != null)
                    {
                        return typeErrors;
                    }

                    List<User> resp;
                    QueryParameterContainer queryParams;
                    var count = -1L;

                    try
                    {
                        queryParams = this.ProcessQueryParameters(this.Request, type);
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

                    try
                    {
                        if (queryParams.IsFiltered && !queryParams.IsPaginated)
                        {
                            resp = User.Where(queryParams.FilterList, null, null, queryParams.OrderProperty,
                                queryParams.IsDescending);

                            if (queryParams.IsCountExpected)
                            {
                                // all entities are already queried and in memory so simple count is fine
                                count = resp.Count;
                            }
                        }
                        else if (queryParams.IsPaginated)
                        {
                            // when paginating, always return count
                            queryParams.IsCountExpected = true;

                            resp = User.Where(queryParams.FilterList, queryParams.Limit, queryParams.Offset,
                                queryParams.OrderProperty, queryParams.IsDescending);

                            // paginated response provide count always and the count is of total filtered records
                            count = User.CountWhere(queryParams.FilterList);
                        }
                        else
                        {
                            resp = User.Where(queryParams.FilterList, null, null, null, true);

                            if (queryParams.IsCountExpected)
                            {
                                // all entities are already queried and in memory so simple count is fine
                                count = resp.Count;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                    }

                    var response = new ResponseContainer();
                    response.AddToResponse(resp);

                    // add count to the response
                    if (queryParams.IsCountExpected)
                    {
                        response.Add("count", new List<object> {count});
                    }

                    return ApiHelper.ConstructSuccessResponse(this.Negotiate, response, this.Context);
                });

            this.Get(
                "/User/{uuid:guid}",
                x =>
                {
                    // parse id
                    var uuid = ApiHelper.GetIdFromString(x.uuid.ToString());

                    if (uuid == null)
                    {
                        return ApiHelper.ConstructFailResponse(
                            this.Negotiate,
                            "uuid",
                            string.Format("The requested uuid {0} cannot be parsed.", x.uuid),
                            this.Context,
                            HttpStatusCode.BadRequest);
                    }

                    // parse entity
                    var entity = "User";

                    var type = ApiHelper.GetEntityTypeFromName(@"Redshift.Seed.Model", entity, "Redshift.Seed");

                    List<Attribute> attributes;
                    var typeErrors =
                        ApiHelper.GetTypeErrors(this.Negotiate, entity, type, out attributes, this.Context);

                    if (typeErrors != null)
                    {
                        return typeErrors;
                    }

                    // acquire the response
                    User resp;

                    try
                    {
                        resp = User.Find(uuid);
                    }
                    catch (Exception e)
                    {
                        return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                    }

                    if (resp == null)
                    {
                        return ApiHelper.ConstructFailResponse(
                            this.Negotiate,
                            "uuid",
                            string.Format("The requested {1} with id {0} does not exist.", uuid, entity),
                            this.Context);
                    }

                    var response = new ResponseContainer();
                    response.AddToResponse(resp);

                    return ApiHelper.ConstructSuccessResponse(this.Negotiate, response, this.Context);

                });

            this.Post(
                "/User",
                x =>
                {
                    var postBody = RequestStream.FromStream(this.Request.Body).AsString();

                    var entity = "User";
                    var type = ApiHelper.GetEntityTypeFromName(@"Redshift.Seed.Model", entity, "Redshift.Seed");

                    List<Attribute> attributes;

                    var typeErrors = ApiHelper.GetTypeErrors(
                        this.Negotiate,
                        entity,
                        type,
                        out attributes,
                        this.Context);

                    if (typeErrors != null)
                    {
                        return typeErrors;
                    }

                    // demoinstrates a way to generalize 
                    return this.PerformSimplePost(type, postBody);
                });

            this.Put(
                "/User/{uuid:guid}",
                x =>
                {
                    var uuid = ApiHelper.GetIdFromString(x.uuid.ToString());
                    var putBody = RequestStream.FromStream(Request.Body).AsString();

                    var entity = "User";
                    var type = ApiHelper.GetEntityTypeFromName(@"Redshift.Seed.Model", entity, "Redshift.Seed");

                    var typeErrors = ApiHelper.GetTypeErrors(
                        this.Negotiate,
                        entity,
                        type,
                        out var attributes,
                        this.Context);

                    if (typeErrors != null)
                    {
                        return typeErrors;
                    }

                    IEntityObject savedResp;

                    try
                    {
                        // acquire the response
                        User resp;
                        
                        try
                        {
                            resp = User.Find(uuid);

                            if (resp != null)
                            {
                                try
                                {
                                    JsonConvert.PopulateObject(
                                        putBody,
                                        resp);

                                    // make sure the uuid is set correctly/not being changed.
                                    resp.Uuid = uuid;
                                }
                                catch (Exception e)
                                {
                                    return ApiHelper.ConstructFailResponse(this.Negotiate, "update",
                                        "The request was not properly formatted and could not be used to update the object!",
                                        this.Context, HttpStatusCode.BadRequest, e);
                                }
                            }
                            else
                            {
                                // if there is an object of this type in the database then this POST is invalid
                                return ApiHelper.ConstructFailResponse(
                                    this.Negotiate,
                                    type.Name,
                                    "An object with this Uuid does not exist and cannot be updated!",
                                    this.Context,
                                    HttpStatusCode.NotFound);
                            }
                        }
                        catch (Exception e)
                        {
                            return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                        }

                        var transaction = DatabaseSession.Instance.CreateTransaction();
                        savedResp = resp.Save(transaction: transaction);
                        DatabaseSession.Instance.CommitTransaction(transaction);
                    }
                    catch (Exception e)
                    {
                        return ApiHelper.ConstructErrorResponse(this.Negotiate, e, this.Context);
                    }

                    var container = new ResponseContainer();
                    container.AddToResponse(savedResp);

                    return ApiHelper.ConstructSuccessResponse(this.Negotiate, container, this.Context);
                });
        }
    }
}
