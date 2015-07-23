using System.IO;
using System.Xml;
using BizTalkComponents.Utils.Tests.UnitTests.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class XmlNamespaceRemoverTests
    {
        [TestMethod]
        public void RemoveQualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.QualifiedXmlFilePath, FileMode.Open))
            {
                var removeNamespace = new XmlNamespaceRemover(fs);

                using (var r = XmlReader.Create(removeNamespace))
                {
                    r.MoveToContent();
                    Assert.IsTrue(r.NamespaceURI == string.Empty, "Root node namespace is not removed");

                    r.MoveToElement();
                    Assert.IsTrue(r.NamespaceURI == string.Empty, "Child node namespace is not removed");
                }
            }
        }

        [TestMethod]
        public void RemoveUnqualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.UnqualifiedXmlFilePath, FileMode.Open))
            {
                var removeNamespace = new XmlNamespaceRemover(fs);

                using (var r = XmlReader.Create(removeNamespace))
                {
                    r.MoveToContent();
                    Assert.IsTrue(r.NamespaceURI == string.Empty, "Root node namespace is not removed");

                    r.MoveToElement();
                    Assert.IsTrue(r.NamespaceURI == string.Empty, "Child node namespace is not removed");
                }
            }
        }

        [TestMethod]
        public void RemoveQualifiedDefaultNamespace()
        {
            using (var fs = new FileStream(TestFiles.QualifiedDefaultXmlFilePath, FileMode.Open))
            {
                var removeNamespace = new XmlNamespaceRemover(fs);

                using (var r = XmlReader.Create(removeNamespace))
                {
                    r.MoveToContent();
                    Assert.IsTrue(r.NamespaceURI == string.Empty, "Root node namespace is not removed");

                    r.MoveToElement();
                    Assert.IsTrue(r.NamespaceURI == string.Empty, "Child node namespace is not removed");
                }
            }
        }
    }
}