using System;
using Microsoft.BizTalk.Message.Interop;

namespace BizTalkComponents.Utils.ContextPropertyHelpers
{
    public static class ContextExtensions
    {
        public static bool TryRead(this IBaseMessageContext ctx ,ContextProperty property, out string val)
        {
            return ((val = ctx.Read(property.PropertyName, property.PropertyNamespace) as string) != null);
        }

        public static void Promote(this IBaseMessageContext ctx, ContextProperty property, string val)
        {
            ctx.Promote(property.PropertyName,property.PropertyNamespace,val);
        }

        public static void Write(this IBaseMessageContext ctx, ContextProperty property, string val)
        {
            ctx.Write(property.PropertyName, property.PropertyNamespace,val);
        }

        public static void Copy(this IBaseMessageContext ctx, ContextProperty source, ContextProperty destination)
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

            if (ctx.TryRead(source, out sourceValue))
            {
                throw new InvalidOperationException("Could not find the specified source property in BizTalk context.");
            }

            ctx.Promote(destination, sourceValue);
        }
    }
}
