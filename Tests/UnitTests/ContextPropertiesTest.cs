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
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/file-properties#ReceivedFileName", FileProperties.ReceivedFileName);
        }

        [TestMethod]
        public void ValidateSSONamesTest()
        {
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#SSOTicket", SSOTicketProperties.SSOTicket);
        }

        [TestMethod]
        public void ValidateSystemNamesTest()
        {
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#CorrelationToken", SystemProperties.CorrelationToken);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#EpmRRCorrelationToken", SystemProperties.EpmRRCorrelationToken);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#IsRequestResponse", SystemProperties.IsRequestResponse);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#MessageType", SystemProperties.MessageType);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#ReqRespTransmitPipelineID", SystemProperties.ReqRespTransmitPipelineID);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#RouteDirectToTP", SystemProperties.RouteDirectToTP);
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2003/system-properties#SchemaStrongName", SystemProperties.SchemaStrongName);
        }

        [TestMethod]
        public void ValidateWCFNamesTest()
        {
            Assert.AreEqual("http://schemas.microsoft.com/BizTalk/2006/01/Adapters/WCF-properties#OutboundHttpStatusCode", WCFProperties.OutboundHttpStatusCode);
        }
    }
}
