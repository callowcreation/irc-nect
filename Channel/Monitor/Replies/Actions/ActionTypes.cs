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

using System.Collections.Generic;
using System.Linq;

namespace IRCnect.Channel.Monitor.Replies.Actions
{
    /// <summary>
    /// Default actions that are available to filter for.
    /// </summary>
    public class ActionTypes
    {
        /// <summary>
        /// No Action specified.
        /// </summary>
        public const string _NONE = "_NONE";
        /// <summary>
        /// Capabilities were acknowledged.
        /// </summary>
        public const string CAP_ACK = "CAP * ACK";
        /// <summary>
        /// Capabilities were not acknowledged and failed.
        /// </summary>
        public const string CAP_NAK = "CAP * NAK";
        /// <summary>
        /// Message inbound chat.
        /// </summary>
        public const string PRIVMSG = "PRIVMSG";
        /// <summary>
        /// Message inbound wisper.
        /// </summary>
        public const string WHISPER = "WHISPER";
        /// <summary>
        /// Global chat room state.
        /// <para>May be specific some IRC rooms only.</para>
        /// </summary>
        public const string GLOBALUSERSTATE = "GLOBALUSERSTATE";
        /// <summary>
        /// Gets the user states for the chat room.
        /// <para>May be specific some IRC rooms only.</para>
        /// </summary>
        public const string USERSTATE = "USERSTATE";
        /// <summary>
        /// Gets the room states for the chat room.
        /// <para>May be specific some IRC rooms only.</para>
        /// </summary>
        public const string ROOMSTATE = "ROOMSTATE";
        /// <summary>
        /// Checks for incomming ping test.
        /// </summary>
        public const string PING = "PING";
        /// <summary>
        /// Action found a notice in the message.
        /// </summary>
        public const string NOTICE = "NOTICE";
        /// <summary>
        /// An error occured and was reported by the server/host.
        /// </summary>
        public const string ERROR = "ERROR";
        /// <summary>
        /// Found join event in the message.
        /// </summary>
        public const string JOIN = "JOIN";
        /// <summary>
        /// Found a pard event in the message.
        /// </summary>
        public const string PART = "PART";
        /// <summary>
        /// Found a numeric actions, 376 End of /MOTD
        /// </summary>
        public const string NUMERIC_376 = "376";
        /// <summary>
        /// Found a numeric actions, 366 End of /NAMES
        /// </summary>
        public const string NUMERIC_366 = "366";

        /// <summary>
        /// All the actions types in one array.
        /// </summary>
        public static readonly List<string> All = new List<string>()
        { 
            CAP_ACK, PRIVMSG, WHISPER, GLOBALUSERSTATE, USERSTATE, ROOMSTATE, PING, NOTICE, ERROR, JOIN, PART, NUMERIC_376, NUMERIC_366 
        };

        /// <summary>
        /// Gets the last part of the actions event.
        /// <para>This is the message after the actual action.</para>
        /// </summary>
        /// <param name="actionsArgs">Args to parse for message.</param>
        /// <returns>The last part of the message if any.</returns>
        public static string GetLastPart(ActionsArgs actionsArgs)
        {
            int indexOf = actionsArgs.data.IndexOf(actionsArgs.type);
            int idxLength = actionsArgs.type.Length;

            var dataList = new List<string>();

            foreach (var item in actionsArgs.data.Split(new[] { ':' }, System.StringSplitOptions.RemoveEmptyEntries))
            {
                if (dataList.Count == 0)
                {
                    if (item.Contains(actionsArgs.type))
                    {
                        dataList.Add(item);
                    }
                }
                else
                {
                    dataList.Add(item);
                }
            }
            if (dataList.Count > 1)
            {
                var data = dataList.Skip(1).ToArray();
                var retVal = string.Join(":", data);
                return retVal;
            }

            return actionsArgs.data;
        }
    }
}
