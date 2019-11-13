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

namespace IRCnect.Channel.Interaction.Native
{
    /// <summary>
    /// Ids for the Native commands for the server/host.
    /// </summary>
    public enum CommandID
    {
        /// <summary>
        /// Command is unknown.
        /// </summary>
        UNKNOWN = -1,
        /// <summary>
        /// Format /help
        /// </summary>
        HELP,
        /// <summary>
        /// Format /me {0}
        /// </summary>
        ME,
    }
}
