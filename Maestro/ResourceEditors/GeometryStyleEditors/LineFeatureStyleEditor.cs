#region Disclaimer / License
// Copyright (C) 2009, Kenneth Skovhede
// http://www.hexad.dk, opensource@hexad.dk
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors
{
	/// <summary>
	/// Summary description for LineFeatureStyleEditor.
	/// </summary>
	public class LineFeatureStyleEditor : System.Windows.Forms.UserControl
	{
		private static byte[] SharedComboDataSet = null;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.GroupBox CompositeGroup;
		private System.Windows.Forms.Panel AdvancedPanel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox previewPicture;
		private System.Windows.Forms.CheckBox applyLineStyle;
		private System.Windows.Forms.CheckBox compositeLines;
		private System.Windows.Forms.ListBox lineStyles;
		private System.Windows.Forms.Panel propertyPanel;
		private System.Windows.Forms.ComboBox sizeUnitsCombo;
		private System.Windows.Forms.ComboBox sizeContextCombo;
		private ResourceEditors.GeometryStyleEditors.LineStyleEditor lineStyleEditor;
		
		private OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection m_item = null;
		private System.Windows.Forms.Panel compositePanel;
		private System.Windows.Forms.GroupBox lineGroup;
		private System.Windows.Forms.GroupBox sizeGroup;
		private System.Windows.Forms.GroupBox previewGroup;
		private System.Data.DataSet ComboBoxDataSet;
		private System.Data.DataTable SizeContextTable;
		private System.Data.DataColumn dataColumn3;
		private System.Data.DataColumn dataColumn4;
		private System.Data.DataTable UnitsTable;
		private System.Data.DataColumn dataColumn5;
		private System.Data.DataColumn dataColumn6;
		private bool isUpdating = false;
        private ToolStrip toolStrip1;
        private ToolStripButton AddStyleButton;
        private ToolStripButton RemoveStyleButton;

		public event EventHandler Changed;

		public LineFeatureStyleEditor()
		{
			if (SharedComboDataSet == null)
			{
				System.IO.Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType(), "LineStyleComboDataset.xml");
				byte[] buf = new byte[s.Length];
				if (s.Read(buf, 0, (int)s.Length) != s.Length)
					throw new Exception(OSGeo.MapGuide.Maestro.ResourceEditors.Strings.Common.AssemblyDataInternalError);
				SharedComboDataSet = buf;
			}

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			using(System.IO.MemoryStream ms = new System.IO.MemoryStream(SharedComboDataSet))
				ComboBoxDataSet.ReadXml(ms);

			lineStyleEditor.displayLine.Visible = false;
			lineStyleEditor.thicknessUpDown.ValueChanged +=new EventHandler(thicknessCombo_SelectedIndexChanged);
			lineStyleEditor.colorCombo.SelectedIndexChanged += new EventHandler(colorCombo_SelectedIndexChanged);
			lineStyleEditor.fillCombo.SelectedIndexChanged += new EventHandler(fillCombo_SelectedIndexChanged);
		}

		public void UpdateDisplay()
		{
			try
			{
				isUpdating = true;
				applyLineStyle.Checked = (m_item != null && m_item.Count != 0);

				lineStyles.Items.Clear();
				if (applyLineStyle.Checked)
					foreach(OSGeo.MapGuide.MaestroAPI.StrokeType st in m_item)
						lineStyles.Items.Add(st);

				compositeLines.Checked = lineStyles.Items.Count > 1;
				if (lineStyles.Items.Count > 0)
					lineStyles.SelectedIndex = 0;

				UpdateDisplayForSelected();

			}
			finally
			{
				isUpdating = false;
			}

		}

		private void UpdateDisplayForSelected()
		{
			bool prevUpdate = isUpdating;
			try
			{
				isUpdating = true;
				OSGeo.MapGuide.MaestroAPI.StrokeType st = this.CurrentStrokeType;
				sizeGroup.Enabled = 
				lineGroup.Enabled =
				previewGroup.Enabled =
					st != null;

                RemoveStyleButton.Enabled = st != null && m_item.Count > 1;

				if (st != null)
				{
				sizeUnitsCombo.SelectedValue = st.Unit.ToString();
					//sizeContextCombo.SelectedValue = st.??;
                    if (st.ColorAsHTML == null)
                        lineStyleEditor.colorCombo.CurrentColor = Color.White;
                    else
                        lineStyleEditor.colorCombo.CurrentColor = st.Color;

                    foreach(object i in lineStyleEditor.fillCombo.Items)
                        if (i as ImageStylePicker.NamedImage != null && (i as ImageStylePicker.NamedImage).Name == st.LineStyle)
                        {
                            lineStyleEditor.fillCombo.SelectedItem = i;
                            break;
                        }
					double o;
					if (double.TryParse(st.Thickness, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out o))
						lineStyleEditor.thicknessUpDown.Value = (decimal)o;
					else
						lineStyleEditor.thicknessUpDown.Value = 0;
				}
				previewPicture.Refresh();
			} 
			finally
			{
				isUpdating = prevUpdate;
			}

		}

		private OSGeo.MapGuide.MaestroAPI.StrokeType CurrentStrokeType
		{
			get 
			{
				if (lineStyles.Items.Count == 0)
					return null;
				else if (lineStyles.Items.Count == 1 || lineStyles.SelectedIndex <= 0)
					return (OSGeo.MapGuide.MaestroAPI.StrokeType)lineStyles.Items[0];
				else
					return (OSGeo.MapGuide.MaestroAPI.StrokeType)lineStyles.SelectedItem;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineFeatureStyleEditor));
            this.applyLineStyle = new System.Windows.Forms.CheckBox();
            this.compositeLines = new System.Windows.Forms.CheckBox();
            this.CompositeGroup = new System.Windows.Forms.GroupBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AddStyleButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveStyleButton = new System.Windows.Forms.ToolStripButton();
            this.lineStyles = new System.Windows.Forms.ListBox();
            this.AdvancedPanel = new System.Windows.Forms.Panel();
            this.compositePanel = new System.Windows.Forms.Panel();
            this.propertyPanel = new System.Windows.Forms.Panel();
            this.previewGroup = new System.Windows.Forms.GroupBox();
            this.previewPicture = new System.Windows.Forms.PictureBox();
            this.lineGroup = new System.Windows.Forms.GroupBox();
            this.lineStyleEditor = new OSGeo.MapGuide.Maestro.ResourceEditors.GeometryStyleEditors.LineStyleEditor();
            this.sizeGroup = new System.Windows.Forms.GroupBox();
            this.sizeUnitsCombo = new System.Windows.Forms.ComboBox();
            this.UnitsTable = new System.Data.DataTable();
            this.dataColumn5 = new System.Data.DataColumn();
            this.dataColumn6 = new System.Data.DataColumn();
            this.sizeContextCombo = new System.Windows.Forms.ComboBox();
            this.SizeContextTable = new System.Data.DataTable();
            this.dataColumn3 = new System.Data.DataColumn();
            this.dataColumn4 = new System.Data.DataColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ComboBoxDataSet = new System.Data.DataSet();
            this.CompositeGroup.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.AdvancedPanel.SuspendLayout();
            this.compositePanel.SuspendLayout();
            this.propertyPanel.SuspendLayout();
            this.previewGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).BeginInit();
            this.lineGroup.SuspendLayout();
            this.sizeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // applyLineStyle
            // 
            this.applyLineStyle.Checked = true;
            this.applyLineStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.applyLineStyle, "applyLineStyle");
            this.applyLineStyle.Name = "applyLineStyle";
            this.applyLineStyle.CheckedChanged += new System.EventHandler(this.applyLineStyle_CheckedChanged);
            // 
            // compositeLines
            // 
            this.compositeLines.Checked = true;
            this.compositeLines.CheckState = System.Windows.Forms.CheckState.Checked;
            resources.ApplyResources(this.compositeLines, "compositeLines");
            this.compositeLines.Name = "compositeLines";
            this.compositeLines.CheckedChanged += new System.EventHandler(this.compositeLines_CheckedChanged);
            // 
            // CompositeGroup
            // 
            this.CompositeGroup.Controls.Add(this.toolStrip1);
            this.CompositeGroup.Controls.Add(this.lineStyles);
            resources.ApplyResources(this.CompositeGroup, "CompositeGroup");
            this.CompositeGroup.Name = "CompositeGroup";
            this.CompositeGroup.TabStop = false;
            this.CompositeGroup.Enter += new System.EventHandler(this.CompositeGroup_Enter);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddStyleButton,
            this.RemoveStyleButton});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // AddStyleButton
            // 
            this.AddStyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.AddStyleButton, "AddStyleButton");
            this.AddStyleButton.Name = "AddStyleButton";
            this.AddStyleButton.Click += new System.EventHandler(this.AddStyleButton_Click);
            // 
            // RemoveStyleButton
            // 
            this.RemoveStyleButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.RemoveStyleButton, "RemoveStyleButton");
            this.RemoveStyleButton.Name = "RemoveStyleButton";
            this.RemoveStyleButton.Click += new System.EventHandler(this.RemoveStyleButton_Click);
            // 
            // lineStyles
            // 
            resources.ApplyResources(this.lineStyles, "lineStyles");
            this.lineStyles.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lineStyles.Name = "lineStyles";
            this.lineStyles.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lineStyles_DrawItem);
            this.lineStyles.SelectedIndexChanged += new System.EventHandler(this.lineStyles_SelectedIndexChanged);
            // 
            // AdvancedPanel
            // 
            this.AdvancedPanel.Controls.Add(this.compositeLines);
            this.AdvancedPanel.Controls.Add(this.applyLineStyle);
            resources.ApplyResources(this.AdvancedPanel, "AdvancedPanel");
            this.AdvancedPanel.Name = "AdvancedPanel";
            // 
            // compositePanel
            // 
            this.compositePanel.Controls.Add(this.CompositeGroup);
            resources.ApplyResources(this.compositePanel, "compositePanel");
            this.compositePanel.Name = "compositePanel";
            // 
            // propertyPanel
            // 
            this.propertyPanel.Controls.Add(this.previewGroup);
            this.propertyPanel.Controls.Add(this.lineGroup);
            this.propertyPanel.Controls.Add(this.sizeGroup);
            resources.ApplyResources(this.propertyPanel, "propertyPanel");
            this.propertyPanel.Name = "propertyPanel";
            // 
            // previewGroup
            // 
            this.previewGroup.Controls.Add(this.previewPicture);
            resources.ApplyResources(this.previewGroup, "previewGroup");
            this.previewGroup.Name = "previewGroup";
            this.previewGroup.TabStop = false;
            // 
            // previewPicture
            // 
            resources.ApplyResources(this.previewPicture, "previewPicture");
            this.previewPicture.BackColor = System.Drawing.Color.White;
            this.previewPicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewPicture.Name = "previewPicture";
            this.previewPicture.TabStop = false;
            this.previewPicture.Paint += new System.Windows.Forms.PaintEventHandler(this.previewPicture_Paint);
            // 
            // lineGroup
            // 
            this.lineGroup.Controls.Add(this.lineStyleEditor);
            resources.ApplyResources(this.lineGroup, "lineGroup");
            this.lineGroup.Name = "lineGroup";
            this.lineGroup.TabStop = false;
            // 
            // lineStyleEditor
            // 
            resources.ApplyResources(this.lineStyleEditor, "lineStyleEditor");
            this.lineStyleEditor.Name = "lineStyleEditor";
            // 
            // sizeGroup
            // 
            this.sizeGroup.Controls.Add(this.sizeUnitsCombo);
            this.sizeGroup.Controls.Add(this.sizeContextCombo);
            this.sizeGroup.Controls.Add(this.label3);
            this.sizeGroup.Controls.Add(this.label2);
            resources.ApplyResources(this.sizeGroup, "sizeGroup");
            this.sizeGroup.Name = "sizeGroup";
            this.sizeGroup.TabStop = false;
            // 
            // sizeUnitsCombo
            // 
            resources.ApplyResources(this.sizeUnitsCombo, "sizeUnitsCombo");
            this.sizeUnitsCombo.DataSource = this.UnitsTable;
            this.sizeUnitsCombo.DisplayMember = "Display";
            this.sizeUnitsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeUnitsCombo.Name = "sizeUnitsCombo";
            this.sizeUnitsCombo.ValueMember = "Value";
            this.sizeUnitsCombo.SelectedIndexChanged += new System.EventHandler(this.sizeUnitsCombo_SelectedIndexChanged);
            // 
            // UnitsTable
            // 
            this.UnitsTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn5,
            this.dataColumn6});
            this.UnitsTable.TableName = "Units";
            // 
            // dataColumn5
            // 
            this.dataColumn5.Caption = "Display";
            this.dataColumn5.ColumnName = "Display";
            // 
            // dataColumn6
            // 
            this.dataColumn6.Caption = "Value";
            this.dataColumn6.ColumnName = "Value";
            // 
            // sizeContextCombo
            // 
            resources.ApplyResources(this.sizeContextCombo, "sizeContextCombo");
            this.sizeContextCombo.DataSource = this.SizeContextTable;
            this.sizeContextCombo.DisplayMember = "Display";
            this.sizeContextCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sizeContextCombo.Name = "sizeContextCombo";
            this.sizeContextCombo.ValueMember = "Value";
            this.sizeContextCombo.SelectedIndexChanged += new System.EventHandler(this.sizeContextCombo_SelectedIndexChanged);
            // 
            // SizeContextTable
            // 
            this.SizeContextTable.Columns.AddRange(new System.Data.DataColumn[] {
            this.dataColumn3,
            this.dataColumn4});
            this.SizeContextTable.TableName = "SizeContext";
            // 
            // dataColumn3
            // 
            this.dataColumn3.Caption = "Display";
            this.dataColumn3.ColumnName = "Display";
            // 
            // dataColumn4
            // 
            this.dataColumn4.Caption = "Value";
            this.dataColumn4.ColumnName = "Value";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ComboBoxDataSet
            // 
            this.ComboBoxDataSet.DataSetName = "ComboBoxDataSet";
            this.ComboBoxDataSet.Locale = new System.Globalization.CultureInfo("da-DK");
            this.ComboBoxDataSet.Tables.AddRange(new System.Data.DataTable[] {
            this.SizeContextTable,
            this.UnitsTable});
            // 
            // LineFeatureStyleEditor
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.propertyPanel);
            this.Controls.Add(this.compositePanel);
            this.Controls.Add(this.AdvancedPanel);
            this.Name = "LineFeatureStyleEditor";
            this.CompositeGroup.ResumeLayout(false);
            this.CompositeGroup.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.AdvancedPanel.ResumeLayout(false);
            this.compositePanel.ResumeLayout(false);
            this.propertyPanel.ResumeLayout(false);
            this.previewGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.previewPicture)).EndInit();
            this.lineGroup.ResumeLayout(false);
            this.sizeGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UnitsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SizeContextTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ComboBoxDataSet)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion


		private void lineStyles_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateDisplayForSelected();
		}

		private void sizeContextCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating)
				return;
			//TODO: Where does this go?
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void sizeUnitsCombo_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (isUpdating || this.CurrentStrokeType == null)
				return;
			this.CurrentStrokeType.Unit = (OSGeo.MapGuide.MaestroAPI.LengthUnitType)Enum.Parse(typeof(OSGeo.MapGuide.MaestroAPI.LengthUnitType), (string)sizeUnitsCombo.SelectedValue);
			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		public OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection Item
		{
			get { return m_item; }
			set
			{
				m_item = value;
				UpdateDisplay();
			}
		}

		private void thicknessCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (isUpdating || this.CurrentStrokeType == null)
				return;
			this.CurrentStrokeType.Thickness = lineStyleEditor.thicknessUpDown.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);

			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void colorCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (isUpdating || this.CurrentStrokeType == null)
				return;
			this.CurrentStrokeType.Color = lineStyleEditor.colorCombo.CurrentColor;
			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void fillCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
            if (isUpdating || this.CurrentStrokeType == null)
				return;

            if (lineStyleEditor.fillCombo.SelectedItem as ImageStylePicker.NamedImage != null)
                this.CurrentStrokeType.LineStyle = (lineStyleEditor.fillCombo.SelectedItem as ImageStylePicker.NamedImage).Name;
			previewPicture.Refresh();
			lineStyles.Refresh();
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void applyLineStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			compositePanel.Enabled = 
			compositeLines.Enabled = 
			sizeGroup.Enabled = 
			lineGroup.Enabled =
			previewGroup.Enabled =
				applyLineStyle.Checked;

            if (!isUpdating)
            {
                if (!applyLineStyle.Checked)
                {
                    applyLineStyle.Tag = m_item;
                    m_item = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();
                }
                else
                {
                    m_item = applyLineStyle.Tag as OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection;

                    if (m_item == null)
                        m_item = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();

                    if (m_item.Count == 0)
                        m_item.Add(new OSGeo.MapGuide.MaestroAPI.StrokeType());

                    UpdateDisplay();
                }
            }
		}

		private void compositeLines_CheckedChanged(object sender, System.EventArgs e)
		{
			if (compositePanel.Visible && !compositeLines.Checked)
				this.AutoScrollMinSize = new Size(this.AutoScrollMinSize.Width, this.AutoScrollMinSize.Height - compositePanel.Height);
			else if (!compositePanel.Visible && compositeLines.Checked)
                this.AutoScrollMinSize = new Size(this.AutoScrollMinSize.Width, this.AutoScrollMinSize.Height + compositePanel.Height);

			compositePanel.Visible = compositeLines.Checked;

			if (isUpdating)
				return;
			if (Changed != null)
				Changed(this, new EventArgs());
		}

		private void previewPicture_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(1, 1, previewPicture.Width - 2, previewPicture.Height - 2), m_item);		
		}

		private void lineStyles_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			e.DrawBackground();
			if (e.Index >= 0 && e.Index < lineStyles.Items.Count)
			{
				OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection col = new OSGeo.MapGuide.MaestroAPI.StrokeTypeCollection();
				col.Add((OSGeo.MapGuide.MaestroAPI.StrokeType) lineStyles.Items[e.Index]);
				FeaturePreviewRender.RenderPreviewLine(e.Graphics, new Rectangle(1, 1, e.Bounds.Width - 2, e.Bounds.Height - 2), col);		
			}
			if ((e.State & DrawItemState.Focus) != 0)
				e.DrawFocusRectangle();
		}

        private void RemoveStyleButton_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < m_item.Count; i++)
                if (m_item[i] == this.CurrentStrokeType)
                {
                    m_item.RemoveAt(i);
                    UpdateDisplay();
                    break;
                }

        }

        private void AddStyleButton_Click(object sender, EventArgs e)
        {
            m_item.Add(new OSGeo.MapGuide.MaestroAPI.StrokeType());
            UpdateDisplay();
            lineStyles.SelectedIndex = lineStyles.Items.Count - 1;
        }


        internal void SetupForTheming()
        {
            lineStyleEditor.colorCombo.Enabled =
            lineStyleEditor.lblColor.Enabled =
            AdvancedPanel.Enabled =
                false;
        }

        private void CompositeGroup_Enter(object sender, EventArgs e)
        {

        }
    }
}
