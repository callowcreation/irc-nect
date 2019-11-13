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
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor.Replies.Inbounds.Links
{
    /// <summary>
    /// Class providing links filter functionality for messages.
    /// </summary>
    public class LinksFilter : InboundsFilter
    {
        const string LINK_FORMAT = "{0}";

        const string LINK_PATTERN = @"\b(?:(?:https?|ftp|file)://|www\.|ftp\.)(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#/%=~_|$?!:,.]*\)|[A-Z0-9+&@#/%=~_|$])";

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        public LinksFilter(string pattern = MESSAGE_PATTERN)
            : base(pattern)
        { }

        /// <summary>
        /// Gets the specific type of args required for this class to store data.
        /// <para>Override to provide unique class interface args.</para>
        /// </summary>
        /// <param name="match">The match that will be preformed.</param>
        /// <returns>Args used by filters to store parsed data.</returns>
        protected override InboundsArgs GetNewArgs(Match match)
        {
            return new LinksArgs(match);
        }

        /// <summary>
        /// Applies default link filter.
        /// <para>Matches against links contained in the message.</para>
        /// </summary>
        /// <param name="callbacks">Callbacks to invoke upon filtered.</param>
        /// <returns>LinksFilter type to chain filter additions.</returns>
        public LinksFilter UseDefaultLinkFilter(params Action<MonitorArgs>[] callbacks)
        {
            AddFilter(LINK_FORMAT, LINK_PATTERN, RegexOptions.IgnoreCase, callbacks);
            return this;
        }
    }
}
