using System;
using Core;
using Newtonsoft.Json;

namespace Web.Infrastructure
{
    public class LogEvents 
    {
        void log(Guid? conversationId, object message, Exception ex = null)
        {
            this.LogInfo($"${conversationId} - {AppTime.Now()} - {message.GetType().FullName} - {JsonConvert.SerializeObject(message)}");
            if (ex != null) this.LogException(ex);
        }
    }
}
