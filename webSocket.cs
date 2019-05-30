using System.Activities;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace RunJavaScript
{
    public class ChromeWebSocketExecuteJS : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> MessageId { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ChromeWebSocketURL { get; set; }

        [Category("Input")]
        public InArgument<string> JSFunction { get; set; }

        [Category("Output")]
        public OutArgument<JObject> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ChromeWebSocket socket = new ChromeWebSocket();
            socket.setId(MessageId.Get(context));
            socket.setWebSocketURL(ChromeWebSocketURL.Get(context));
            socket.setFunction(JSFunction.Get(context));
            JObject jobject = socket.getResponseFromChrome();
            Result.Set(context, jobject);
        }
    }
}
