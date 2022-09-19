using System.Reflection;
using Common.RequestMessages;
using Common.ResponseMessages;

namespace Common
{
    public static class InstanceCreator
    {
        private static Assembly _assembly;
        private static readonly object LockObject = new object();

        public static FinancialBaseResponse GetResponseInstance(FinancialBaseRequest requestParameter)
        {
            if (_assembly == null)
            {
                lock (LockObject)
                {
                    if (_assembly == null)
                    {
                        _assembly = Assembly.Load("Common");
                    }
                }
            }
            var responseTypeName = "";
            var fullName = requestParameter.GetType().FullName;

            if (fullName != null)
                responseTypeName = fullName.Replace("Request", "Response");

            if (!responseTypeName.EndsWith("Response"))
                responseTypeName += "Response";


            return _assembly.CreateInstance(responseTypeName) as FinancialBaseResponse;
        }
    }
}