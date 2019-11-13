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

namespace IRCnect.Workers
{
    /// <summary>
    /// Worker to mainly handle Monitor reading.
    /// </summary>
    public abstract class Worker : IWorker
    {
        /// <summary>
        /// Worker has been started and is running.
        /// </summary>
        public virtual bool IsRunning { get; protected set; }
        bool IWorker.IsRunning { get { return IsRunning; } set { IsRunning = value; } }

        /// <summary>
        /// Starts the worker
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Stops the active worker..
        /// <para>Override to provide addition stop instructions.</para>
        /// </summary>
        public virtual void Stop()
        {
            IsRunning = false;
        }
        void IWorker.Stop()
        {
            Stop();
        }
    }
}
