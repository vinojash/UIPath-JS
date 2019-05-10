using System;
using System.Threading.Tasks;
using EdgeJs;

namespace RunJavaScript
{
    class Helper
    {
        private string data, jsFunction;

        private string buildFunction()
        {
            return @"return function(data, callback){" + jsFunction + "callback(null, data);}";
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

        public string executeJSFunction(string data, string jsFunction)
        {
            this.setData(data);
            this.setJSFunction(jsFunction);
            return execute();
        }
    }
}
