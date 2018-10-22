using System.Collections.Generic;
using System.Linq;

namespace Shell.Routing
{
    public class ArgResult<T> where T: IArgument
    {
        public T Arg;
        public bool Success;
        public IList<T> Matches;
        public int Count => Matches?.Count ?? 0;
        public string Message;

        public static implicit operator ArgResult<T> (T arg)
        {
            return new ArgResult<T> { Arg = arg, Success = true };
        }

        public static implicit operator T (ArgResult<T> result)
        {
            return result.Arg;
        }

        public static ArgResult<T> Fail(IList<T> matches)
        {
            return new ArgResult<T>
            {
                Success = false,
                Arg = default,
                Matches = matches
            };
        }

        public static ArgResult<T> Fail(string message)
        {
            return new ArgResult<T>
            {
                Success = false,
                Arg = default,
                Message = message
            };
        }
    }


}