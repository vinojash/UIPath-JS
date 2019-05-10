using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;

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
        public OutArgument<string> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Helper runJS = new Helper();

            var data = Data.Get(context);
            var jsFunction = JSFunction.Get(context);

            var result = runJS.executeJSFunction(data, jsFunction);
            Result.Set(context, result);
        }
    }
}
