using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using BizTalkComponents.Utils.Tests.UnitTests.Constants;
using BizTalkComponents.Utils.Tests.UnitTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class XmlNamespaceModifierTests
    {
        [TestMethod]
        public void ModifyQualifiedDefaultNamespace()
        {
            using (var fs = new FileStream(TestFiles.QualifiedDefaultXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var modifyNamespace = new XmlNamespaceModifier(reader, Misc.ModifiedNamespace, string.Empty, Misc.ExistingNamespace);

                    using (var r = XmlReader.Create(modifyNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.ModifiedNamespace, "Root element is not qualified within {0}", Misc.ModifiedNamespace);

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.ModifiedNamespace, "Child element is not qualified within {0}", Misc.ModifiedNamespace);
                    }
                }
            }
        }

        [TestMethod]
        public void ModifyDefaultUnqualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.UnqualifiedDefaultXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var modifyNamespace = new XmlNamespaceModifier(reader, Misc.ModifiedNamespace, string.Empty, Misc.ExistingNamespace);

                    using (var r = XmlReader.Create(modifyNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.ModifiedNamespace, "Root element is not qualified within {0}", Misc.ModifiedNamespace);

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Child element is not unqaulified");
                    }
                }
            }
        }

        [TestMethod]
        public void ModifyQualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.QualifiedXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var modifyNamespace = new XmlNamespaceModifier(reader, Misc.ModifiedNamespace, string.Empty, Misc.ExistingNamespace);

                    using (var r = XmlReader.Create(modifyNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.ModifiedNamespace, "Root element is not qualified within {0}", Misc.ModifiedNamespace);

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.ModifiedNamespace, "Child element is not qualified within {0}", Misc.ModifiedNamespace);
                    }
                }
            }
        }

        [TestMethod]
        public void ModifyUnqualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.UnqualifiedXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var modifyNamespace = new XmlNamespaceModifier(reader, Misc.ModifiedNamespace, string.Empty, Misc.ExistingNamespace);

                    using (var r = XmlReader.Create(modifyNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.ModifiedNamespace, "Root element is not qualified within {0}", Misc.ModifiedNamespace);

                        r.MoveToNextElement();

                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Child element should is not unqualified");
                    }
                }
            }
        }

        //Excluded for now. Not sure if differences in line breaks etc are relevant.

        //[TestMethod]
        //public void SkipNonMatchingNamespace()
        //{
        //    var s = new MemoryStream();
        //    using (var fs = new FileStream(TestFiles.UnqualifiedXmlFilePath, FileMode.Open, FileAccess.Read))
        //    {
        //        fs.CopyTo(s);
        //    }

        //    s.Seek(0, SeekOrigin.Begin);
        //    s.Position = 0;

        //    using (var reader = XmlReader.Create(s))
        //    {
        //        var modifyNamespace = new XmlNamespaceModifier(reader, Misc.ModifiedNamespace, string.Empty, "NoMatch");
                
        //        using (var file = File.Open(TestFiles.UnqualifiedXmlFilePath, FileMode.Open, FileAccess.Read))
        //        {
        //            var xml1 = Encoding.UTF8.GetString(file.ToByteArray());
        //            var xml2 = Encoding.UTF8.GetString(modifyNamespace.ToByteArray());
        //            var result = string.Compare(xml1, xml2);
        //            Assert.IsTrue(file.ToByteArray().SequenceEqual(modifyNamespace.ToByteArray()), "Non matching file is changed");
        //        }
        //    }
        //}


        [TestMethod]
        public void HandleMissingBom()
        {
            using (var fs = new FileStream(TestFiles.MissingBomXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var modifyNamespace = new XmlNamespaceModifier(reader, new UTF8Encoding(false), Misc.ModifiedNamespace, string.Empty, Misc.ExistingNamespace);

                    Assert.IsFalse(HasUtf8Bom(modifyNamespace), "BOM has been added to file");
                }
            }
        }

        private static bool HasUtf8Bom(Stream stream)
        {
            var buffer = new byte[3];
            stream.Read(buffer, 0, 3);

            return Encoding.UTF8.GetPreamble().SequenceEqual(buffer);
        }

    }
}