using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class PropertyBagHelperTests
    {
        private IPropertyBag _propertyBag;
        private const bool TestBool = true;
        private DateTime TestDate = new DateTime(0001, 12, 25, 6, 0, 0);
        private const int TestInt = 42;
        private const string TestString = "SomeTestString";

        private const string TestBoolString = "TestBoolean";
        private const string TestDateString = "TestDate";
        private const string TestIntString = "TestInteger";
        private const string TestStringString = "TestString";
        private const string TestXmlDocumentString = "TestXmlDocument";

        [TestInitialize]
        public void InitializeTest()
        {
            int vtBoolean = (int)VarEnum.VT_BOOL;
            int vtDate = (int)VarEnum.VT_DATE;
            int vtInt = (int)VarEnum.VT_I4;
            int vtString = (int)VarEnum.VT_LPSTR;
            string xml = @"<Properties>" +
                "<" + TestBoolString + " vt='" + vtBoolean + "'>" + Convert.ToInt32(TestBool) + "</" + TestBoolString + ">" +
                "<" + TestDateString + " vt='" + vtDate + "'>" + TestDate.ToString("s") + "</" + TestDateString + ">" +
                "<" + TestIntString + " vt='" + vtInt + "'>" + TestInt + "</" + TestIntString + ">" +
                "<" + TestStringString + " vt='" + vtString + "'>" + TestString + "</" + TestStringString + ">" +
                "</Properties>";

            _propertyBag = new InstConfigPropertyBag(Load(xml));
        }

        [TestMethod]
        public void ToStringOrDefaultTest()
        {
            object property = PropertyBagHelper.ReadPropertyBag(_propertyBag, TestStringString);
            string value = PropertyBagHelper.ToStringOrDefault(property, "WrongValue");
            Assert.AreEqual(TestString, value);
        }

        [TestMethod]
        public void ToStringOrDefaultTestDefaultValue()
        {
            string defaultValue = "DefaultValue";
            object property = PropertyBagHelper.ReadPropertyBag(_propertyBag, "Wrong" + TestStringString);
            string value = PropertyBagHelper.ToStringOrDefault(property, defaultValue);
            Assert.AreEqual(defaultValue, value);
        }

       [TestMethod]
        public void ReadPropertyBagDateTimeWithOldValueFailTest()
        {
            DateTime oldValue = default(DateTime);
            DateTime property = PropertyBagHelper.ReadPropertyBag<DateTime>(_propertyBag, "Wrong" + TestDateString, oldValue);
            Assert.AreEqual(default(DateTime), property);
        }

        [TestMethod]
        public void ReadPropertyBagDateTimeGenericTest()
        {
            DateTime property = PropertyBagHelper.ReadPropertyBag<DateTime>(_propertyBag, TestDateString);
            Assert.AreEqual(TestDate, property);
        }

        [TestMethod]
        public void ReadPropertyBagDateTimeGenericFailTest()
        {
            DateTime property = PropertyBagHelper.ReadPropertyBag<DateTime>(_propertyBag, "Wrong" + TestDateString);
            Assert.AreEqual(default(DateTime), property);
        }

       [TestMethod]
        public void ReadPropertyBagBooleanWithOldValueFailTest()
        {
            bool oldValue = false;
            bool property = PropertyBagHelper.ReadPropertyBag<bool>(_propertyBag, "Wrong" + TestBoolString, oldValue);
            Assert.AreEqual(default(bool), property);
        }

        [TestMethod]
        public void ReadPropertyBagBooleanGenericTest()
        {
            bool property = PropertyBagHelper.ReadPropertyBag<bool>(_propertyBag, TestBoolString);
            Assert.AreEqual(TestBool, property);
        }

        [TestMethod]
        public void ReadAllPropertyBag()
        {
            var testClass = new PropertyBagTestClass();
            PropertyBagHelper.ReadAll(_propertyBag,testClass);
            Assert.AreEqual(true, testClass.TestBoolean);
            Assert.AreEqual(TestDate, testClass.TestDate);
            Assert.AreEqual(42, testClass.TestInteger);
            Assert.AreEqual("SomeTestString", testClass.TestString);
        }
        

        [TestMethod]
        public void ReadPropertyBagBooleanGenericFailTest()
        {
            bool property = PropertyBagHelper.ReadPropertyBag<bool>(_propertyBag, "Wrong" + TestBoolString);
            Assert.AreEqual(default(bool), property);
        }
        

        [TestMethod]
        public void ReadPropertyBagWithOldValueIntGenericFailTest()
        {
            int property = PropertyBagHelper.ReadPropertyBag<int>(_propertyBag, "Wrong" + TestIntString,default(int));
            Assert.AreEqual(default(int), property);
        }
        
        [TestMethod]
        public void ReadPropertyBagIntGenericTest()
        {
            int property = PropertyBagHelper.ReadPropertyBag<int>(_propertyBag, TestIntString);
            Assert.AreEqual(TestInt, property);
        }

        [TestMethod]
        public void ReadPropertyBagIntGenericFailTest()
        {
            int property = PropertyBagHelper.ReadPropertyBag<int>(_propertyBag, "Wrong" + TestIntString);
            Assert.AreEqual(default(int), property);
        }

        [TestMethod]
        public void ReadPropertyBagWrongTypeTest()
        {
            int property = PropertyBagHelper.ReadPropertyBag<int>(_propertyBag, TestBoolString);
            Assert.AreEqual(default(int), property);
        }

        [TestMethod]
        public void ReadPropertyWitOldValueBagWrongTypeTest()
        {
            int property = PropertyBagHelper.ReadPropertyBag<int>(_propertyBag, TestBoolString,3);
            Assert.AreEqual(default(int), property);
        }

        [TestMethod]
        public void WritePropertyBagComplexObjectTest()
        {
            PropertyBagHelper.WritePropertyBag(_propertyBag, TestXmlDocumentString, new XmlDocument());
            var property = PropertyBagHelper.ReadPropertyBag<XmlDocument>(_propertyBag, TestXmlDocumentString);
            Assert.IsNotNull(property);
        }

        [TestMethod]
        public void WriteAllPropertyBagTest()
        {
            var testClass = new PropertyBagTestClass
            {
                TestBoolean = true,
                TestDate = new DateTime(2000, 12, 25, 6, 0, 0),
                TestInteger = 13,
                TestString = "This is a test string"
            };
            PropertyBagHelper.WriteAll(_propertyBag, testClass);

            Assert.AreEqual(true, PropertyBagHelper.ReadPropertyBag(_propertyBag, "TestBoolean", TestBool));
            Assert.AreEqual("This is a test string", PropertyBagHelper.ReadPropertyBag(_propertyBag, "TestString", TestString));
            Assert.AreEqual(new DateTime(2000, 12, 25, 6, 0, 0), PropertyBagHelper.ReadPropertyBag(_propertyBag, "TestDate", TestDate));
            Assert.AreEqual(13, PropertyBagHelper.ReadPropertyBag(_propertyBag, "TestInteger", TestInt));
        }
        
        
        private XmlReader Load(string xml)
        {
            XmlReader reader = new XmlTextReader(new StringReader(xml));
            reader.Read();
            return reader;
        }
    }

    internal class PropertyBagTestClass
    {
        public bool TestBoolean { get; set; }
        public string TestString { get; set; }
        public DateTime TestDate { get; set; }
        public int TestInteger { get; set; }
    }
}