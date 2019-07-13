using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using EdgeJs;
using WebSocket4Net;

namespace Chrome
{
    class Helper
    {
        private string data, jsFunction;

        private string buildFunction()
        {
            return jsFunction;
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

        public String executeJSFunction(string data, string jsFunction)
        {
            this.setData(data);
            this.setJSFunction(jsFunction);
            return this.execute();
        }


    }

    class ChromeWebSocket
    {

        private String message = null;
        private int id = 0;

        public String executeJSInChrome(String wsURL, int id, String jsFunction, Boolean waitForResponse)
        {
            this.setMessage(id, jsFunction);
            if (waitForResponse)
            {
                return this.sendJSToChromeAndWait(wsURL, true);
            }
            else
            {
                return this.sendJSToChrome(wsURL);
            }
        }
        public String executeJSInChrome(String wsURL, int id, String method, String parameters, Boolean waitForResponse)
        {
            this.setMessage(id, method, parameters);

            if (waitForResponse)
            {
                return this.sendJSToChromeAndWait(wsURL, false);
            }
            else
            {
                return this.sendJSToChrome(wsURL);
            }
        }


        public String sendJSToChrome(String wsURL)
        {
            WebSocket ws = new WebSocket(wsURL);

            ws.Opened += (sender, e) =>
            {
                ws.Send(message);
            };
            ws.Error += (sender, e) =>
            {
                Console.WriteLine(e);
            };

            ws.Closed += (sender, e) =>
            {
                Console.WriteLine(e);
            };
            ws.Open();

            return message;
        }


        public String sendJSToChromeAndWait(String wsURL, Boolean isJSFunction)
        {
            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            Task<string> t = tcs.Task;

            WebSocket ws = new WebSocket(wsURL);

            Task.Factory.StartNew(() =>
            {
                Task.Run(() =>
                {
                    ws.Opened += (sender, e) =>
                    {
                        ws.Send(message);
                        Console.WriteLine(message);
                    };
                    ws.Error += (sender, e) =>
                    {
                        Console.WriteLine(e);
                    };
                    ws.MessageReceived += (sender, e) =>
                    {
                        String result = parseReceivedMessage(e.Message, ws, isJSFunction);
                        if (null != result)
                        {
                            tcs.SetResult(result);
                        }

                    };
                    ws.Closed += (sender, e) =>
                    {
                        Console.WriteLine(e);
                    };
                    ws.Open();
                });

            });
            return t.Result;
        }

        private String parseReceivedMessage(String receivedMessage, WebSocket ws, Boolean isJSFunction)
        {
            Console.WriteLine(receivedMessage);
            Console.WriteLine(isJSFunction);
            String value = null;
            try
            {
                JObject inputMessage = JObject.Parse(receivedMessage);
                JToken jtoken;
                if (isJSFunction && inputMessage.TryGetValue("id", out jtoken) && ((Int32)inputMessage["id"] == id) && inputMessage.TryGetValue("result", out jtoken))
                {
                    ws.Close();
                    ws.Dispose();
                    JObject result = JObject.Parse(inputMessage.GetValue("result").ToString());
                    if (result.TryGetValue("result", out jtoken))
                    {
                        JObject finalResult = JObject.Parse(result.GetValue("result").ToString());
                        value = finalResult.GetValue("value").ToString();
                    }
                }
                else if (!isJSFunction && inputMessage.TryGetValue("id", out jtoken) && ((Int32)inputMessage["id"] == id))
                {
                    ws.Close();
                    ws.Dispose();
                    value = receivedMessage;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            finally
            {
                //as of now not required..!
            }
            return value;
        }
        private void setMessage(int id, String jsFunction)
        {
            this.setMessage(id, "Runtime.evaluate", "\"expression\": \"" + jsFunction + "\"");
        }

        private void setMessage(int id, String method, String parameters)
        {
            this.message = "{ \"id\":" + id + ",\"method\":\"" + method + "\",\"params\": { " + parameters + "} }";
            this.id = id;
        }

    }
}
