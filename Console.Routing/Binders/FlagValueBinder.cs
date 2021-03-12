using System;

namespace ConsoleRouting
{
    public class FlagValueBinder : IBinder
    {
        public bool Optional => true;

        public bool Match(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Flag<>);
    
        public int TryUse(Arguments arguments, Parameter param, int index, out object value)
        {
            Type innertype = param.Type.GetGenericArguments()[0];

            if (arguments.TryGetOptionString(param, out string s))
            {
                if (innertype == typeof(string))
                {
                    value = new Flag<string>(param.Name, s);
                    return 2;
                }
                else if (innertype == typeof(int))
                {
                    if (int.TryParse(s, out int n))
                    {
                        value = new Flag<int>(param.Name, n);
                        return 2;
                    }
                }
                else if (innertype.IsEnum)
                {

                    if (StringHelpers.TryParseEnum(innertype, s, out object enumvalue))
                    {
                        var flagt = FlagActivator.CreateInstance(innertype, param.Name, enumvalue);
                        value = flagt;
                        return 2;
                    }
                    else
                    {
                        value = default;
                        return 0;
                    }
                }

            }
            
            value = FlagActivator.CreateNotSetInstance(innertype, param.Name);
            return 0;

        }
    }

}
