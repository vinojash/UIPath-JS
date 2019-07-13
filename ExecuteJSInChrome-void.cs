using System.Activities;
using System.ComponentModel;

namespace Chrome
{
    public class ExecuteJSInChromeVoid : CodeActivity
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

        protected override void Execute(CodeActivityContext context)
        {
            ChromeWebSocket socket = new ChromeWebSocket();

            int fnId = MessageId.Get(context);
            string webSocketURL = ChromeWebSocketURL.Get(context);
            string jsFunction = JSFunction.Get(context);
            string result = socket.executeJSInChrome(webSocketURL, fnId, jsFunction, false);
        }
    }
}
