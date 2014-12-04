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
    }
}
