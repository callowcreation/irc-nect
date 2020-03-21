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
    /// Create parameters to send whisper message
    /// </summary>
    public class WhisperParams
    {
        /// <summary>
        /// WHo the message is whispered to
        /// </summary>
        public readonly string toNick;

        /// <summary>
        /// The message to whisper
        /// </summary>
        public readonly string toMessage;

        WhisperParams(string message)
        {
            int length = message.IndexOf(' ');
            toNick = message.Substring(0, length);
            toMessage = message.Substring(toNick.Length);
        }

        /// <summary>
        /// Create parameters to send whisper message
        /// </summary>
        /// <param name="message"></param>
        public static WhisperParams Create(string message)
        {
            return new WhisperParams(message);
        }
    }
}
