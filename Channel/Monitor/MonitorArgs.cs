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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor
{
    /// <summary>
    /// Args used by filters to store parsed data.
    /// </summary>
    [Serializable]
    public abstract class MonitorArgs : EventArgs
    {
        class EmptyMonitorArgs : MonitorArgs
        {
            public override void MatchFilters(Dictionary<Regex, Action<MonitorArgs>> filters)
            {
                //Not implemented empty args;
            }
        }

        /// <summary>
        /// Empty Monitor args to used instead of null.
        /// </summary>
        new public readonly static MonitorArgs Empty = new EmptyMonitorArgs(); 

        string m_Data = null;

        /// <summary>
        /// Data passes in before processing (usually raw data)
        /// </summary>
        public string data { get { return m_Data; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">Data passes in before prcessing (usually raw data)</param>
        public MonitorArgs(string data)
            : base()
        {
            m_Data = data;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public MonitorArgs()
            : this(string.Empty)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="obj">Data passes in before processing (usually raw data)
        /// <para>Uses objects ToString() method to get value.</para></param>
        public MonitorArgs(object obj)
            : this(obj.ToString())
        { }

        /// <summary>
        /// Compares data to filters for matches of specific data.
        /// </summary>
        /// <param name="filters">Filters to match data against.</param>
        public abstract void MatchFilters(Dictionary<Regex, Action<MonitorArgs>> filters);

    }

}
