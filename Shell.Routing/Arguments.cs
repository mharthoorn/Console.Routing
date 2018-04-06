﻿using System.Collections.Generic;
using System.Linq;

namespace Harthoorn.Shell.Routing 
{
    
    public class Arguments 
    {
        List<string> args;

        public Arguments(string[] parameters)
        {
            this.args = parameters.ToList();
        }
        
        public IEnumerable<string> GetAll() => args;

        public int Count => args.Count;

        public OptionValue GetOptionValue(string option)
        {
            int idx = args.IndexOf("-" + option);
            if (idx >= 0)
            {
                idx++;
                if (idx <= Count - 1) return new OptionValue(args[idx]);
            }
            return OptionValue.Unset;
        }

        public bool TryGetOptionValue(string name, out OptionValue option)
        {
            var i = args.IndexOf("-" + name);
            if (i >= 0)
            {
                if (i + 1 <= args.Count)
                {
                    option = new OptionValue(args[i+1]);
                    return true;
                }
                else
                {
                    option = new OptionValue(null, provided: false);
                    return false;
                }
            }
            else
            {
                option = OptionValue.Unset;
                return false;
            }
                
        }

        public OptionValue TakeOptionValue(string option)
        {
            int idx = args.IndexOf("-" + option);
            if (idx >= 0)
            {
                args.RemoveAt(idx);
                if (idx <= Count - 1)
                {
                    var value = args[idx];
                    args.RemoveAt(idx);

                    return new OptionValue(value);
                }
            }
            return OptionValue.Unset;
        }

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