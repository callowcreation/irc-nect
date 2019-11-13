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

namespace IRCnect.Channel.Interaction.Native
{
    /// <summary>
    /// Native commands are those provided by the IRC srever/hosts.
    /// </summary>
    public class NativeCommands
    {
        static string CommandHelpString = "Command syntax is: ";

        /// <summary>
        /// Commands provided by most IRC servers/hosts.
        /// </summary>
        public static readonly Dictionary<CommandID, string> Commands = new Dictionary<CommandID, string>()
        {
            { CommandID.UNKNOWN, CommandHelpString + "{0}" },
            { CommandID.HELP, "/help" },
            { CommandID.ME, "/me {0}" },
        };

        /// <summary>
        /// Send or Invoke will send the commandMessags if it is a message or invoke the command if it parses into a native command.
        /// </summary>
        /// <param name="sendMessage">The method to send the message or command.</param>
        /// <param name="commandMessage">The message or fully formatted command.</param>
        /// <param name="commands">The commands list to check against.</param>
        public static void SendOrInvoke(Action<string, object[]> sendMessage, string commandMessage, Dictionary<Enum, string> commands)
        {
            if (string.IsNullOrEmpty(commandMessage)) return;
            if (commandMessage.StartsWith("/"))
            {
                var cmdSplit = commandMessage.Split(' ');
                string command = cmdSplit.Length >= 1 ? cmdSplit[0] : null;
                string nick = cmdSplit.Length >= 2 ? cmdSplit[1] : null;
                string param = cmdSplit.Length > 2 ? cmdSplit[2] : null;

                foreach (var item in commands)
                {
                    if (item.Value.StartsWith(command))
                    {
                        Invoke(sendMessage, item.Value, nick, param);
                        return;
                    }
                }
                sendMessage.Invoke(commandMessage, new object[] { });
                return;
            }
            else
            {
                sendMessage.Invoke(commandMessage, new object[] { });
            }
        }

        /// <summary>
        /// Invoke a native command on the server/host.
        /// </summary>
        /// <param name="sendMessage">The method to send the message or command.</param>
        /// <param name="command">The command to call on the server/host.</param>
        /// <param name="param1">The first parameter if required.</param>
        /// <param name="param2">The second parameter if required.</param>
        public static void Invoke(Action<string, object[]> sendMessage, string command, object param1 = null, object param2 = null)
        {
            var cmd = command.ToString();
            if (cmd.Contains("{0}") && cmd.Contains("{1}"))
            {
                if (param1 != null && param2 != null)
                {
                    sendMessage.Invoke(cmd, new[] { param1, param2 });
                }
                else if (param1 != null)
                {
                    sendMessage.Invoke("{0} {1}", new[] { CommandHelpString, string.Format(cmd, param1, "<param>") });
                }
                else if (param2 != null)
                {
                    sendMessage.Invoke("{0} {1}", new[] { CommandHelpString, string.Format(cmd, "<param>", param2) });
                }
                else
                {
                    sendMessage.Invoke("{0} {1}", new[] { CommandHelpString, string.Format(cmd, "<param1>", "<param2>") });
                }
                return;
            }
            else if (cmd.Contains("{0}"))
            {
                if (param1 != null)
                {
                    sendMessage.Invoke(cmd, new[] { param1 });
                }
                else if (param2 != null)
                {
                    sendMessage.Invoke(cmd, new[] { param2 });
                }
                else
                {
                    sendMessage.Invoke("{0} {1}", new[] { CommandHelpString, string.Format(cmd, "<param>") });
                }
                return;
            }
            else
            {
                sendMessage.Invoke(string.Concat(cmd, "{0}"), new[] { "" });
                return;
            }
            //sendMessage.Invoke("Unknown command ID {0}", new[] { command.ToString() });
            //return;
        }

    }
}
