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

namespace IRCnect.Channel.Monitor.Replies.Inbounds
{
    /// <summary>
    /// Class providing inbounds filter functionality for messages.
    /// </summary>
    public class InboundsFilter : RepliesFilter
    {
        /// <summary>
        /// Pattern to match for incomming message.
        /// </summary>
        protected const string MESSAGE_PATTERN = @":(\w+)!.*#(\w+) :" + PATTERN_ALL;
        //
        internal const string GREETING_FORMAT = @"^((?:.*)?(?:\W|\b|\s)+({0})(?:\W|\s|\b)+(?:.*)?)$";

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        public InboundsFilter(string pattern = MESSAGE_PATTERN)
            : base(pattern)
        { }

        /// <summary>
        /// Parse a chat message and returns matched expresions
        /// </summary>
        /// <param name="message">Raw chat response message to parse</param>
        /// <returns>Matched expressions class</returns>
        public override MonitorArgs Parse(string message)
        {
            Match match = m_Regex.Match(message);

            var args = new InboundsArgs(message);
            args.messageMatch = match;
            args.data = match.Value;
            if (match.Success)
            {
                args.MatchFilters(m_Filters);
                //IRCnect.Utils.Logger.L(args.GetType().Name + "<-----" + args.channel);
            }
            return args;
        }

        /// <summary>
        /// Gets the specific type of args required for this class to store data.
        /// <para>Override to provide unique class interface args.</para>
        /// </summary>
        /// <param name="match">The match that will be preformed.</param>
        /// <returns>Args used by filters to store parsed data.</returns>
        [Obsolete("Use default constructor for a new InboundsArgs", true)]
        protected virtual InboundsArgs GetNewArgs(Match match)
        {
            return new InboundsArgs(match);
        }

        /// <summary>
        /// Adds a list of greetings to the statement check list
        /// <para>NOTE: Greeting checks added this way use RegexOptions.IgnoreCase</para>
        /// </summary>
        /// <param name="greetings">List of greetings to check for</param>
        /// <param name="callbacks">Callbacks to invoke on greetings filtered</param>
        public InboundsFilter AddBasicGreetings(IEnumerable<string> greetings, params Action<MonitorArgs>[] callbacks)
        {
            AddFilter(GREETING_FORMAT, string.Join(RAW_SEPARATOR, greetings.ToArray()), RegexOptions.IgnoreCase, callbacks);
            return this;
        }
    }
}
