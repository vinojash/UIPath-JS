using System.Activities;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace RunJavaScript
{
    public class execuiteJavaScript : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Data { get; set; }

        [Category("Input")]
        public InArgument<string> JSFunction { get; set; }

        [Category("Output")]
        public OutArgument<JObject> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Helper runJS = new Helper();

            var data = Data.Get(context);
            var jsFunction = JSFunction.Get(context);

            JObject result = runJS.executeJSFunction(data, jsFunction);
            Result.Set(context, result);
        }
    }
}
