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
using System.IO;

namespace IRCnect.Utils
{
    /// <summary>
    /// General logger utility.
    /// <para>Logs to various outputs file, memory and console.</para>
    /// </summary>
    public class Logger
    {

        #region Master Logger
        /// <summary>
        /// Name for master IRCnect log file
        /// </summary>
        public const string MASTER_LOG_FILE = "ircnect-master-logger-log.txt";
        /// <summary>
        /// Turns all log tracking on/off
        /// </summary>
        public static bool AllLogTracking = false;

        static void TrackAllLogs(string loggerName, string value, Action<string> method)
        {
            method.Invoke(value);
            if (AllLogTracking)
            {
                FileLog('*', string.Format("{0} {1}\t{2}", loggerName, DateTime.Now, value), MASTER_LOG_FILE);
            }
        }

        #endregion

        #region Memory Logger

        static List<string> m_MemoryLogger;

        /// <summary>
        /// Sets the custom callback for the Customlogger.
        /// </summary>
        /// <param name="memoryLogger">Memory structure to lag data.</param>
        public static void SetMemoryLogger(List<string> memoryLogger)
        {
            m_MemoryLogger = memoryLogger;
        }

        /// <summary>
        /// M Memory, logs to memory.
        /// <para>NOTE: Recommended to call FlushM() to not lose logged data.</para>
        /// </summary>
        /// <param name="value">A System.String to log.</param>
        public static void M(string value)
        {
            if (m_MemoryLogger != null)
            {
                m_MemoryLogger.Add(value);
            }
            else
            {
                throw new NullReferenceException("MemoryLogger action can not be null set.  SetMemoryLogger to an List<string>() to use this method.");
            }
        }

        /// <summary>
        /// M Memory, logs to memory.
        /// <para>NOTE: Recommended to call FlushM() to not lose logged data.</para>
        /// </summary>
        /// <param name="format">A composite format string/</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void M(string format, params object[] args)
        {
            Logger.M(string.Format(format, args));
        }

        /// <summary>
        /// How to output the memory logger's data.
        /// </summary>
        /// <param name="how">Method that will persist the memory data.</param>
        public static void FlushM(Action<IEnumerable<string>> how)
        {
            how.Invoke(m_MemoryLogger);
        } 

        #endregion

        #region Custom Logger

        static Action<string> m_CustomLogger;
        /// <summary>
        /// Sets the custom callback for the Customlogger.
        /// </summary>
        /// <param name="customLogger">Method to invoke when logging data.</param>
        public static void SetCustomLogger(Action<string> customLogger)
        {
            m_CustomLogger = customLogger;
        }

        /// <summary>
        /// C Custom, logs through custom function call.
        /// </summary>
        /// <param name="value">A System.String to log.</param>
        public static void C(string value)
        {
            if (m_CustomLogger != null)
            {
                Logger.TrackAllLogs("C", value, (val) => m_CustomLogger.Invoke(val));
            }
            else
            {
                throw new NullReferenceException("CustomLogger action can not be null set.  SetCustomLogger to an Action<string>() to use this method.");
            }
        }

        /// <summary>
        /// C Custom, logs through custom function call.
        /// </summary>
        /// <param name="format">A composite format string/</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void C(string format, params object[] args)
        {
            Logger.C(string.Format(format, args));
        } 

        #endregion

        #region To Console Logger

        /// <summary>
        /// L Log Console, logs input to the Console
        /// </summary>
        /// <param name="value">A System.String to log.</param>
        public static void L(string value)
        {
            Logger.TrackAllLogs("L", value, (val) => Console.WriteLine(val));
        }

        /// <summary>
        /// L Log Console, logs input to the Console.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void L(string format, params object[] args)
        {
            Logger.L(string.Format(format, args));
        } 

        #endregion

        #region To File Logger

        static string m_FileName = "logger-ircnect-log.txt";
        /// <summary>
        /// Sets the filename for F Log to File.
        /// </summary>
        /// <param name="fileName">Filename of where to log the data.</param>
        public static void SetFileName(string fileName)
        {
            m_FileName = fileName;
        }

        /// <summary>
        /// F Log File, logs input to a file.
        /// </summary>
        /// <param name="value">A System.String to log.</param>
        public static void F(string value)
        {
            Logger.F(m_FileName, value);
        }

        /// <summary>
        /// F Log File, logs input to a file.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void F(string format, params object[] args)
        {
            Logger.F(m_FileName, string.Format(format, args));
        }

        /// <summary>
        /// F Log File, logs input to a file.
        /// </summary>
        /// <param name="fileName">Filename to log the data.</param>
        /// <param name="value">A System.String to log.</param>
        public static void F(string fileName, string value)
        {
            Logger.TrackAllLogs("F", value, (val) => FileLog('F', val, fileName));
        }

        /// <summary>
        /// F Log File, logs input to a file.
        /// </summary>
        /// <param name="fileName">Filename to log the data.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static void F(string fileName, string format, params object[] args)
        {
            Logger.F(fileName, string.Format(format, args));
        }

        static void FileLog(char key, string value, string fileName)
        {
            FlushToLog(key, new string[] { value }, fileName);
        }

        /// <summary>
        /// Writes values to a file.
        /// </summary>
        /// <param name="key">Log function key one of {'F', 'L', 'C', '*'} * is reserved for the master file identifer.</param>
        /// <param name="values">Values to los to file.</param>
        /// <param name="fileName">Name opf file to store information.</param>
        public static void FlushToLog(char key, IEnumerable<string> values, string fileName)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                foreach (var value in values)
                {
                    sw.WriteLine(value);
                }
            }
        }

        #endregion

    }
}
