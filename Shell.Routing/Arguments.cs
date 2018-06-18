using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing 
{
    
    public class Arguments 
    {
        List<string> args;

        public Arguments(string[] parameters, params string[] defaults)
        {
            this.args = (parameters.Length > 0) ? parameters.ToList(): defaults.ToList();

        }
        
        public IEnumerable<string> GetAll() => args;

        public int Count => args.Count;

        public OptionValue GetOptionValue(string name)
        {
            if (TryGetOptionValue(name, out var option))
            {
                return option;
            }
            else
            {
                return OptionValue.Unset;
            }
        }

        public bool TryGetOptionValue(string name, out OptionValue option)
        {
            // format: -option (space) value
            var i = args.IndexOf("-" + name);
            if (i >= 0)
            {
                if (i + 1 <= args.Count)
                {
                    option = new OptionValue(args[i+1], 2);
                    return true;
                }
                else
                {
                    option = new OptionValue(null, 1, provided: false);
                    return false;
                }
            }
            else // format: -option:value
            {
                var parameter = "-" + name + ":";
                foreach (var arg in args)
                {
                    if (arg.StartsWith(parameter, StringComparison.InvariantCultureIgnoreCase))
                    {
                        var l = parameter.Length;
                        string value = arg.Substring(l);
                        option = new OptionValue(value, 1);
                        return true;
                    }
                }
            }

            option = OptionValue.Unset;
            return false;
                
        }

        //public OptionValue TakeOptionValue(string option)
        //{
        //    int idx = args.IndexOf("-" + option);
        //    if (idx >= 0)
        //    {
        //        args.RemoveAt(idx);
        //        if (idx <= Count - 1)
        //        {
        //            var value = args[idx];
        //            args.RemoveAt(idx);

        //            return new OptionValue(value);
        //        }
        //    }
        //    return OptionValue.Unset;
        //}

        public IEnumerable<string> GetAssignments()
        {
            foreach(var s in args)
            {
                if (s.Contains('=')) yield return s;
            }
        }

        public bool HasOption(string option)
        {
            int idx = args.IndexOf("-" + option);
            return (idx >= 0);
        }

        public bool TakeOption(string option)
        {
            int idx = args.IndexOf("-" + option);
            if (idx > 0) args.RemoveAt(idx);
            return (idx >= 0);
        }

        public static bool IsOption(string value)
        {
            return (value.StartsWith("-") || value.StartsWith("/"));
        }

        public static bool IsOption(string value, out string option)
        {
            var isoption = IsOption(value);
            option = isoption ? value.Substring(1) : value;
            return isoption;
        }

        public static bool IsValue(string value)
        {
            return !IsOption(value);
        }

        public string GetValue(int index)
        {

            if (index <= Count - 1)
            {
                string s = args[index];
                if (IsOption(s)) return null;
                return s;
            }
            else
            {
                return null;
            }
        }

        public bool TryGetValue(int index, out string value)
        {
            value = GetValue(index);
            return (!string.IsNullOrEmpty(value));
        }

        public bool TryGetHead(out string head)
        {
            head = args.FirstOrDefault();
            return !string.IsNullOrEmpty(head);
        }

        public bool TryTakeHead(out string head)
        {
            head = TakeHead();
            return (!string.IsNullOrEmpty(head));
        }

        public void RemoveHead()
        {
            args.RemoveAt(0);
        }

        public bool TryTakeHeadValue(out string value)
        {
            bool success = TryGetValue(0, out value);
            if (success) RemoveHead();
            return success;

        }

        public string TakeHead()
        {
            if (args.Any())
            {
                string value = args.First();
                RemoveHead();
                return value;
            }
            return null;
        }

        //public string GetLowercaseValue(int index)
        //{
        //    return GetValue(index)?.ToLower();
        //}

        public string GetOptionValue(string option, string fallback)
        {
            return GetOptionValue(option) ?? fallback;
        }

        public override string ToString()
        {
            return string.Join(" ", args);
        }
    }
}