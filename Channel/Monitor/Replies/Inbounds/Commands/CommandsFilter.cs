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
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor.Replies.Inbounds.Commands
{
    /// <summary>
    /// Class providing user commands filter functionality for messages.
    /// </summary>
    public class CommandsFilter : InboundsFilter
    {
        static string GetCommandFormatSyntax(string commandSymbol)
        {
            string symbol = EscapeChars.FirstOrDefault(x => x == commandSymbol);

            string formatSyntax = string.IsNullOrEmpty(symbol) ? "({0}{1})" : "(\\{0}{1})";

            return string.Format(formatSyntax, commandSymbol, "{0}");
        }

        static string GetCommandFormat(string commandSymbol)
        {
            return "^" + GetCommandFormatSyntax(commandSymbol) + "$";
        }

        static string GetParameterizedCommandFormat(string commandSymbol)
        {
            return "^" + GetCommandFormatSyntax(commandSymbol) + @"\s(\S+)?$";
        }

        static string GetNParameterizedCommandFormat(string commandSymbol)
        {
            return "^" + GetCommandFormatSyntax(commandSymbol) + @"(\s?\S+\b)+$";
        }

        /// <summary>
        /// Command sybmol to use as a prefix for all commands
        /// </summary>
        public readonly string commandSymbol;

        /// <summary>
        /// Char used as command symbols that need to be escaped for regex
        /// </summary>
        public static List<string> EscapeChars { get; } = new List<string>(new string [] { "$", "-" });

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        [System.Obsolete("Use CommandsFilter constructor that allows for custom command symbols")]
        public CommandsFilter(string pattern = MESSAGE_PATTERN)
            : base(pattern)
        {
            commandSymbol = "!";
        }

        /// <summary>
        /// Constructor 
        /// <para>
        /// Use InboundsFilter.MESSAGE_PATTERN as a default value for the pattern parameter
        /// </para>
        /// </summary>
        /// <param name="commandSymbol">Command sybmol to use as a prefix for all commands.</param>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        public CommandsFilter(string commandSymbol, string pattern)
            : base(pattern)
        {
            this.commandSymbol = commandSymbol;
        }


        /// <summary>
        /// Parse a chat message and returns matched expresions
        /// </summary>
        /// <param name="message">Raw chat response message to parse</param>
        /// <returns>Matched expressions class</returns>
        public override MonitorArgs Parse(string message)
        {
            Match match = m_Regex.Match(message);

            var args = new CommandsArgs(message);
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
        /// Adds a command to the statement check list
        /// <para>NOTE: Command checks added this way are case sensitive</para>
        /// </summary>
        /// <param name="command">Command to add to list.  Exclude the !, this will be automatically added.</param>
        /// <param name="callbacks">Callbacks to invoke on commands filtered</param>
        public CommandsFilter AddBasicCommand(string command, params Action<MonitorArgs>[] callbacks)
        {
            AddFilter(GetCommandFormat(commandSymbol), command, RegexOptions.None, callbacks);
            return this;
        }

        /// <summary>
        /// Adds a command to the statement check list and checks for a single parameter.
        /// </summary>
        /// <param name="command">Command to add to list.  Exclude the !, this will be automatically added.</param>
        /// <param name="callbacks">Callbacks to invoke on commands filtered</param>
        public CommandsFilter AddParameterizedCommand(string command, params Action<MonitorArgs>[] callbacks)
        {
            AddFilter(GetParameterizedCommandFormat(commandSymbol), command, RegexOptions.None, callbacks);
            return this;
        }

        /// <summary>
        /// Adds a command to the statement check list and checks for a n number of parameter
        /// <para>Will not check for a no parameters</para>
        /// <para>NOTE: Seperate arguments with spaces. Command checks added this way are case sensitive.</para>
        /// </summary>
        /// <param name="command">Command to add to list.  Exclude the !, this will be automatically added.</param>
        /// <param name="callbacks">Callbacks to invoke on commands filtered</param>
        public CommandsFilter AddNParameterCommand(string command, params Action<MonitorArgs>[] callbacks)
        {
            AddFilter(GetNParameterizedCommandFormat(commandSymbol), command, RegexOptions.None, callbacks);
            return this;
        }


        /// <summary>
        /// Adds a list of greetings to the statement check list
        /// <para>NOTE: Greeting checks added this way use RegexOptions.IgnoreCase</para>
        /// </summary>
        /// <param name="greetings">List of greetings to check for</param>
        /// <param name="callbacks">Callbacks to invoke on greetings filtered</param>
        [Obsolete("This method is not supported in CommandsFilter to use this method use InboundsFilter.", true)]
        new public InboundsFilter AddBasicGreetings(IEnumerable<string> greetings, params Action<MonitorArgs>[] callbacks)
        {
            return base.AddBasicGreetings(greetings, callbacks); ;
        }

        /// <summary>
        /// Adds a list of greetings to the statement check list
        /// <para>NOTE: Greeting checks added this way use RegexOptions.IgnoreCase</para>
        /// </summary>
        /// <param name="messages">List of greetings to check for</param>
        /// <param name="callbacks">Callbacks to invoke on greetings filtered</param>
        /// <returns>RepliesFilter type to chain filter additions.</returns>
        [Obsolete("This method is not supported in CommandsFilter to use this method use RepliesFilter.", true)]
        new public RepliesFilter AddRepliesFilter(IEnumerable<string> messages, params Action<MonitorArgs>[] callbacks)
        {
            return base.AddRepliesFilter(messages, callbacks);
        }

        /// <summary>
        /// Adds all default filters to the check list.
        /// </summary>
        /// <param name="callbacks">Callbacks to invoke upon filtered.</param>
        /// <returns>RepliesFilter type to chain filter additions.</returns>
        [Obsolete("This method is not supported in CommandsFilter to use this method use RepliesFilter.", true)]
        new public RepliesFilter FilterAll(params Action<MonitorArgs>[] callbacks)
        {
            return base.FilterAll(callbacks);
        }
    }
}
