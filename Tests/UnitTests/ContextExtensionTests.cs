using System;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class ContextExtensionTests
    {
        private IBaseMessage _testMessage;

        [TestInitialize]
        public void InitializeTest()
        {
            _testMessage = MessageHelper.Create("<test></test>");
            _testMessage.Context.Promote(new ContextProperty("http://testuri.org#SourceProperty"), "Value");
        }

        #region TryRead
        [TestMethod]
        public void TryReadValidTest()
        {
            object val;

            Assert.IsTrue(_testMessage.Context.TryRead(new ContextProperty("http://testuri.org#SourceProperty"), out val));
            Assert.AreEqual("Value", val.ToString());
        }

        [TestMethod]
        public void TryReadInValidTest()
        {
            object val;

            Assert.IsFalse(_testMessage.Context.TryRead(new ContextProperty("http://testuri.org#NonExisting"), out val));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryReadInValidArgumentTest()
        {
            object val;

            _testMessage.Context.TryRead(null, out val);
        }

        [TestMethod]
        public void TryReadStringValidTest()
        {
            string val;

            Assert.IsTrue(_testMessage.Context.TryRead(new ContextProperty("http://testuri.org#SourceProperty"), out val));
            Assert.AreEqual("Value", val);
        }

        [TestMethod]
        public void TryReadStringInValidTest()
        {
            string val;

            Assert.IsFalse(_testMessage.Context.TryRead(new ContextProperty("http://testuri.org#NonExisting"), out val));
        }

        #endregion

        #region Promote
        [TestMethod]
        public void PromoteValidTest()
        {
            _testMessage.Context.Promote(new ContextProperty("http://testuri.org#TestProperty"), "Value");

            Assert.IsTrue(_testMessage.Context.IsPromoted(new ContextProperty("http://testuri.org#TestProperty")));
            Assert.AreEqual("Value", _testMessage.Context.Read(new ContextProperty("http://testuri.org#TestProperty")));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PromoteInvalidArgumentTest()
        {
            _testMessage.Context.Promote(null, "Value");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PromoteInvalidArgumentValueTest()
        {
            _testMessage.Context.Promote(new ContextProperty("http://testuri.org#TestProperty"), null);
        }
        #endregion

        #region Write
        [TestMethod]
        public void WriteValidTest()
        {
            _testMessage.Context.Write(new ContextProperty("http://testuri.org#TestProperty"), "Value");

            Assert.IsFalse(_testMessage.Context.IsPromoted(new ContextProperty("http://testuri.org#TestProperty")));
            Assert.AreEqual("Value", _testMessage.Context.Read(new ContextProperty("http://testuri.org#TestProperty")));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteInvalidArgumentTest()
        {
            _testMessage.Context.Write(null, "Value");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WriteInvalidArgumentValueTest()
        {
            _testMessage.Context.Write(new ContextProperty("http://testuri.org#TestProperty"), null);
        }

        #endregion

        #region Copy
        [TestMethod]
        public void CopyValidTest()
        {
            _testMessage.Context.Copy(new ContextProperty("http://testuri.org#SourceProperty"), new ContextProperty("http://testuri.org#DestinationProperty"));
            object val;

            Assert.IsTrue(_testMessage.Context.TryRead(new ContextProperty("http://testuri.org#DestinationProperty"), out val));
            Assert.AreEqual("Value", val);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CopyNonExistingTest()
        {
            _testMessage.Context.Copy(new ContextProperty("http://testuri.org#NonExisting"), new ContextProperty("http://testuri.org#DestinationProperty"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyNullSourcePropertyTest()
        {
            _testMessage.Context.Copy(null, new ContextProperty("http://testuri.org#DestinationProperty"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CopyNullDestinationPropertyTest()
        {
            _testMessage.Context.Copy(new ContextProperty("http://testuri.org#NonExisting"), null);
        }
        #endregion

        #region IsPromoted
        [TestMethod]
        public void IsPromotedTrueValidTest()
        {
            Assert.IsTrue(_testMessage.Context.IsPromoted(new ContextProperty("http://testuri.org#SourceProperty")));
        }

        [TestMethod]
        public void IsPromotedFalseValidTest()
        {
            Assert.IsFalse(_testMessage.Context.IsPromoted(new ContextProperty("http://testuri.org#OtherProperty")));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsPromotedInvalidArgumentTest()
        {
            _testMessage.Context.IsPromoted(null);
        }
        #endregion

        #region Read
        [TestMethod]
        public void ReadExistingValidTest()
        {
            object val  = _testMessage.Context.Read(new ContextProperty("http://testuri.org#SourceProperty"));
            
            Assert.AreEqual("Value", val);
        }

        [TestMethod]
        public void ReadNonExistingValidTest()
        {
            object val = _testMessage.Context.Read(new ContextProperty("http://testuri.org#NonExisting"));

            Assert.IsNull(val);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadInvalidArgumentTest()
        {
            _testMessage.Context.Read(null);
        }
        
        #endregion
    }
}
