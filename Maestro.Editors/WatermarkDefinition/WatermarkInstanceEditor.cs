﻿#region Disclaimer / License
// Copyright (C) 2011, Jackie Ng
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.WatermarkDefinition;
using Maestro.Editors.Generic;
using OSGeo.MapGuide.MaestroAPI.Services;
using OSGeo.MapGuide.MaestroAPI;
using System.Diagnostics;

namespace Maestro.Editors.WatermarkDefinition
{
    internal partial class WatermarkInstanceEditor : UserControl
    {
        private IWatermark _watermark;
        private IResourceService _resSvc;

        private IWatermarkAppearance _ovAppearance;
        private ITilePosition _ovTilePosition;
        private IXYPosition _ovXyPosition;

        private bool _init = false;

        public WatermarkInstanceEditor(IResourceService resSvc, IWatermark watermark)
        {
            InitializeComponent();
            _watermark = watermark;
            _resSvc = resSvc;

            try
            {
                _init = true;

                txtResourceId.Text = _watermark.ResourceId;
                txtName.Text = _watermark.Name;

                _ovAppearance = _watermark.AppearanceOverride;

                chkOverrideAppearance.Checked = (_ovAppearance != null);
                chkOverridePosition.Checked = (_watermark.PositionOverride != null);

                if (_ovAppearance == null)
                    _ovAppearance = _watermark.CreateDefaultAppearance();

                //Init appearance
                numOvRotation.Value = Convert.ToDecimal(_ovAppearance.Rotation);
                numOvTransparency.Value = Convert.ToDecimal(_ovAppearance.Transparency);

                if (_watermark.PositionOverride == null)
                {
                    _ovTilePosition = _watermark.CreateDefaultTilePosition();
                    _ovXyPosition = _watermark.CreateDefaultXYPosition();
                }
                else
                {
                    if (_watermark.PositionOverride.Type == PositionType.Tile)
                    {
                        _ovXyPosition = _watermark.CreateDefaultXYPosition();
                        _ovTilePosition = (ITilePosition)_watermark.PositionOverride;

                        rdOvTilePos.Checked = true;
                    }
                    else
                    {
                        _ovXyPosition = (IXYPosition)_watermark.PositionOverride;
                        _ovTilePosition = _watermark.CreateDefaultTilePosition();

                        rdOvPosXY.Checked = true;
                    }
                }

                Debug.Assert(_ovTilePosition != null);
                Debug.Assert(_ovXyPosition != null);
            }
            finally
            {
                _init = false;
            }
        }
        
        private void TilePos_CheckedChanged(object sender, EventArgs e)
        {
            ovPosPanel.Controls.Clear();
            Control c = null;
            if (rdOvPosXY.Checked) 
            {
                c = new XYPositionEditor(_ovXyPosition);
            }
            else if (rdOvTilePos.Checked)
            {
                c = new TilePositionEditor(_ovTilePosition);
            }

            if (c != null)
            {
                c.Dock = DockStyle.Fill;
                ovPosPanel.Controls.Add(c);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_resSvc, ResourceTypes.WatermarkDefinition, ResourcePickerMode.OpenResource))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtResourceId.Text = picker.ResourceID;
                }
            }
        }

        private void txtResourceId_TextChanged(object sender, EventArgs e)
        {
            _watermark.ResourceId = txtResourceId.Text;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            _watermark.Name = txtName.Text;
        }

        private void cmbUsage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _watermark.Usage = (UsageType)cmbUsage.SelectedItem;
        }

        private void chkOverrideAppearance_CheckedChanged(object sender, EventArgs e)
        {
            numOvTransparency.Enabled = numOvRotation.Enabled = chkOverrideAppearance.Checked;

            if (chkOverrideAppearance.Checked)
            {
                if (rdOvPosXY.Checked)
                    _watermark.PositionOverride = _ovXyPosition;
                else if (rdOvTilePos.Checked)
                    _watermark.PositionOverride = _ovTilePosition;
            }
            else
            {
                _watermark.AppearanceOverride = null;
            }
        }

        private void chkOverridePosition_CheckedChanged(object sender, EventArgs e)
        {
            rdOvPosXY.Enabled = rdOvTilePos.Enabled = ovPosPanel.Enabled = chkOverridePosition.Checked;
        }

        private void numOvTransparency_ValueChanged(object sender, EventArgs e)
        {
            _ovAppearance.Transparency = Convert.ToDouble(numOvTransparency.Value);
        }

        private void numOvRotation_ValueChanged(object sender, EventArgs e)
        {
            _ovAppearance.Rotation = Convert.ToDouble(numOvRotation.Value);
        }
    }
}