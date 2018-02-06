using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Streaming;
using Microsoft.BizTalk.XPath;
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace BizTalkComponents.Utils
{
    public static class MessageExtensions
    {
        public static Dictionary<string, string> SelectMultiple(this IBaseMessage pInMsg, params string[] xPaths)
        {
            Stream inboundStream = pInMsg.BodyPart.GetOriginalDataStream();
            var virtualStream = new VirtualStream();
            var readOnlySeekableStream = new ReadOnlySeekableStream(inboundStream, virtualStream);
            readOnlySeekableStream.Seek(0, SeekOrigin.Begin);
            var xmlTextReader = new XmlTextReader(readOnlySeekableStream);

            var xPathCollection = new XPathCollection();

            //Keep track of the mapping between XPaths and their position in the XPathCollection
            var indexToXPathMap = new Dictionary<int, string>();
            foreach (var xPath in xPaths)
            {
                int index = xPathCollection.Add(xPath);
                indexToXPathMap.Add(index, xPath);
            }

            var xPathReader = new XPathReader(xmlTextReader, xPathCollection);

            var xPathToValueMap = new Dictionary<string, string>();
            while (xPathReader.ReadUntilMatch())
            {
                string value = xPathReader.NodeType == XmlNodeType.Attribute ?
                    xPathReader.GetAttribute(xPathReader.Name) :
                    xPathReader.ReadString();

                //Which XPath triggered the match
                int index = indexToXPathMap.Keys.First(x => xPathReader.Match(x));

                //Only return the first match for each XPath
                if (!xPathToValueMap.ContainsKey(indexToXPathMap[index]))
                {
                    xPathToValueMap.Add(indexToXPathMap[index], value);
                }
            }

            readOnlySeekableStream.Seek(0, SeekOrigin.Begin);
            return xPathToValueMap;
        }

        public static string Select(this IBaseMessage pInMsg, string xPath)
        {
            return pInMsg.SelectMultiple(xPath).Values.FirstOrDefault();
        }

        public static void Mutate(this IBaseMessage pInMsg,
            Dictionary<string, Func<string, string>> xPathToMutatorMap)
        {
            var xPathCollection = new XPathCollection();
            foreach (var xPath in xPathToMutatorMap.Keys)
            {
                xPathCollection.Add(xPath);
            }

            Stream inboundStream = pInMsg.BodyPart.GetOriginalDataStream();
            var virtualStream = new VirtualStream(inboundStream);
            var valueMutator = new ValueMutator(HandleXpathFound);
            var xPathMutatorStream = new XPathMutatorStream(virtualStream, xPathCollection, valueMutator);
            pInMsg.BodyPart.Data = xPathMutatorStream;

            void HandleXpathFound(int matchIdx, XPathExpression matchExpr, string origVal, ref string finalVal) 
            { 
                var mutator = xPathToMutatorMap[matchExpr.XPath];
                finalVal = mutator(origVal);
            }
        }

        public static void Replace(this IBaseMessage pInMsg, string xPath, string replacement)
        {
            var replacementMap = new Dictionary<string, string> { [xPath] = replacement };
            pInMsg.ReplaceMultiple(replacementMap);
        }

        public static void ReplaceMultiple(this IBaseMessage pInMsg, Dictionary<string, string> replacementMap)
        {
            var dict = new Dictionary<string, Func<string, string>>();

            foreach (var item in replacementMap)
            {
                dict.Add(item.Key, x => item.Value);
            }

            pInMsg.Mutate(dict);
        }
    }
}
