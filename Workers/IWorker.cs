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

namespace IRCnect.Workers
{
    /// <summary>
    /// Worker interface to mainly handle Monitor reading.
    /// </summary>
    public interface IWorker
    {
        /// <summary>
        /// Worker has been started and is running.
        /// </summary>
        bool IsRunning { get; set; }
        /// <summary>
        /// Starts the worker
        /// </summary>
        void Run();
        /// <summary>
        /// Stops the active worker..
        /// </summary>
        void Stop();
    }

}
