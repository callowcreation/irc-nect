#region Author
/*
     
     Jones St. Lewis Cropper (caLLow)
     
     Another caLLowCreation
     
     Visit us on Google+ and other social media outlets @caLLowCreation
     
     Thanks for using our product.
     
     Send questions/comments/concerns/requests to 
      e-mail: caLLowCreation@gmail.com
      subject: IRCnect
     
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor.Replies
{
    /// <summary>
    /// Basic catch all filter
    /// </summary>
    public class RepliesFilter : MonitorFilter
    {

        const string REPLIES_FORMAT = @"^({0})$";
        const string REPLIED_PATTERN = @"^" + PATTERN_ALL + "$";
        /// <summary>
        /// Seperator to devide multiple entries.
        /// </summary>
        protected const string RAW_SEPARATOR = "|";

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        public RepliesFilter(string pattern = REPLIED_PATTERN)
            : base(pattern)
        { }

        /// <summary>
        /// Return raw chat message
        /// </summary>
        /// <param name="message">Raw chat response message to parse</param>
        /// <returns>Message interface</returns>
        public override MonitorArgs Parse(string message)
        {
            Match match = m_Regex.Match(message);

            var args = new RepliesArgs(message);
            if (match.Success)
            {
                args.MatchFilters(m_Filters);
                //IRCnect.Utils.Logger.L(args.GetType().Name + "<-----" + args.data);
            }

            return args;
        }

        /// <summary>
        /// Adds a list of greetings to the statement check list
        /// <para>NOTE: Greeting checks added this way use RegexOptions.IgnoreCase</para>
        /// </summary>
        /// <param name="messages">List of greetings to check for</param>
        /// <param name="callbacks">Callbacks to invoke on greetings filtered</param>
        /// <returns>RepliesFilter type to chain filter additions.</returns>
        public RepliesFilter AddRepliesFilter(IEnumerable<string> messages, params Action<MonitorArgs>[] callbacks)
        {
            AddFilter(REPLIES_FORMAT, string.Join(RAW_SEPARATOR, messages.ToArray()), RegexOptions.IgnoreCase, callbacks);
            return this;
        }

        /// <summary>
        /// Adds all default filters to the check list.
        /// </summary>
        /// <param name="callbacks">Callbacks to invoke upon filtered.</param>
        /// <returns>RepliesFilter type to chain filter additions.</returns>
        public RepliesFilter FilterAll(params Action<MonitorArgs>[] callbacks)
        {
            return AddRepliesFilter(new[] { ".*" }, callbacks);
        }
    }
}
