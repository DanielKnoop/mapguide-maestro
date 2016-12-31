﻿#region Disclaimer / License

// Copyright (C) 2014, Jackie Ng
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

#endregion Disclaimer / License
using NUnit.Framework;
using OSGeo.MapGuide.MaestroAPI.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace OSGeo.MapGuide.MaestroAPI.Tests
{
    [TestFixture]
    public class SchemaTests
    {
        //These tests exercise various parts of the schema part of the Maestro API

        [Test]
        public void TestFeatureSchemaRoundtrip()
        {
            FeatureSchema schema = new FeatureSchema("Default", "Default Schema");
            ClassDefinition cls = new ClassDefinition("Class1", "Test Class");

            cls.AddProperty(new DataPropertyDefinition("ID", "ID Property")
            {
                IsAutoGenerated = true,
                DataType = DataPropertyType.Int64,
                IsNullable = false,
            }, true);

            var prop = cls.FindProperty("ID") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(1, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.Int64, prop.DataType);
            Assert.IsTrue(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);

            cls.AddProperty(new DataPropertyDefinition("Name", "The name")
            {
                DataType = DataPropertyType.String,
                Length = 255
            });

            prop = cls.FindProperty("Name") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(2, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.String, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);

            cls.AddProperty(new DataPropertyDefinition("Date", "The date")
            {
                DataType = DataPropertyType.DateTime,
                IsNullable = true
            });

            prop = cls.FindProperty("Date") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.DateTime, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsTrue(prop.IsNullable);

            schema.AddClass(cls);
            Assert.AreEqual(1, schema.Classes.Count);

            XmlDocument doc = new XmlDocument();
            schema.WriteXml(doc, doc);

            string path = Path.GetTempFileName();
            doc.Save(path);

            FeatureSourceDescription fsd = new FeatureSourceDescription(Utils.OpenFile(path));
            Assert.AreEqual(1, fsd.Schemas.Length);

            schema = fsd.Schemas[0];
            Assert.NotNull(schema);

            cls = schema.GetClass("Class1");
            Assert.NotNull(cls);

            prop = cls.FindProperty("ID") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.Int64, prop.DataType);
            Assert.IsTrue(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);

            prop = cls.FindProperty("Name") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.String, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsFalse(prop.IsNullable);

            prop = cls.FindProperty("Date") as DataPropertyDefinition;

            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.NotNull(prop);
            Assert.AreEqual(DataPropertyType.DateTime, prop.DataType);
            Assert.IsFalse(prop.IsAutoGenerated);
            Assert.IsFalse(prop.IsReadOnly);
            Assert.IsTrue(prop.IsNullable);
        }

        [Test]
        public void TestCreate()
        {
            var schema = new FeatureSchema("Default", "Default Schema");
            Assert.AreEqual("Default", schema.Name);
            Assert.AreEqual("Default Schema", schema.Description);

            var cls = new ClassDefinition("Class1", "My Class");
            Assert.AreEqual("Class1", cls.Name);
            Assert.AreEqual("My Class", cls.Description);
            Assert.IsTrue(string.IsNullOrEmpty(cls.DefaultGeometryPropertyName));
            Assert.AreEqual(0, cls.Properties.Count);
            Assert.AreEqual(0, cls.IdentityProperties.Count);

            var prop = new DataPropertyDefinition("ID", "identity");
            Assert.AreEqual("ID", prop.Name);
            Assert.AreEqual("identity", prop.Description);
            Assert.AreEqual(false, prop.IsAutoGenerated);
            Assert.AreEqual(false, prop.IsReadOnly);
            Assert.IsTrue(string.IsNullOrEmpty(prop.DefaultValue));

            prop.DataType = DataPropertyType.Int32;
            Assert.AreEqual(DataPropertyType.Int32, prop.DataType);

            prop.IsAutoGenerated = true;
            Assert.IsTrue(prop.IsAutoGenerated);

            prop.IsReadOnly = true;
            Assert.IsTrue(prop.IsReadOnly);

            cls.AddProperty(prop, true);
            Assert.AreEqual(1, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(cls, prop.Parent);
            Assert.NotNull(cls.FindProperty(prop.Name));

            cls.RemoveProperty(prop);
            Assert.AreEqual(0, cls.Properties.Count);
            Assert.AreEqual(0, cls.Properties.Count);
            Assert.IsNull(prop.Parent);
            Assert.IsNull(cls.FindProperty(prop.Name));

            cls.AddProperty(prop, true);
            Assert.AreEqual(1, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.AreEqual(cls, prop.Parent);
            Assert.NotNull(cls.FindProperty(prop.Name));

            cls.AddProperty(new DataPropertyDefinition("Name", "")
            {
                DataType = DataPropertyType.String,
                Length = 255,
                IsNullable = true
            });

            Assert.AreEqual(2, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);

            cls.AddProperty(new GeometricPropertyDefinition("Geom", "")
            {
                HasMeasure = false,
                HasElevation = false,
                GeometricTypes = FeatureGeometricType.All,
                SpecificGeometryTypes = (SpecificGeometryType[])Enum.GetValues(typeof(SpecificGeometryType))
            });
            Assert.AreEqual(3, cls.Properties.Count);
            Assert.AreEqual(1, cls.IdentityProperties.Count);
            Assert.IsTrue(string.IsNullOrEmpty(cls.DefaultGeometryPropertyName));

            var geom = cls.FindProperty("Geom");
            Assert.NotNull(geom);

            cls.DefaultGeometryPropertyName = geom.Name;
            Assert.False(String.IsNullOrEmpty(cls.DefaultGeometryPropertyName));

            schema.AddClass(cls);
            Assert.AreEqual(schema, cls.Parent);
        }

        [Test]
        public void TestClassNameEncoding()
        {
            // Round-trip various invalid XML names. Copied from FDO test suite
            string name1 = "Abc def";
            string name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "Abc-x20-def");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = " Abc#defg$$";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x20-Abc-x23-defg-x24--x24-");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = " Abc#defg hij";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x20-Abc-x23-defg-x20-hij");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "--abc-def---ghi--";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x2d--abc-def---ghi--");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "--abc-x20-def-x23--x24-ghi--";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x2d--abc-x2d-x20-def-x2d-x23--x2d-x24-ghi--");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "-xab";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x2d-xab");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "&Entity";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x26-Entity");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "11ab";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x31-1ab");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "2_Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x32-_Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "2%Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x32--x25-Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "2-Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x32--Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "2-x2f-Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x32--x2d-x2f-Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "_x2d-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x00-_x2d-");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "-x3d-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x2d-x3d-");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "_x2d-_x3f-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x00-_x2d-_x3f-");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "__x2d-_x3f-";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "__x2d-_x3f-");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "_Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "_5Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_5Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "_-5Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_-5Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "-_x2f-Class";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "_x2d-_x2f-Class");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            name1 = "Foo/Bar - snafu";
            name2 = Utility.EncodeFDOName(name1);
            Assert.AreEqual(name2, "Foo-x2f-Bar-x20---x20-snafu");
            Assert.AreEqual(name1, Utility.DecodeFDOName(name2));

            // Backward compatibility check. Make sure old-style 1st character encodings get decoded.
            name2 = "-x40-A";
            Assert.AreEqual(Utility.DecodeFDOName(name2), "@A");
        }
    }
}
