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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using OSGeo.MapGuide.ObjectModels.ApplicationDefinition;
using Maestro.Editors.Generic;

namespace Maestro.Editors.Fusion.WidgetEditors
{
    public partial class MapMenuWidgetCtrl : UserControl, IWidgetEditor
    {
        public MapMenuWidgetCtrl()
        {
            InitializeComponent();
        }

        private IWidget _widget;
        private IEditorService _edsvc;

        public void Setup(IWidget widget, FlexibleLayoutEditorContext context, IEditorService edsvc)
        {
            _widget = widget;
            _edsvc = edsvc;
            baseEditor.Setup(_widget, context, edsvc);
            txtFolder.Text = _widget.GetValue("Folder");
        }

        public Control Content
        {
            get { return this; }
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            using (var picker = new ResourcePicker(_edsvc.ResourceService, ResourcePickerMode.OpenFolder))
            {
                if (picker.ShowDialog() == DialogResult.OK)
                {
                    txtFolder.Text = picker.ResourceID;
                }
            }
        }

        private void txtFolder_TextChanged(object sender, EventArgs e)
        {
            _widget.SetValue("Folder", txtFolder.Text);
        }
    }
}