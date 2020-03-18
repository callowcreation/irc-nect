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
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor.Replies.Inbounds
{
    /// <summary>
    /// Args used by filters to store parsed inbound data.
    /// </summary>
    [Serializable]
    public class InboundsArgs : RepliesArgs
    {

        /// <summary>
        /// Greeting if found in the matching/parsing.
        /// </summary>
        protected string m_Greeting = string.Empty;

        /// <summary>
        /// Match expression if match was successful.
        /// </summary>
        public Match messageMatch { get; set; } = null;

        /// <summary>
        /// Nick name of the user found in the parsed message.
        /// </summary>
        public string nick { get { return messageMatch.Groups[1].Value; } }
        /// <summary>
        /// Channel the message came from.
        /// </summary>
        public string channel { get { return messageMatch.Groups[2].Value; } }
        /// <summary>
        /// What was said in the message.
        /// </summary>
        public string said { get { return messageMatch.Groups[3].Value; } }

        /// <summary>
        /// Greeting if found in the matching/parsing.
        /// </summary>
        public string greeting { get { return m_Greeting; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageMatch">Previously match data for further parsing/matching.</param>
        [Obsolete("Use default constructor passing the raw data as the only parameter", true)]
        public InboundsArgs(Match messageMatch) : base(messageMatch.Value) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data passes in before prcessing (usually raw data)</param>
        public InboundsArgs(string data) : base(data) { }

        /// <summary>
        /// Compares data to filters for matches of specific data.
        /// </summary>
        /// <param name="filters">Filters to match data against.</param>
        public override void MatchFilters(Dictionary<Regex, Action<MonitorArgs>> filters)
        {
            foreach (var filter in filters)
            {
                Match match = filter.Key.Match(this.said);
                if (match.Success)
                {
                    int last = match.Groups.Count - 1;

                    m_Greeting = match.Groups[last].Value;

                    filter.Value.Invoke(this);
                    return;
                }
            }
        }
    }
}
