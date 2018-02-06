using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class MessageExtensionTests
    {
        #region SelectMultiple

        [TestMethod]
        public void SelectMultiple_ShouldReturnValues_WhenXPathsMatch()
        {
            string xPath1 = "/root/element1[1]";
            string xPath2 = "/root/element2[1]";

            string inputXml =
            @"<root>
                <element1>value1</element1>
                <element1>value2</element1>
                <element2>value3</element2>
            </root>";

            string expectedValue1 = "value1";
            string expectedValue2 = "value3";

            var msg = MessageHelper.Create(inputXml);

            Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

            Assert.AreEqual(expectedValue1, results[xPath1]);
            Assert.AreEqual(expectedValue2, results[xPath2]);
        }

        [TestMethod]
        public void SelectMultiple_ShouldReturnEmptyDictionary_WhenXPathsDoNotMatch()
        {
            string xPath1 = "/root/nomatch1[1]";
            string xPath2 = "/root/nomatch2[1]";

            string inputXml =
            @"<root>
                <element1>value1</element1>
                <element1>value2</element1>
                <element2>value3</element2>
            </root>";

            var msg = MessageHelper.Create(inputXml);

            Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void SelectMultiple_ShouldReturnValue_WhenOneXPathMatches()
        {
            string xPath1 = "/root/nomatch2[1]";
            string xPath2 = "/root/element2[1]";

            string inputXml =
            @"<root>
                <element1>value1</element1>
                <element1>value2</element1>
                <element2>value3</element2>
            </root>";

            string expectedValue = "value3";

            var msg = MessageHelper.Create(inputXml);

            Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

            Assert.AreEqual(expectedValue, results[xPath2]);
        }
        #endregion

        #region Select

        [TestMethod]
        public void Select_ShouldReturnValue_WhenXPathMatches()
        {
            string xPath = "/root/element1[1]";

            string inputXml =
            @"<root>
                <element1>value</element1>
                <element1>value</element1>
                <element2>value2</element2>
            </root>";

            string expectedValue = "value";
            var msg = MessageHelper.Create(inputXml);

            string result = msg.Select(xPath);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Select_ShouldReturnNull_WhenXPathDoesNotMatch()
        {
            string xPath = "/root/nomatch[1]";

            string inputXml =
            @"<root>
                <element>value</element>
                <element>value</element>
            </root>";

            var msg = MessageHelper.Create(inputXml);

            string result = msg.Select(xPath);

            Assert.IsNull(result);
        }
        #endregion

        #region Mutate
        [TestMethod]
        public void Mutate_ShouldIncrement_WhenXPathMatches()
        {
            string xPath = "/root/element[1]";

            string inputXml =
            @"<root>
                <element>1</element>
            </root>";

            string expectedXml =
            $@"<root>
                <element>2</element>
            </root>";

            var msg = MessageHelper.Create(inputXml);

            var dict = new Dictionary<string, Func<string, string>>();
            dict.Add(xPath, x =>
            {
                int y = int.Parse(x);
                y++;
                return y.ToString();
            });

            msg.Mutate(dict);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }
        #endregion

        #region ReplaceMultiple

        [TestMethod]
        public void ReplaceMultiple_ShouldReplaceMultiple_WhenMultipleXPathsMatch()
        {
            string xPath1 = "/root/element1[1]";
            string newValue1 = "newValue";
            string xPath2 = "/root/element2[1]";
            string newValue2 = "newValue";

            string inputXml =
            @"<root>
                <element1>value</element1>
                <element2>value</element2>
            </root>";

            string expectedXml =
            $@"<root>
                <element1>{newValue1}</element1>
                <element2>{newValue2}</element2>
            </root>";

            var msg = MessageHelper.Create(inputXml);
            var replacements = new Dictionary<string, string>
            {
                [xPath1] = newValue1,
                [xPath2] = newValue2
            };

            msg.ReplaceMultiple(replacements);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        [TestMethod]
        public void ReplaceMultiple_ShouldReplaceOne_WhenOneXPathMatches()
        {
            string xPath1 = "/root/nomatch[1]";
            string newValue1 = "newValue";
            string xPath2 = "/root/element2[1]";
            string newValue2 = "newValue";

            string inputXml =
            @"<root>
                <element1>value</element1>
                <element2>value</element2>
            </root>";

            string expectedXml =
            $@"<root>
                <element1>value</element1>
                <element2>{newValue2}</element2>
            </root>";

            var msg = MessageHelper.Create(inputXml);
            var replacements = new Dictionary<string, string>
            {
                [xPath1] = newValue1,
                [xPath2] = newValue2
            };

            msg.ReplaceMultiple(replacements);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        #endregion

        #region Replace

        [TestMethod]
        public void Replace_ShouldReplaceOne_WhenXPathMatchesOne()
        {
            string xPath = "/root/element[1]";
            string newValue = "newValue";

            string inputXml =
            @"<root>
                <element>value</element>
                <element>value</element>
            </root>";

            string expectedXml =
            $@"<root>
                <element>{newValue}</element>
                <element>value</element>
            </root>";

            var msg = MessageHelper.Create(inputXml);

            msg.Replace(xPath, newValue);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        [TestMethod]
        public void Replace_ShouldReplaceMultiple_WhenXPathMatchesMultiple()
        {
            string xPath = "/root/element";
            string newValue = "newValue";

            string inputXml =
            @"<root>
                <element>value</element>
                <element>value</element>
            </root>";

            string expectedXml =
            $@"<root>
                <element>{newValue}</element>
                <element>{newValue}</element>
            </root>";

            var msg = MessageHelper.Create(inputXml);

            msg.Replace(xPath, newValue);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        [TestMethod]
        public void Replace_ShouldNotReplace_WhenXPathDoesNotMatch()
        {
            string xPath = "/root/nomatch";
            string newValue = "newValue";

            string inputXml =
            @"<root>
                <element>value</element>
                <element>value</element>
            </root>";

            string expectedXml = inputXml;

            var msg = MessageHelper.Create(inputXml);

            msg.Replace(xPath, newValue);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        #endregion
    }
}
