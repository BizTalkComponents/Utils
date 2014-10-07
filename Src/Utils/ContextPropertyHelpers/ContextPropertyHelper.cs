using System;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkComponents.Utils.ContextPropertyHelpers
{
    public class ContextPropertyHelper
    {
        public static void CopyContextProperty(IBaseMessage msg, ContextProperty source, ContextProperty destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            string sourceValue;
            
            if (msg.Context.TryRead(source, out sourceValue))
            {
                throw new InvalidOperationException("Could not find the specified source property in BizTalk context.");
            }

            msg.Context.Promote(destination,sourceValue);
        }
    }
}
