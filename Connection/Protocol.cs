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

namespace IRCnect.Connection
{
    /// <summary>
    /// Helper class containing connection and required IRC protocols.
    /// <para>Predifined preformatted strings to interact with IRC clients/servers.</para>
    /// </summary>
    public class Protocol
    {

        internal const int MAX_CHANNEL_NAME = 50;
        /// <summary>
        /// Channel prefix used #
        /// </summary>
        public const string CHANNEL_PREFIX = "#";

        /// <summary>
        /// Connection join prefix with channel #prefix identifier.
        /// </summary>
        public const string JOIN = "JOIN " + CHANNEL_PREFIX;
        /// <summary>
        /// Connection part prefix with channel #prefix identifier.
        /// </summary>
        public const string PART = "PART " + CHANNEL_PREFIX;

        /// <summary>
        /// Connection ping prefix.
        /// </summary>
        public const string PING_COMMAND = "PING ";
        /// <summary>
        /// Connection ping prefix with colon (:).
        /// </summary>
        public const string PING_MESSAGE = "PING :";
        /// <summary>
        /// Connection ping prefix format PING {0} {1}.
        /// </summary>
        public const string PING_REQUEST_FORMAT = "PING {0} {1}";

        /// <summary>
        /// Connection pong prefix.
        /// </summary>
        public const string PONG_COMMAND = "PONG ";
        /// <summary>
        /// Connection pong prefix with colon (:).
        /// </summary>
        public const string PONG_MESSAGE = "PONG :";
        /// <summary>
        /// Connection pong prefix format PONG {0} {1}.
        /// </summary>
        public const string PONG_REQUEST_FORMAT = "PONG {0} {1}";

        /// <summary>
        /// Sets/Gets user mode
        /// <para>Default 8 (i - marks a users as invisible)</para>
        /// <para>Refer to Internet Relay Chat: Client Protocol for all user modes</para>
        /// </summary>
        public const int MODE = 8;
        /// <summary>
        /// Connection password prefix.
        /// </summary>
        public const string PASS = "PASS ";

        /// <summary>
        /// Connection nick (Nick Name) prefix.
        /// </summary>
        public const string NICK = "NICK ";

        /// <summary>
        /// Example below
        /// <para>:Angel!wings@irc.org PRIVMSG Wiz :Are you receiving this message?</para>
        /// </summary>
        public const string PRIVMSG_FORMAT = ":{0}!{1}@{2} PRIVMSG {3} :{4}";
        /// <summary>
        /// Shorter form for sending a message to an IRC channel
        /// </summary>
        public const string PRIVMSG_FORMAT_SHORT = "PRIVMSG {0} {1}";
        //PRIVMSG #jtv :/w otherusername HeyGuys PRIVMSG_FORMAT_SHORT
        /// <summary>
        /// Example below
        /// <para>:Angel!wings@irc.org WHISPER Wiz :Are you receiving a wisper this message?</para>
        /// </summary>
        public const string WHISPER_FORMAT = ":{0}!{1}@{2} PRIVMSG #{3} :/w {4} :{5}";
        /// <summary>
        /// Shorter form for sending a wisper message to an IRC channel
        /// </summary>
        public const string WHISPER_FORMAT_SHORT = "WHISPER {0} :{1}";

        /// <summary>
        /// Parameter {0} username and {1} realname may contain spaces
        /// </summary>
        /// <param name="mode">Refer to Internet Relay Chat: Client Protocol for all user modes
        /// <para>Default 8 (i - marks a users as invisible)</para></param>
        /// <returns>Format for the user string to connect to IRC server.</returns>
        public static string GetUserFormat(int mode = MODE)
        {
            return "USER {0} " + mode + " * :{1}";
        }

        /// <summary>
        /// Connections strings to connect to an IRC channel.
        /// <para>These are general purpose connection requirements for MOST IRC servers/hosts.</para>
        /// </summary>
        /// <param name="nick">Who is connecting, nick name.</param>
        /// <param name="oauth">Authorization key or password.</param>
        /// <param name="mode">Connection mode (Leave alone if you are not sure).</param>
        /// <param name="realName">Real name, recommended to use an alias and NOT a Real name.</param>
        /// <returns>Array of prepares connection strings.</returns>
        public static string[] NickConnectionStrings(string nick, string oauth, int mode = MODE, string realName = "")
        {
            //Required Connection Strings
            return new[] {
                string.Concat(PASS, oauth),
                string.Concat(NICK, nick),
                string.Format(GetUserFormat(mode), nick, string.IsNullOrEmpty(realName) ? nick : realName)
            };
        }

    }
}
