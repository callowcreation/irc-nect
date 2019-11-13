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

using IRCnect.Connection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace IRCnect.Channel.Monitor
{
    /// <summary>
    /// Base class for IRC inbound stream reader.
    /// </summary>
    public abstract class MonitorBase : IMonitorBase
    {
        /// <summary>
        /// Event invoked when a message is received from the IRC stream.
        /// </summary>
        public static event EventHandler<MonitorArgs> OnReceived;

        /// <summary>
        /// Event invoked when a message is received from the IRC stream.
        /// </summary>
        public event EventHandler<MonitorArgs> onReceived;

        Client m_Client = null;

        StreamReader m_Reader = null;

        /// <summary>
        /// Filters used to check the stream for specific information.
        /// </summary>
        protected MonitorFilter[] Filters { get; set; } = null;

        MonitorFilter[] IMonitorBase.Filters { get { return Filters; } set { Filters = value; } }
        
        /// <summary>
        /// Method to log all messages that pass through the monitor if NOT consumed by a derived class.
        /// </summary>
        public Action<string> LogMonitor { get; set; }  

        MonitorBase(params MonitorFilter[] filters)
        {
            Filters = new MonitorFilter[0];
            AddFilters(filters);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">The client to connect to the chat room.</param>
        /// <param name="filters">Filters to add derived from MonitorFilters.</param>
        public MonitorBase(Client client, params MonitorFilter[] filters)
            : this(filters)
        {
            m_Client = client;
            m_Reader = client.reader;
            IsInitialized = m_Client != null && m_Reader != null;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader">Reader to read the IRC stream.</param>
        /// <param name="filters">Filters to add derived from MonitorFilters.</param>
        public MonitorBase(StreamReader reader, params MonitorFilter[] filters)
            : this(filters)
        {
            m_Reader = reader;
            IsInitialized = reader != null;
        }

        /// <summary>
        /// Add a set of important filters to the monitor.
        /// </summary>
        /// <param name="error">Error filter to add to the monitor.</param>
        /// <param name="notice">Notice filter to add to the monitor.</param>
        /// <param name="ping">Ping filter to add to the monitor.</param>
        /// <returns>The Monitor for chain filter additions.</returns>
        public MonitorBase ImportantFilters(MonitorFilter error = null, MonitorFilter notice = null, MonitorFilter ping = null)
        {
            return AddFilters(error ?? MonitorFilter.Empty, notice ?? MonitorFilter.Empty, ping ?? MonitorFilter.Empty);
        }

        /// <summary>
        /// Adds filters to filter the incoming message.
        /// </summary>
        /// <param name="filters">Filters to add derived from MonitorFilters.</param>
        /// <returns>The Monitor for chain filter additions.</returns>
        public MonitorBase AddFilters(params MonitorFilter[] filters)
        {
            if (filters.Length == 0) return this;
            Filters = Filters.Concat(filters.Where(x => x != MonitorFilter.Empty)).Distinct().ToArray();
            return this;
        }
        
        /// <summary>
        /// Removes filters to filter the incoming message.
        /// </summary>
        /// <param name="filters">Filters to remove derived from MonitorFilters.</param>
        /// <returns>The Monitor for chain filter additions.</returns>
        public MonitorBase RemoveFilters(params MonitorFilter[] filters)
        {
            foreach (MonitorFilter filter in filters)
            {
                filter.RemoveAll();
            }
            Filters = Filters.Except(filters).ToArray();
            
            return this;
        }

        /// <summary>
        /// The parser for the stream information.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        /// <returns>List of the data parsed for the filters.</returns>
        protected abstract IList<MonitorArgs> Parse(string message);
        IList<MonitorArgs> IMonitorBase.Parse(string message)
        {
            return Parse(message);
        }

        /// <summary>
        /// Reads a line for the IRC stream.
        /// </summary>
        /// <returns>Returns the IRC line read.</returns>
        protected string ReadLine()
        {
            string result = string.Empty;
            try
            {
                if(m_Client != null)
                {
                    if(m_Client.tcpClient.Available > 0 || m_Reader.Peek() >= 0)
                    {
                        result = m_Reader.ReadLine();
                    }
                }
                else if(m_Reader.Peek() >= 0)
                {
                    result = m_Reader.ReadLine();
                }
            }
            catch(ThreadAbortException tex)
            {
                Console.WriteLine("[[MONITOR BASE]] {0}", tex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
        string IMonitorBase.ReadLine()
        {
            return ReadLine();
        }

        /// <summary>
        /// Try to read the message for the IRC stream.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        /// <returns>True if there was a message on the stream, False if not.</returns>
        protected bool TryGet(out string message)
        {
            return !string.IsNullOrEmpty(message = ReadLine());
        }
        bool IMonitorBase.TryGet(out string message)
        {
            return TryGet(out message);
        }

        /// <summary>
        /// Invoke all subscribed events for data received.
        /// </summary>
        /// <param name="monitorArgs">Data to send for</param>
        protected virtual void InvokeRecieved(MonitorArgs monitorArgs)
        {
            if (OnReceived != null)
            {
                OnReceived(this, monitorArgs);
            }
            if (onReceived != null)
            {
                onReceived(this, monitorArgs);
            }
        }
        void IMonitorBase.InvokeRecieved(MonitorArgs monitorArgs)
        {
            InvokeRecieved(monitorArgs);
        }

        /// <summary>
        /// The monitor is initialized when both streams are not null.
        /// </summary>
        public bool IsInitialized { get; protected set; }
        bool IMonitor.IsInitialized
        {
            get { return IsInitialized; }
            set { IsInitialized = value; }
        }

        /// <summary>
        /// The call to monitor the incoming IRC stream.
        /// </summary>
        public virtual void Monitor()
        {
            string message;
            if (TryGet(out message))
            {
                if (!ConsumeMessage(message))
                {
                    if (LogMonitor != null)
                    {
                        LogMonitor.Invoke(message);
                    }
                    foreach (var args in Parse(message))
                    {
                        InvokeRecieved(args);
                    }
                }
            }
        }
        void IMonitor.Monitor()
        {
            Monitor();
        }

        /// <summary>
        /// Consume/use the message before it is sent for parsing and matching by the filters.
        /// </summary>
        /// <param name="message">The incoming message.</param>
        /// <returns>True if the message was consumed, False if the message should be parsed and matched.</returns>
        protected virtual bool ConsumeMessage(string message) { return false; }
    }
}
