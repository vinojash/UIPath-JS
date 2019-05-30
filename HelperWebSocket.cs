using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WebSocketSharp;

namespace RunJavaScript
{
    class HelperWebSocket
    {
        private int id;
        private string message = @"{""id"":<id>, ""method"": ""Runtime.evaluate"",""params"": {""expression"": ""<jsCode>""}}";
        private string webSocketURL = null;
        private WebSocket connect(string webSocketURL)
        {
            WebSocket ws = new WebSocket(webSocketURL);
            return ws;
        }

        private string receiveMessage(WebSocket ws, string messageToBeSend, string matchString)
        {
            TaskCompletionSource<string> tcs1 = new TaskCompletionSource<string>();
            Task<string> t1 = tcs1.Task;
            string temp = null;

            Task.Factory.StartNew(() =>
            {
                ws.OnMessage += (sender, e) =>
                {
                    var uploadTask = Task.Run(() => {
                        temp = e.Data;
                        if (!String.IsNullOrEmpty(matchString) && temp.Contains(matchString)) 
                        {
                            tcs1.SetResult(temp);
                            //ws.Close();
                        }
                        else
                        {
                            tcs1.SetResult(temp);
                        }
                        
                    });
                };
            });
            if (!ws.IsAlive)
            {
                ws.Connect();
            }
            ws.Send(messageToBeSend);
            return t1.Result;
        }

        private void sendMessage(WebSocket ws, string messageToBeSend)
        {
            if (!ws.IsAlive)
            {
                ws.Connect();
            }
            ws.Send(messageToBeSend);
        }

        private void closeConnection(WebSocket ws)
        {
            if (ws.IsAlive)
            {
                ws.Close();
            }
        }


        private JObject execute()
        {
            TaskCompletionSource<JObject> tcs1 = new TaskCompletionSource<JObject>();
            Task<JObject> t1 = tcs1.Task;
            JObject temp = null;
            WebSocket ws = new WebSocket(webSocketURL);

            Task.Factory.StartNew(() =>
            {
                ws.OnMessage += (sender, e) =>
                {
                    var uploadTask = Task.Run(() => {
                        temp = JObject.Parse(e.Data);
                        JToken jtoken;
                        if (temp.TryGetValue("id", out jtoken) && ((Int32)temp["id"] == id))
                        {
                            tcs1.SetResult(temp);
                            ws.Close();
                        }
                    });
                };
            });

            ws.Connect();
            ws.Send(message);
            return t1.Result;
        }

        public void setFunction(string jsCode)
        {

            this.message = this.message.Replace("<jsCode>", jsCode);
            Console.WriteLine(jsCode);
        }
        public void setId(int id)
        {
            this.message = message.Replace("<id>", id.ToString());
            this.id = id;
            Console.WriteLine(id);
        }
        public void setWebSocketURL(string webSocketURL)
        {
            this.webSocketURL = webSocketURL;
        }
        public JObject getResponseFromChrome()
        {
            return this.execute();
        }
    }
}

