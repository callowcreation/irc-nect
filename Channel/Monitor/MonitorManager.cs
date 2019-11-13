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

namespace IRCnect.Channel.Monitor
{
    internal sealed class MonitorManager : IMonitor
    {
        object m_Locker = new object();

        public static event EventHandler<MonitorArgs> OnReceived;

        IMonitorBase[] m_Monitors;

        IMonitorBase[] Monitors
        {
            get { lock (m_Locker) return m_Monitors; }
            set { lock (m_Locker) m_Monitors = value; }
        }

        internal MonitorManager()
        {
            Initialize();
        }

        void Initialize()
        {
            Monitors = new IMonitorBase[0];
            IsInitialized = true;
        }

        void Add(IMonitorBase monitor, bool subscribeToReceived)
        {
            Func<IMonitorBase, bool> addMonitor = (IMonitorBase m) =>
            {
                if (!Monitors.Contains(m))
                {
                    var tempMonitors = Monitors.ToList();
                    tempMonitors.Add(m);
                    Monitors = tempMonitors.ToArray();

                    //IRCnect.Utils.Logger.L("Monitors: {0}", Monitors.Length);
                    return true;
                }
                return false;
            };
            if (addMonitor.Invoke(monitor))
            {
                if (subscribeToReceived)
                {
                    SubscribeToReceived(monitor);
                }
            }
        }

        void SubscribeToReceived(IMonitorBase monitor)
        {
            monitor.onReceived -= OnReceived;
            monitor.onReceived += OnReceived;
        }

        internal void AddMonitor(IMonitorBase monitor, bool subscribeToReceived = false)
        {
            Add(monitor, subscribeToReceived);
        }

        internal bool IsInitialized { get; private set; }
        bool IMonitor.IsInitialized
        {
            get { return IsInitialized; }
            set { IsInitialized = value; }
        }

        internal void Monitor()
        {
            string message = null;

            Action<IMonitorBase> getMessage = (monitor) => { if (string.IsNullOrEmpty(message)) message = monitor.ReadLine(); };

            foreach (var monitor in Monitors)
            {
                if (!monitor.IsInitialized) continue;

                getMessage.Invoke(monitor);

                if (!string.IsNullOrEmpty(message))
                {
                    foreach (MonitorArgs monitorArgs in monitor.Parse(message))
                    {
                        monitor.InvokeRecieved(monitorArgs);
                        if (OnReceived != null)
                        {
                            OnReceived(this, monitorArgs);
                        }
                    }
                }
            }
        }
        void IMonitor.Monitor()
        {
            Monitor();
        }
    }
}
