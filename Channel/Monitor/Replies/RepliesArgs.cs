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

namespace IRCnect.Channel.Monitor.Replies
{
    /// <summary>
    /// Args used by filters to store parsed replies catch all data.
    /// </summary>
    [Serializable]
    public class RepliesArgs : MonitorArgs
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data passes in before prcessing (usually raw data)</param>
        public RepliesArgs(string data)
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
                    filter.Value.Invoke(this);
                    return;
                }
            }
        }

    }
}
