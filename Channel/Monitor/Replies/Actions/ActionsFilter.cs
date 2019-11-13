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

namespace IRCnect.Channel.Monitor.Replies.Actions
{
    /// <summary>
    /// Class providing actions event filter functionality for messages.
    /// </summary>
    public class ActionsFilter : RepliesFilter
    {

        const string ACTIONS_FORMAT = @"{0}";
        const string ACTIONS_PATTERN = @"(?:([A-Z]{4,}) [#|:]|(?:[a-z\.]+\.+[\w]+ \b([0-9]{3}))\b|([A-Z]{3,} \* [A-Z]{3,})|(?:[a-z\.]+\.+[\w]+ (([A-Z]{3,} \* [A-Z]{3,})|[A-Z]{3,})))";

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        public ActionsFilter(string pattern = ACTIONS_PATTERN)
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

            var args = new ActionsArgs(message);
            if (match.Success)
            {
                args.MatchFilters(m_Filters);
                //IRCnect.Utils.Logger.L(args.GetType().Name + "<-----" + args.data);
            }

            return args;
        }
    
        /// <summary>
        /// Adds a list of greetings to the statement check list
        /// <para>NOTE: Actions checks added this way use case sensitive regex</para>
        /// </summary>
        /// <param name="actionTypes">List of actions to check for.  Use one of the ActionTypes in the RCnect.Channel.Monitor.Replies.Actions namespace.</param>
        /// <param name="callbacks">Callbacks to invoke on action filtered</param>
        /// <returns>ActionsFilter type to chain filter additions.</returns>
        public ActionsFilter AddActionsFilters(IEnumerable<string> actionTypes, params Action<MonitorArgs>[] callbacks)
        {
            foreach (var action in actionTypes)
            {
                if (action == ActionTypes._NONE) continue;
                AddFilter(ACTIONS_FORMAT, action, RegexOptions.None, callbacks);
            }
            return this;
        }

        /// <summary>
        /// Filter all adds all the default filters to the list to check for.
        /// </summary>
        /// <param name="callbacks">Callbacks to invoke upon filtered.</param>
        /// <returns>ActionsFilter type to chain filter additions.</returns>
        new public ActionsFilter FilterAll(params Action<MonitorArgs>[] callbacks)
        {
            var actions = ActionTypes.All.Skip(1).ToArray();
            AddActionsFilters(actions, callbacks);
            return this;
        }

        /// <summary>
        /// Adds a list of replies to check for.
        /// </summary>
        /// <param name="messages">Messages to check for.</param>
        /// <param name="callbacks">Callbacks to invoke upon filtered.</param>
        /// <returns>RepliesFilter type to chain filter additions.</returns>
        [Obsolete("This method is not supported by ActionsFilter.  To use this method use a RepliesFilter.", true)]
        new public RepliesFilter AddRepliesFilter(IEnumerable<string> messages, params Action<MonitorArgs>[] callbacks)
        {
            return base.AddRepliesFilter(messages, callbacks);
        }
    }
}
