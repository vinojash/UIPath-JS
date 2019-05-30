using System;
using System.Threading.Tasks;
using EdgeJs;
using Newtonsoft.Json.Linq;
using WebSocketSharp;

namespace RunJavaScript
{
    class Helper
    {
        private string data, jsFunction;

        private string buildFunction()
        {
            return jsFunction ;
        }

        private string execute()
        {
            Func<object, Task<object>> myFunc = Edge.Func(buildFunction());
            return (string)myFunc(data).Result; ;
        }

        private void setData(string data)
        {
            this.data = data;
        }

        private void setJSFunction(string jsFunction)
        {
            this.jsFunction = jsFunction;
        }

        public JObject executeJSFunction(string data, string jsFunction)
        {
            this.setData(data);
            this.setJSFunction(jsFunction);
            return JObject.Parse(execute());
        }

       
    }

    class ChromeWebSocket
    {
        private int id;
        private string message = @"{""id"":<id>, ""method"": ""Runtime.evaluate"",""params"": {""expression"": ""<jsCode>""}}";

        private string webSocketURL = null;
        private JObject execute()
        {
            Console.WriteLine(message);
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
