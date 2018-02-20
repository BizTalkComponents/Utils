using System;
using System.Collections.Generic;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;

namespace BizTalkComponents.Utils.Tests.UnitTests
{
    [TestClass]
    public class MessageExtensionTests
    {
        IBaseMessage msg;
        SendPipelineWrapper pipeline;
        string inputXml;

        [TestInitialize]
        public void Initialize()
        {
            inputXml =
            @"<root>
                <element1>value1</element1>
                <element2>value2</element2>
                <element3>value3</element3>
            </root>";

            msg = MessageHelper.CreateFromString(inputXml);
            pipeline = PipelineFactory.CreateEmptySendPipeline();
        }

        #region SelectMultiple

        [TestMethod]
        public void SelectMultiple_ShouldReturnValues_WhenXPathsMatch()
        {
            string xPath1 = "/root/element1[1]";
            string xPath2 = "/root/element3[1]";
            string expectedValue1 = "value1";
            string expectedValue2 = "value3";

            Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

            Assert.AreEqual(expectedValue1, results[xPath1]);
            Assert.AreEqual(expectedValue2, results[xPath2]);
        }

        [TestMethod]
        public void SelectMultiple_ShouldReturnEmptyDictionary_WhenXPathsDoNotMatch()
        {
            string xPath1 = "/root/nomatch1[1]";
            string xPath2 = "/root/nomatch2[1]";

            Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void SelectMultiple_ShouldReturnValue_WhenOneXPathMatches()
        {
            string xPath1 = "/root/nomatch2[1]";
            string xPath2 = "/root/element3[1]";
            string expectedValue = "value3";

            Dictionary<string, string> results = msg.SelectMultiple(xPath1, xPath2);

            Assert.AreEqual(expectedValue, results[xPath2]);
        }
        #endregion

        #region Select

        [TestMethod]
        public void Select_ShouldReturnValue_WhenXPathMatches()
        {
            string xPath = "/root/element1[1]";
            string expectedValue = "value1";

            string result = msg.Select(xPath);

            Assert.AreEqual(expectedValue, result);
        }

        [TestMethod]
        public void Select_ShouldReturnNull_WhenXPathDoesNotMatch()
        {
            string xPath = "/root/nomatch[1]";

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

            string expectedXml = inputXml
                .Replace("value1", newValue1)
                .Replace("value2", newValue2);

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

            string expectedXml = inputXml.Replace("value2", newValue1);

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
            string xPath = "/root/element1[1]";
            string newValue = "newValue";

            string expectedXml = inputXml.Replace("value1", newValue);

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
                inputXml.Replace("value", newValue);

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

            string expectedXml = inputXml;

            msg.Replace(xPath, newValue);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        #endregion

        #region FindReplace

        [TestMethod]
        public void FindReplace_ShouldReplace_WhenXPathAndFindMatches()
        {
            string xPath = "/root/element1[1]";
            string find = "value1";
            string newValue = "newValue";
            string expectedXml = inputXml.Replace("value1", newValue);

            msg.FindReplace(xPath, newValue, find);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        [TestMethod]
        public void FindReplace_ShouldNotReplace_WhenXPathButNotFindMatches()
        {
            string xPath = "/root/element1[1]";
            string find = "nomatch1";
            string newValue = "newValue";

            string expectedXml = inputXml;

            msg.FindReplace(xPath, newValue, find);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }
        #endregion

        #region FindReplaceMultiple
        [TestMethod]
        public void FindReplaceMultiple_ShouldReplaceMultiple_WhenMultipleXPathsAndFindMatch()
        {
            string xPath1 = "/root/element1[1]";
            string find1 = "value1";
            string newValue1 = "newValue";
            string xPath2 = "/root/element2[1]";
            string find2 = "value2";
            string newValue2 = "newValue";

            string expectedXml = inputXml
                 .Replace("value1", newValue1)
                 .Replace("value2", newValue2);

            var msg = MessageHelper.Create(inputXml);

            var replacements = new Dictionary<string, KeyValuePair<string, string>>
            {
                [xPath1] = new KeyValuePair<string, string>(find1, newValue1),
                [xPath2] = new KeyValuePair<string, string>(find2, newValue2),
            };

            msg.FindReplaceMultiple(replacements);

            var resultXml = MessageHelper.ReadString(msg);
            Assert.AreEqual(expectedXml, resultXml);
        }

        #endregion

    }
}
