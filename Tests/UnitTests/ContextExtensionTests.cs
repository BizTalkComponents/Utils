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
    }
}
