using System.Activities;
using System.ComponentModel;

namespace Chrome
{
    public class ExecuteJSInChrome : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> MessageId { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ChromeWebSocketURL { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> JSFunction { get; set; }

        [Category("Output")]
        public OutArgument<string> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ChromeWebSocket socket = new ChromeWebSocket();

            int fnId = MessageId.Get(context);
            string webSocketURL = ChromeWebSocketURL.Get(context);
            string jsFunction = JSFunction.Get(context);

            string result = socket.executeJSInChrome(webSocketURL, fnId, jsFunction, true);

            Result.Set(context, result);
        }
    }
}
