﻿#region Disclaimer / License
// Copyright (C) 2010, Jackie Ng
// http://trac.osgeo.org/mapguide/wiki/maestro, jumpinjackie@gmail.com
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using ObjCommon = OSGeo.MapGuide.ObjectModels.Common;
using OSGeo.MapGuide.MaestroAPI.Schema;
using OSGeo.MapGuide.MaestroAPI.Feature;

namespace OSGeo.MapGuide.MaestroAPI.Services
{
    /// <summary>
    /// Provides services for accessing, querying and inspecting feature sources
    /// </summary>
    /// <remarks>
    /// Note that <see cref="T:OSGeo.MapGuide.MaestroAPI.IServerConnection"/> provides
    /// built-in access to resource and feature services. Using the <see cref="M:OSGeo.MapGuide.MaestroAPI.IServerConnection.GetService"/>
    /// method is not necessary
    /// </remarks>
    public interface IFeatureService : IService
    {
        /// <summary>
        /// Gets the capabilities of the specified provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        OSGeo.MapGuide.ObjectModels.Capabilities.FdoProviderCapabilities GetProviderCapabilities(string provider);

        /// <summary>
        /// Gets an array of all registered providers
        /// </summary>
        ObjCommon.FeatureProviderRegistryFeatureProvider[] FeatureProviders { get; }

        /// <summary>
        /// Tests the specified connection settings
        /// </summary>
        /// <param name="providername"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string TestConnection(string providername, NameValueCollection parameters);

        /// <summary>
        /// Tests the connection settings of the specified feature source
        /// </summary>
        /// <param name="featureSourceId"></param>
        /// <returns></returns>
        string TestConnection(string featureSourceId);

        /// <summary>
        /// Removes the version numbers from a providername
        /// </summary>
        /// <param name="providername">The name of the provider, with or without version numbers</param>
        /// <returns>The provider name without version numbers</returns>
        string RemoveVersionFromProviderName(string providername);

        /// <summary>
        /// Gets the possible values for a given connection property
        /// </summary>
        /// <param name="providerName">The FDO provider name</param>
        /// <param name="propertyName">The property name</param>
        /// <param name="partialConnectionString">A partial connection string if certain providers require such information</param>
        /// <returns>A list of possible values for the given property</returns>
        string[] GetConnectionPropertyValues(string providerName, string propertyName, string partialConnectionString);

        /// <summary>
        /// Returns an installed provider, given the name of the provider
        /// </summary>
        /// <param name="providername">The name of the provider</param>
        /// <returns>The first matching provider or null</returns>
        ObjCommon.FeatureProviderRegistryFeatureProvider GetFeatureProvider(string providername);

        /// <summary>
        /// Executes a SQL query
        /// </summary>
        /// <param name="featureSourceID"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        IReader ExecuteSqlQuery(string featureSourceID, string sql);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="className"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className, string filter);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="className"></param>
        /// <param name="filter"></param>
        /// <param name="propertyNames"></param>
        /// <returns></returns>
        IFeatureReader QueryFeatureSource(string resourceID, string className, string filter, string[] propertyNames);

        /// <summary>
        /// Executes a feature query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="query"></param>
        /// <param name="columns"></param>
        /// <param name="computedProperties"></param>
        /// <returns></returns>
        IFeatureReader QueryFeatureSource(string resourceID, string schema, string query, string[] columns, NameValueCollection computedProperties);

        /// <summary>
        /// Executes an aggregate query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter);

        /// <summary>
        /// Executes an aggregate query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="filter"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, string[] columns);

        /// <summary>
        /// Executes an aggregate query on the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="filter"></param>
        /// <param name="aggregateFunctions"></param>
        /// <returns></returns>
        IReader AggregateQueryFeatureSource(string resourceID, string schema, string filter, NameValueCollection aggregateFunctions);

        /// <summary>
        /// Gets the geometric extent of the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="geometry"></param>
        /// <returns></returns>
        ObjCommon.IEnvelope GetSpatialExtent(string resourceID, string schema, string geometry);

        /// <summary>
        /// Gets the geometric extent of the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="geometry"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        ObjCommon.IEnvelope GetSpatialExtent(string resourceID, string schema, string geometry, string filter);

        /// <summary>
        /// Gets the geometric extent of the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <param name="geometry"></param>
        /// <param name="allowFallbackToContextInformation"></param>
        /// <exception cref="T:OSGeo.MapGuide.MaestroAPI.Exceptions.NullExtentException">Thrown if the geometric extent is null</exception>
        /// <returns></returns>
        ObjCommon.IEnvelope GetSpatialExtent(string resourceID, string schema, string geometry, bool allowFallbackToContextInformation);

        /// <summary>
        /// Describes the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <remarks>
        /// If you only need to list schemas and class names, use the respective <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetSchemas" /> and
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetClassNames" /> methods. Using this API will have a noticeable performance impact on 
        /// really large datastores (whose size is in the 100s of classes).
        /// </remarks>
        /// <returns></returns>
        FeatureSourceDescription DescribeFeatureSource(string resourceID);

        /// <summary>
        /// Describes the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="schema"></param>
        /// <remarks>
        /// If you only need to list schemas and class names, use the respective <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetSchemas" /> and
        /// <see cref="M:OSGeo.MapGuide.MaestroAPI.Services.IFeatureService.GetClassNames" /> methods. Using this API will have a noticeable performance impact on 
        /// really large datastores (whose size is in the 100s of classes).
        /// </remarks>
        /// <returns></returns>
        FeatureSchema DescribeFeatureSource(string resourceID, string schema);

        /// <summary>
        /// Gets the specified class definition
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="className">The feature class name. You can pass a qualified class name to be explicit about which class definition you are after</param>
        /// <returns></returns>
        ClassDefinition GetClassDefinition(string resourceID, string className);

        /// <summary>
        /// Get the spatial context information for the specified feature source
        /// </summary>
        /// <param name="resourceID"></param>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        ObjCommon.FdoSpatialContextList GetSpatialContextInfo(string resourceID, bool activeOnly);

        /// <summary>
        /// Gets the names of the identity properties from a feature
        /// </summary>
        /// <param name="resourceID">The resourceID for the FeatureSource</param>
        /// <param name="classname">The classname of the feature, including schema</param>
        /// <returns>A string array with the found identities</returns>
        string[] GetIdentityProperties(string resourceID, string classname);

        /// <summary>
        /// Enumerates all the data stores and if they are FDO enabled for the specified provider and partial connection string
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="partialConnString"></param>
        /// <returns></returns>
        OSGeo.MapGuide.ObjectModels.Common.DataStoreList EnumerateDataStores(string providerName, string partialConnString);

        /// <summary>
        /// Gets an array of schema names from the specified feature source
        /// </summary>
        /// <param name="resourceId"></param>
        /// <returns></returns>
        string[] GetSchemas(string resourceId);

        /// <summary>
        /// Gets an array of qualified feature class names from the specified feature source
        /// </summary>
        /// <param name="resourceId">The feature source id</param>
        /// <param name="schemaName">
        /// The name of the schema whose class names are to be returned. If null, class names from all schemas in the feature source
        /// are returned
        /// </param>
        /// <returns></returns>
        string[] GetClassNames(string resourceId, string schemaName);
    }
}
