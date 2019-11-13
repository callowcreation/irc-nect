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

namespace IRCnect.Channel.Monitor.Replies.Inbounds.Links
{
    /// <summary>
    /// Args used by filters to store parsed link data.
    /// </summary>
    [Serializable]
    public sealed class LinksArgs : InboundsArgs
    {
        string m_Link;

        /// <summary>
        /// Link found during the matching/parsing
        /// </summary>
        public string link { get { return m_Link; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messageMatch">Previously match data for further parsing/matching.</param>
        public LinksArgs(Match messageMatch)
            : base(messageMatch)
        { }

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
                    m_Link = match.Value;

                    filter.Value.Invoke(this);
                    return;
                }
            }
        }
    }
}
