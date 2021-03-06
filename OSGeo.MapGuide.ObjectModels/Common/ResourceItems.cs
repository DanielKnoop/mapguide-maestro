﻿#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

using System.Collections.Generic;
using System.Xml.Serialization;
#pragma warning disable 1591, 0114, 0108, 0114, 0108
namespace OSGeo.MapGuide.ObjectModels.Common
{
    partial class ResourceList
    {
        [XmlIgnore]
        public IEnumerable<IRepositoryItem> Children
        {
            get
            {
                foreach (var obj in this.Items)
                {
                    yield return (IRepositoryItem)obj;
                }
            }
        }
    }

    partial class ResourceListResourceDocument : IRepositoryItem
    {
        [XmlIgnore]
        public string Name
        {
            get { return ResourceIdentifier.GetName(this.ResourceId); }
        }

        [XmlIgnore]
        public bool HasChildren { get { return false; } } //Documents don't have child resources

        [XmlIgnore]
        public string ResourceType { get { return ResourceIdentifier.GetResourceTypeAsString(this.ResourceId); } }

        [XmlIgnore]
        public bool IsFolder { get { return false; } }
    }

    partial class ResourceListResourceFolder : IRepositoryItem
    {
        [XmlIgnore]
        public string Name
        {
            get
            {
                if (this.ResourceId != "Library://")
                    return ResourceIdentifier.GetName(this.ResourceId);
                else
                    return "Root"; //LOCALIZE
            }
        }

        [XmlIgnore]
        public bool HasChildren { get { return int.Parse(this.NumberOfDocuments) > 0 || int.Parse(this.NumberOfFolders) > 0; } }

        [XmlIgnore]
        public string ResourceType { get { return ResourceTypes.Folder.ToString(); } }

        [XmlIgnore]
        public bool IsFolder { get { return true; } }
    }
}