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
using System.IO;

namespace IRCnect.Utils
{
    /// <summary>
    /// Helper class to write to a stream and flush the stream after writing all lines.
    /// </summary>
    public class StreamIRC
    {
        /// <summary>
        /// Writes unformatted text to the chat stream
        /// <para>Use a method prefixed with Send to send messages and commands.</para>
        /// </summary>
        /// <param name="stream">StreamWriter to write to</param>
        /// <param name="lines">Lines to write to stream</param>
        /// <exception cref="System.ApplicationException">Thrown when writer fails.</exception>
        public static void WriteAllLines(StreamWriter stream, params string[] lines)
        {
            try
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    stream.WriteLine(line);
                }
                stream.Flush();
            }
            catch (Exception ex)
            {
                //throw new ApplicationException("Unknown exception occured in Stream.WriteAllLines()", ex);
                Console.WriteLine("Unknown exception occured in Stream.WriteAllLines() " +  ex);
            }
        }
    }
}
