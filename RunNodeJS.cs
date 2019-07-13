using System.Activities;
using System.ComponentModel;

namespace Chrome
{
    public class RunNodeJS : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> Data { get; set; }

        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> JSFunction { get; set; }

        [Category("Output")]
        public OutArgument<string> Result { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            Helper runJS = new Helper();

            var data = Data.Get(context);
            var jsFunction = JSFunction.Get(context);

            string result = runJS.executeJSFunction(data, jsFunction);
            Result.Set(context, result);
        }
    }
}
