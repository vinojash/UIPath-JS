using System.Activities;
using System.ComponentModel;

namespace Chrome
{
    public class DevToolsProtocolVoid : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<int> MessageId { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ChromeWebSocketURL { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Method { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Params { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            ChromeWebSocket socket = new ChromeWebSocket();

            int fnId = MessageId.Get(context);
            string webSocketURL = ChromeWebSocketURL.Get(context);
            string mwthod = Method.Get(context);
            string parameters = Params.Get(context);
            string result = socket.executeJSInChrome(webSocketURL, fnId, mwthod, parameters, false);
        }
    }
}
