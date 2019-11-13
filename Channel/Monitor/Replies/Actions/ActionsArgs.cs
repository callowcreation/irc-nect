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

namespace IRCnect.Channel.Monitor.Replies.Actions
{
    /// <summary>
    /// Args used by filters to store parsed actions event data.
    /// </summary>
    [Serializable]
    public class ActionsArgs : RepliesArgs
    {
        string m_Type = ActionTypes._NONE;

        /// <summary>
        /// Type of actions this instance represents.
        /// </summary>
        public string type { get { return m_Type; } }

        /// <summary>
        /// Any who sent the actions event.
        /// </summary>
        public string who { get; private set; }

        /// <summary>
        /// Any message pulled from the actions event.
        /// </summary>
        public string message { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data passes in before prcessing (usually raw data)</param>
        public ActionsArgs(string data)
            : base(data)
        { }

        /// <summary>
        /// Compares data to filters for matches of specific data.
        /// </summary>
        /// <param name="filters">Filters to match data against.</param>
        public override void MatchFilters(Dictionary<Regex, Action<MonitorArgs>> filters)
        {
            foreach (var filter in filters)
            {
                Match match = filter.Key.Match(this.data);
                if (match.Success)
                {
                    m_Type = match.Value;
                    this.message = ActionTypes.GetLastPart(this);

                    string[] split = this.data.Split(new[] { ':' }, System.StringSplitOptions.RemoveEmptyEntries);
                    string detailInfo = split.Length > 1 ? split[1] : string.Empty;
                    if (string.IsNullOrEmpty(detailInfo) == false)
                    {
                        string[] detailSplit = detailInfo.Split(new[] { '!' }, System.StringSplitOptions.RemoveEmptyEntries);
                        this.who = detailSplit[0];
                    }

                    filter.Value.Invoke(this);
                    return;
                }
            }
        }
    }
}
