using Microsoft.BizTalk.Message.Interop;

namespace BizTalkComponents.Utils.ContextPropertyHelpers
{
    public static class ContextExtensions
    {
        public static bool TryRead(this IBaseMessageContext ctx ,ContextProperty property, out string val)
        {
            return ((val = ctx.Read(property.PropertyName, property.PropertyNamespace) as string) == null);
        }

        public static void Promote(this IBaseMessageContext ctx, ContextProperty property, string val)
        {
            ctx.Promote(property.PropertyName,property.PropertyNamespace,val);
        }
    }
}
