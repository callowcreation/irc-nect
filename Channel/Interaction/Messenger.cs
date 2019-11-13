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

using IRCnect.Utils;
using System;
using System.IO;

namespace IRCnect.Channel.Interaction
{
    /// <summary>
    /// Abstract class providing the functionality for sending messages along the IRC stream.
    /// </summary>
    public abstract class Messenger
    {
        
        StreamWriter m_Writer;
        
        DateTime m_LastSendTime = DateTime.Now;
        
        double m_SendDelaySeconds = 2.0;

        /// <summary>
        /// Delay to wait between allowing a send message method call.
        /// </summary>
        public double SendDelaySeconds
        {
            get { return m_SendDelaySeconds; }
            set { m_SendDelaySeconds = value; }
        }

        /// <summary>
        /// Has throttle timer elapsed and a message can be sent.
        /// </summary>
        public bool CanSendMessage 
        { 
            get { return m_LastSendTime < DateTime.Now; } 
        }

        /// <summary>
        /// Sets the local stream writer.
        /// </summary>
        public StreamWriter writer
        {
            set { m_Writer = value; }
        }

        /// <summary>
        /// Invokes each time an attempt to send a message and the throttle was not at zero and send message did not send the message.
        /// </summary>
        public event EventHandler<PendingMessageArgs> onMessagePending;

        /// <summary>
        /// Constructor
        /// </summary>
        public Messenger() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="writer">IRC stream writer to write to the IRC stream.</param>
        public Messenger(StreamWriter writer)
        {
            m_Writer = writer;
        }

        /// <summary>
        /// Sends a message along the IRC stream.
        /// <para>This method is throttled and can be controlled with the SendDelaySeconds property.</para>
        /// </summary>
        /// <param name="type">How to send.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="args">Parameters if message contains formatting.</param>
        public void Send(PendingMessageArgs.Type type, string message, params object[] args)
        {
            if (CanSendMessage)
            {
                SendUnsafe(type, message, args);
            }
            else
            {
                if (onMessagePending != null)
                {
                    onMessagePending.Invoke(this, new PendingMessageArgs(type, message, args));
                }
            }
        }

        /// <summary>
        /// Sends a message along the IRC stream.
        /// <para>NOTE: Use Send(message, args) if you don't know which send method to use.</para>
        /// <para>WARNING: Use this method with caution as it is not thorttled and will send messages as called.  This may exceed the allowed quota per minute.</para>
        /// </summary>
        /// <param name="type">How to send.</param>
        /// <param name="message">Message to send.</param>
        /// <param name="args">Parameters if message contains formatting.</param>
        public void SendUnsafe(PendingMessageArgs.Type type, string message, params object[] args)
        {
            message = ComposeAndPrepareMessage(type, message, args);
            StreamIRC.WriteAllLines(m_Writer, message);
            UpdateLastSendTime();
        }

        /// <summary>
        /// Sends a message along the IRC stream.
        /// <para>This method is throttled and can be controlled with the SendDelaySeconds property.</para>
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="args">Parameters if message contains formatting.</param>
        [System.Obsolete("Limits message sends to chat only to use whispers, use alternate method Send(PendingMessageArgs.Type type, string message, args)", false)]
        public void Send(string message, params object[] args)
        {
            if (CanSendMessage)
            {
                SendUnsafe(message, args);
            }
            else
            {
                if (onMessagePending != null)
                {
                    onMessagePending.Invoke(this, new PendingMessageArgs(PendingMessageArgs.Type.Chat, message, args));
                }
            }
        }

        /// <summary>
        /// Sends a message along the IRC stream.
        /// <para>NOTE: Use Send(message, args) if you don't know which send method to use.</para>
        /// <para>WARNING: Use this method with caution as it is not thorttled and will send messages as called.  This may exceed the allowed quota per minute.</para>
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <param name="args">Parameters if message contains formatting.</param>
        [System.Obsolete("Limits message sends to chat only to use whispers, use alternate method SendUnsafe(PendingMessageArgs.Type type, string message, args)", false)]
        public void SendUnsafe(string message, params object[] args)
        {
            message = ComposeAndPrepareMessage(PendingMessageArgs.Type.Chat, message, args);
            StreamIRC.WriteAllLines(m_Writer, message);
            UpdateLastSendTime();
        }

        string ComposeAndPrepareMessage(PendingMessageArgs.Type type, string message, object[] args)
        {
            message = ComposeMessage(message, args);
            message = PreparedMessage(type, message);
            return message;
        }

        void UpdateLastSendTime()
        {
            m_LastSendTime = DateTime.Now.AddSeconds(SendDelaySeconds);
        }

        /// <summary>
        /// Hook to prepare the message for sending.
        /// <para>Provided to allow for IRC specific message formats.</para>
        /// </summary>
        /// <param name="message">Message to prepare for sending.</param>
        /// <returns>A prepared message for derived class IRC connection.</returns>
        [System.Obsolete("Limits message sends to chat only to use whispers, use alternate method PreparedMessage(PendingMessageArgs.Type type, string message)", false)]
        public abstract string PreparedMessage(string message);

        /// <summary>
        /// Hook to prepare the message for sending.
        /// <para>Provided to allow for IRC specific message formats.</para>
        /// </summary>
        /// <param name="type">How to prepare the message.</param>
        /// <param name="message">Message to prepare for sending.</param>
        /// <returns>A prepared message for derived class IRC connection.</returns>
        public abstract string PreparedMessage(PendingMessageArgs.Type type, string message);

        static string ComposeMessage(string message, object[] args)
        {
            message = args.Length > 0 ? string.Format(message, args) : message;
            return message;
        }

    }
}
