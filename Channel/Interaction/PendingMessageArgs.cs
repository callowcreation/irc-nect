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

namespace IRCnect.Channel.Interaction
{
    /// <summary>
    /// Creates a pending message object that contains all relavant information for the message that was not sent.
    /// </summary>
    public class PendingMessageArgs : EventArgs
    {
        /// <summary>
        /// Empty PendingEventArgs args to used instead of null
        /// </summary>
        public class EmptyPendingMessageArgs : PendingMessageArgs
        {
            /// <summary>
            /// Constructor
            /// <para>Empty PendingEventArgs args to used instead of null</para>
            /// </summary>
            public EmptyPendingMessageArgs()
                : base(PendingMessageArgs.Type.Chat, string.Empty, new string [0])
            {

            }
        }

        /// <summary>
        /// Empty PendingEventArgs args to used instead of null.
        /// </summary>
        new public readonly static EmptyPendingMessageArgs Empty = new EmptyPendingMessageArgs();

        /// <summary>
        /// 
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Send message to chat
            /// <para>Default value</para>
            /// </summary>
            Chat,
            /// <summary>
            /// Send message to user via whisper
            /// </summary>
            Whisper
        }

        /// <summary>
        /// The pending message
        /// </summary>
        public readonly PendingMessageArgs.Type type;

        /// <summary>
        /// The pending message
        /// </summary>
        public readonly string message;

        /// <summary>
        /// Pending message string format arguments
        /// </summary>
        public readonly object[] args;

        /// <summary>
        /// Constructor
        /// <para>Creates a pending message object that contains all relevant information for the message that was not sent.</para>
        /// </summary>
        /// <param name="message">The full composed message</param>
        [System.Obsolete("Causes duplicate irc send format to occur, use alternate constructor PendingMessageArgs(string message, string[] args)", true)]
        public PendingMessageArgs(string message)
        {
            this.message = message;
        }

        /// <summary>
        /// Constructor
        /// <para>Creates a pending message object that contains all relevant information for the message that was not sent.</para>
        /// </summary>
        /// <param name="message">The full composed message</param>
        /// <param name="args">String format arguments</param>
        [System.Obsolete("Limits message sends to chat only to use whispers, use alternate constructor PendingMessageArgs(string message, PendingMessageArgs.Type type, string[] args)", false)]
        public PendingMessageArgs(string message, object[] args)
        {
            this.message = message;
            this.args = args;
        }

        /// <summary>
        /// Constructor
        /// <para>Creates a pending message object that contains all relevant information for the message that was not sent.</para>
        /// </summary>
        /// <param name="type">How to send message</param>
        /// <param name="message">The full composed message</param>
        /// <param name="args">String format arguments</param>
        public PendingMessageArgs(PendingMessageArgs.Type type, string message, object[] args)
        {
            this.type = type;
            this.message = message;
            this.args = args;
        }
    }
}
