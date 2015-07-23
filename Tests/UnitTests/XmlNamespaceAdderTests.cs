using System;
using System.IO;
using System.Xml;
using BizTalkComponents.Utils.Tests.UnitTests.Constants;
using BizTalkComponents.Utils.Tests.UnitTests.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class XmlNamespaceAdderTests
    {
        [TestMethod]
        public void AddUnqualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.NoNamespaceXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, null, NamespaceFormEnum.Unqualified, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Node node is not unqualified");
                    }
                }
            }
        }

        [TestMethod]
        public void AddQualifiedNamespace()
        {
            using (var fs = new FileStream(TestFiles.NoNamespaceXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, null, NamespaceFormEnum.Qualified, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");
                    }
                }
            }
        }

        [TestMethod]
        public void AddQualifiedNamespaceToChildNode()
        {
            using (var fs = new FileStream(TestFiles.NoNamespaceXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, "Tests/Test1", NamespaceFormEnum.Qualified, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Node node is not unqualified");

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");


                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");
                    }
                }
            }
        }

        [TestMethod]
        public void AddUnqualifiedNamespaceToChildNode()
        {
            using (var fs = new FileStream(TestFiles.NoNamespaceXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, "Tests/Test1", NamespaceFormEnum.Unqualified, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Node node is not unqualified");

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");


                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Node node is not unqualified");
                    }
                }
            }
        }

        [TestMethod]
        public void AddUnqualifiedNamespaceToChildNodeWithExistingParentNamespace()
        {
            using (var fs = new FileStream(TestFiles.UnqualifiedXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, "http://test:Tests/Test1", NamespaceFormEnum.Unqualified, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.ExistingNamespace, "Child node is not qualified within {0}", Misc.ExistingNamespace);

                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");


                        r.MoveToNextElement();
                        Assert.IsTrue(r.NamespaceURI == string.Empty, "Node node is not unqualified");
                    }
                }
            }
        }

        [TestMethod]
        public void AddMultipleNamespaces()
        {
            using (var fs = new FileStream(TestFiles.NoNamespaceXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, null, NamespaceFormEnum.Unqualified, Misc.NamespaceToAdd);

                    using (var reader2 = XmlReader.Create(addNamespace))
                    {
                        var addNamespace2 = new XmlNamespaceAdder(reader2, "http://testAdd:Tests/Test1",NamespaceFormEnum.Qualified, Misc.NamespaceToAdd2);

                        using (var reader3 = XmlReader.Create(addNamespace2))
                        {
                            var addNamespace3 = new XmlNamespaceAdder(reader3, "http://testAdd:Tests/Test2", NamespaceFormEnum.Unqualified, Misc.NamespaceToAdd3);

                            using (var r = XmlReader.Create(addNamespace3))
                            {
                                r.MoveToContent();
                                Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);

                                Assert.IsTrue(r.ReadToFollowing("Test1", Misc.NamespaceToAdd2));
                                Assert.IsTrue(r.Prefix != string.Empty, "Node is not qualified with prefix");

                                Assert.IsTrue(r.ReadToFollowing("Test2", Misc.NamespaceToAdd3));
                                r.MoveToNextElement();
                                Assert.IsTrue(r.NamespaceURI == string.Empty, "Node node is not unqualified");
                            }
                        }
                    }

                   
                }
            }
        }

        [TestMethod]
        public void AddDefaultNamespace()
        {
            using (var fs = new FileStream(TestFiles.NoNamespaceXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, null, NamespaceFormEnum.Default, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        r.MoveToContent();
                        Assert.IsTrue(r.NamespaceURI == Misc.NamespaceToAdd, "Child node is not qualified within {0}", Misc.NamespaceToAdd);
                        Assert.IsTrue(reader.Prefix == string.Empty, "Node is not without with prefix");

                        r.MoveToNextElement();
                        Assert.IsTrue(reader.Prefix == string.Empty, "Node is not without with prefix");
                    }
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "Exception should be thrown as namespace exists")]
        public void ExistingNamespaceException()
        {
            using (var fs = new FileStream(TestFiles.UnqualifiedXmlFilePath, FileMode.Open))
            {
                using (var reader = XmlReader.Create(fs))
                {
                    var addNamespace = new XmlNamespaceAdder(reader, null, NamespaceFormEnum.Unqualified, Misc.NamespaceToAdd);

                    using (var r = XmlReader.Create(addNamespace))
                    {
                        while (reader.Read()) { }
                    }
                }
            }
        }
    }
}