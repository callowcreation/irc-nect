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

namespace IRCnect.Channel.Monitor
{
    /// <summary>
    /// Interface for IRC inbound stream reader.
    /// </summary>
    public interface IMonitorBase : IMonitor
    {
        /// <summary>
        /// Event invoked when a message is received from the iIRC stream.
        /// </summary>
        event EventHandler<MonitorArgs> onReceived;

        /// <summary>
        /// Filters used to check the stream for specific information.
        /// </summary>
        MonitorFilter[] Filters { get; set; }

        /// <summary>
        /// The parser for the stream information.
        /// </summary>
        /// <param name="message">The incomming message.</param>
        /// <returns>List of the data parsed for the filters.</returns>
        IList<MonitorArgs> Parse(string message);

        /// <summary>
        /// Reads a line for the IRC stream.
        /// </summary>
        /// <returns>Returns the IRC line read.</returns>
        string ReadLine();
        
        /// <summary>
        /// Try to read the message for the IRC stream.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        /// <returns>True if there was a message on the stream, False if not.</returns>
        bool TryGet(out string message);

        /// <summary>
        /// Invoke all subscribed events for data received.
        /// </summary>
        /// <param name="monitorArgs">Data to send for</param>
        void InvokeRecieved(MonitorArgs monitorArgs);

    }
}
