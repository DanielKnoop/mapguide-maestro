﻿#region Disclaimer / License

// Copyright (C) 2011, Jackie Ng
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

using OSGeo.MapGuide.MaestroAPI.Services;
using System;
using System.Windows.Forms;

namespace Maestro.Editors.FeatureSource
{
    /// <summary>
    /// A dialog that displays the spatial contexts of a feature source
    /// </summary>
    public partial class SpatialContextsDialog : Form
    {
        private SpatialContextsDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpatialContextsDialog"/> class.
        /// </summary>
        /// <param name="fsId">The fs id.</param>
        /// <param name="featSvc">The feat SVC.</param>
        public SpatialContextsDialog(string fsId, IFeatureService featSvc)
            : this()
        {
            lblFeatureSource.Text = fsId;
            grdSpatialContexts.DataSource = featSvc.GetSpatialContextInfo(fsId, false).SpatialContext;
            lblCount.Text = string.Format(Strings.SpatialContextsFound, grdSpatialContexts.Rows.Count);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}