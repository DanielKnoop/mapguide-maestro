﻿#region Disclaimer / License

// Copyright (C) 2013, Jackie Ng
// https://github.com/jumpinjackie/mapguide-maestro
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

#endregion Disclaimer / License

using OSGeo.MapGuide.MaestroAPI;
using OSGeo.MapGuide.ObjectModels;
using System.Collections.Generic;

namespace Maestro.Editors.Preview
{
    /// <summary>
    /// A simple <see cref="T:Maestro.Editors.Preview.IResourcePreviewer"/> resolution container
    /// </summary>
    public static class ResourcePreviewerFactory
    {
        private static readonly Dictionary<string, IResourcePreviewer> _previewers = new Dictionary<string, IResourcePreviewer>();

        /// <summary>
        /// Registers the given <see cref="T:Maestro.Editors.Preview.IResourcePreviewer"/> for a given connection provider
        /// </summary>
        /// <param name="provider">The name of the connection provider</param>
        /// <param name="previewer">The previewer implementation</param>
        public static void RegisterPreviewer(string provider, IResourcePreviewer previewer)
        {
            _previewers[provider.ToUpper()] = previewer;
        }

        /// <summary>
        /// Gets whether a previewer has been registered for the given connection provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static bool HasPreviewer(string provider)
        {
            return _previewers.ContainsKey(provider.ToUpper());
        }

        /// <summary>
        /// Gets whether the given resource type is previewable for the given connection provider
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static bool IsPreviewable(IServerConnection conn, IResource resource)
        {
            Check.ArgumentNotNull(conn, nameof(conn));
            Check.ArgumentNotNull(resource, nameof(resource));
            var preview = GetPreviewer(conn.ProviderName);
            if (preview != null)
                return preview.IsPreviewable(resource, conn);

            return false;
        }

        /// <summary>
        /// Gets the registered previewer for the specified connection provider
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IResourcePreviewer GetPreviewer(string provider)
        {
            if (HasPreviewer(provider))
                return _previewers[provider.ToUpper()];

            return null;
        }
    }
}