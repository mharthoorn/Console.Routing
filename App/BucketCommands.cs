using ConsoleRouting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTemplate
{
    /// <summary>
    /// Bucket commands
    /// </summary>
    [Module]
    public class BucketCommands
    {
        /// <summary>
        /// Curl is a fictitious command mimicking the curl cli tool.
        /// </summary>
        /// <param name="settings">These settings need not be visible</param>
        [Command]
        public void Curl(CurlSettings settings)
        {
            if (settings.CrlF) Console.WriteLine("cr/lf has been set");
        }

    }


    /// <summary>
    /// Bucket list
    /// </summary>
    [Bucket]
    public class CurlSettings
    {
        /// <summary>
        /// Sets a new line for each row.
        /// </summary>
        public Flag CrlF;

        /// <summary>
        /// Append to existing output
        /// </summary>
        public bool Append;

        /// <summary>
        /// The Url to connect to
        /// </summary>
        public Flag<string> Url;

        /// <summary>
        /// Use only ascii characters
        /// </summary>
        [Alt("use-ascii")]
        public Flag UseAscii { get; set; }

        /// <summary>
        /// Use basic authentication
        /// </summary>
        public Flag Basic { get; set; }

        /// <summary>
        /// Optionally set the remote name.
        /// </summary>
        public Flag<string> RemoteName { get; set; }
    }
}
