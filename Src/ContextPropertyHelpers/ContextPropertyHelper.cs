using System;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkComponents.Utils.ContextPropertyHelpers
{
    public class ContextPropertyHelper
    {
        public void CopyContextProperty(IBaseMessage msg, ContextProperty source, ContextProperty destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            var sourceValue = msg.Context.Read(source.PropertyName, source.PropertyNamespace) as string;

            if (sourceValue == null)
            {
                throw new InvalidOperationException("Could not find the specified source property in BizTalk context.");
            }
            
            msg.Context.Promote(destination.PropertyName, destination.PropertyNamespace,sourceValue);
        }
    }
}
