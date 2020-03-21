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
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor.Replies.Inbounds.Commands
{
    /// <summary>
    /// Args used by filters to store parsed command data.
    /// </summary>
    [Serializable]
    public sealed class CommandsArgs : InboundsArgs
    {        
        string m_Command = string.Empty;
        string m_Argument = string.Empty;
        string[] m_NArgument = new string[] { };

        /// <summary>
        /// Command if found in the matching/parsing.
        /// </summary>
        public string command { get { return m_Command; } }
        /// <summary>
        /// Argument if it exists in the command message.
        /// </summary>
        public string argument { get { return m_Argument; } }
        /// <summary>
        /// Multiple arguments if they exists in the command message.
        /// </summary>
        public string[] nArgument { get { return m_NArgument; } }

        /// <summary>
        /// Greeting if found in the matching/parsing
        /// </summary>
        [Obsolete("Greetings are not used in CommandsArgs class use InboundArgs to access the greeting.", true)]
        new public string greeting { get { return m_Greeting; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageMatch">Previously match data for further parsing/matching.</param>
        [Obsolete("Use default constructor passing the raw data as the only parameter", true)]
        public CommandsArgs(Match messageMatch) : base(messageMatch) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data passes in before prcessing (usually raw data)</param>
        public CommandsArgs(string data) : base(data) { }

        /// <summary>
        /// Compares data to filters for matches of specific data.
        /// </summary>
        /// <param name="filters">Filters to match data against.</param>
        public override void MatchFilters(Dictionary<Regex, Action<MonitorArgs>> filters)
        {
            /*string[] dataSplit = this.data.Split(' ');

            string item = s_Names.FirstOrDefault(x => dataSplit.Contains(x));

            if (item != null)
            {

                foreach (var filter in filters)
                {
                    Match match = filter.Key.Match(this.said);
                    if (match.Success)
                    {
                        m_Command = match.Groups[1].Value;
                        m_Argument = match.Groups[2].Value.Trim(' ');
                        m_NArgument = match.Value.Split(' ').Skip(1).ToArray();

                        filter.Value.Invoke(this);
                        break;
                    }
                }
            }*/
            foreach (var filter in filters)
            {
                Match match = filter.Key.Match(this.said);
                if (match.Success)
                {
                    m_Command = match.Groups[1].Value;
                    m_Argument = match.Groups[2].Value.Trim(' ');
                    m_NArgument = match.Value.Split(' ').Skip(1).ToArray();

                    filter.Value.Invoke(this);
                    break;
                }
            }
        }
    }
}
