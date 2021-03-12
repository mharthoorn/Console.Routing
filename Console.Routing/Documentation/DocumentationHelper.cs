using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleRouting
{
    public static class DocumentationHelper
    {
        public static string GetParamKey(Type type)
        {
            var builder = new StringBuilder();
            builder.Append(type.Namespace);
            builder.Append(".");

            var l = type.Name.IndexOf('`');
            var name = (l > 0) ? type.Name.Substring(0, l) : type.Name;
            builder.Append(name);
            
            var generics = type.GenericTypeArguments;
            if (generics.Length > 0)
            {
                builder.Append("{");
                foreach (var arg in generics)
                {
                    builder.Append(arg.FullName); // this will fail on recursive generic types
                }
                builder.Append("}");
            }
            return builder.ToString();
        }
        

        public static string GetKey(MethodInfo method)
        {
            var builder = new StringBuilder();
            builder.Append("M:");

            string typename = method.DeclaringType.FullName.Replace("+", ".");
            builder.Append(typename);

            builder.Append(".");

            
            builder.Append(method.Name);
            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                builder.Append("(");
                bool first = true;
                foreach (var param in method.GetParameters())
                {
                    if (!first) builder.Append(",");
                    var paramkey = GetParamKey(param.ParameterType);
                    first = false;
                    builder.Append(paramkey);
                }
                builder.Append(")");
            }

            return builder.ToString();
        }

        public static string ConsiseTrim(string input)
        {
            string result = Regex.Replace(input, " {2,}", " ").Replace("\n ", "\n").Trim('\n', ' ');
            //string result = input.Trim('\n');
            return result;
        }
    }
}
