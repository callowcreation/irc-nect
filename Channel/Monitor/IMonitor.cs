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

namespace IRCnect.Channel.Monitor
{
    /// <summary>
    /// Interface to monitor IRC incoming stream.
    /// </summary>
    public interface IMonitor
    {
        /// <summary>
        /// Is the monitor initialized.
        /// <para>Usually meaning the reader is initialized.</para>
        /// </summary>
        bool IsInitialized { get; set; }

        /// <summary>
        /// The call to monitor the incoming IRC stream.
        /// </summary>
        void Monitor();

    }
}
