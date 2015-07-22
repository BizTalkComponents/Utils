using System.IO;
using System.Text;
using System.Xml;
using Microsoft.BizTalk.Streaming;

namespace BizTalkComponents.Utils
{
    public class XmlNamespaceRemover : XmlTranslatorStream
    {
        public XmlNamespaceRemover(XmlReader reader) : base(reader)
        {

        }

        public XmlNamespaceRemover(XmlReader reader, Encoding encoding) : base(reader, encoding)
        {
        }

        public XmlNamespaceRemover(XmlReader reader, Encoding encoding, MemoryStream outputStream) : base(reader, encoding, outputStream)
        {
        }

        protected override void TranslateStartElement(string prefix, string localName, string nsURI)
        {
            base.TranslateStartElement(null, localName, null);
        }

        protected override void TranslateAttributeValue(string prefix, string localName, string nsURI, string val)
        {
            if (localName == "xmlns")
            {
                base.TranslateAttributeValue(prefix, localName, nsURI, null);
            }
            else
            {
                base.TranslateAttributeValue(prefix, localName, nsURI, val);    
            }
        }
    }
}