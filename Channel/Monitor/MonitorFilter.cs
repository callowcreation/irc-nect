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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace IRCnect.Channel.Monitor
{
    /// <summary>
    /// Abstract class providing basic filter functionality for messages.
    /// </summary>
    public abstract class MonitorFilter
    {
        class EmptyMonitorFilter : MonitorFilter
        {
            public override MonitorArgs Parse(string message)
            {
                return MonitorArgs.Empty;
            }
        }
        /// <summary>
        /// Empty Monitor filter to use instead of null.
        /// </summary>
        public static readonly MonitorFilter Empty = new EmptyMonitorFilter();

        /// <summary>
        /// Pattern to match against all inputs.
        /// </summary>
        protected const string PATTERN_ALL = "(.*)";
        /// <summary>
        /// Filters for matching used in derived classes.
        /// </summary>
        protected Dictionary<Regex, Action<MonitorArgs>> m_Filters;

        /// <summary>
        /// Raw Input provided for message matching.
        /// </summary>
        protected Dictionary<Regex, string> m_RawInput;

        /// <summary>
        /// Regular expressions used for initial filter match
        /// </summary>
        protected Regex m_Regex;

        /// <summary>
        /// Regular expressions used for initial filter match
        /// </summary>
        public Regex regex { get { return m_Regex; } }

        /// <summary>
        /// Raw Input provided for message matching.
        /// </summary>
        public Dictionary<Regex, string> rawInput { get { return m_RawInput; } }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="pattern">Regular expressions pattern to match against input.</param>
        public MonitorFilter(string pattern = PATTERN_ALL)
        {
            m_Regex = new Regex(pattern);
            m_Filters = new Dictionary<Regex, Action<MonitorArgs>>();
            m_RawInput = new Dictionary<Regex, string>();
        }
        
        /// <summary>
        /// Adds filters to look for in incoming messages.
        /// </summary>
        /// <param name="format">Format for compiling the regex.</param>
        /// <param name="item">String item to match against the pattern.</param>
        /// <param name="regexOptions">Any options to modify matching.</param>
        /// <param name="callbacks">Callbacks to invoke upon filtered.</param>
        /// <returns>MonitorFilter type to chain filter additions.</returns>
        public MonitorFilter AddFilter(string format, string item, RegexOptions regexOptions, params Action<MonitorArgs>[] callbacks)
        {
            foreach (var callback in callbacks)
            {
                Regex rx = new Regex(string.Format(format, item), regexOptions);
                m_RawInput.Add(rx, item);
                m_Filters.Add(rx, callback);
            }
            return this;
        }

        /// <summary>
        /// Removes a filter from incoming messages.
        /// </summary>
        /// <param name="key">Regex key to remove filter</param>
        /// <returns>MonitorFilter type to chain filter additions.</returns>
        public MonitorFilter RemoveFilter(Regex key)
        {
            if (m_RawInput.TryGetValue(key, out string valueString))
            {
               m_RawInput.Remove(key);
            }
            
            if (m_Filters.TryGetValue(key, out Action <MonitorArgs> valueAction))
            {
                m_Filters.Remove(key);
            }
            return this;
        }

        /// <summary>
        /// Removes all regex fiters
        /// </summary>
        public void RemoveAll()
        {
            List<Regex> keys = m_RawInput.Keys.ToList();
            for (int j = 0; j < keys.Count; j++)
            {
                RemoveFilter(keys[j]);
            }
        }

        /// <summary>
        /// Gets a filter from the collection based on the key
        /// </summary>
        /// <param name="regex">Filters collection key</param>
        /// <returns>Action callbacks</returns>
        public Action<MonitorArgs> GetFilterCallbacks(Regex regex)
        {
            return m_Filters[regex];
        }

        /// <summary>
        /// Abstract mothod to parse messages for detailed data.
        /// </summary>
        /// <param name="message">Message to be parsed.</param>
        /// <returns>Parsed message container as Event args.</returns>
        public abstract MonitorArgs Parse(string message);
    }
}
