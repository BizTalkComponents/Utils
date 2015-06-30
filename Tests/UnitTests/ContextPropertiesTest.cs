using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class ContextPropertiesTest
    {
        [TestMethod]
        public void ValidateFileNamesTest()
        {
            var properties = new FileProperties();
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/file-properties#ReceivedFileName", properties.ReceivedFileName);
        }

        [TestMethod]
        public void ValidateSSONamesTest()
        {
            var properties = new SSOTicketProperties();
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#SSOTicket", properties.SSOTicket);
        }

        [TestMethod]
        public void ValidateSystemNamesTest()
        {
            var properties = new SystemProperties();
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#CorrelationToken", properties.CorrelationToken);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#EpmRRCorrelationToken", properties.EpmRRCorrelationToken);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#IsRequestResponse", properties.IsRequestResponse);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#MessageType", properties.MessageType);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#ReqRespTransmitPipelineID", properties.ReqRespTransmitPipelineID);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#RouteDirectToTP", properties.RouteDirectToTP);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#SchemaStrongName", properties.SchemaStrongName);
        }

        [TestMethod]
        public void ValidateWCFNamesTest()
        {
            var properties = new WCFProperties();
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties#OutboundHttpStatusCode", properties.OutboundHttpStatusCode);
        }
    }
}
