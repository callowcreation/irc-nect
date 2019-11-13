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
using System.Net.Sockets;

namespace IRCnect.Connection
{
    /// <summary>
    /// Client holds reference to the TcpClient used to connect to the IRC server/host
    /// </summary>
    public class Client : IDisposable
    {
        NetworkStream m_Stream;
        /// <summary>
        /// IRC host name that will be connected to.
        /// </summary>
        public string hostname { get; private set; }
        /// <summary>
        /// Port to use to provide a gateway for connection.
        /// </summary>
        public int port { get; private set; }
        /// <summary>
        /// Stream reader to read IRC communication stream.
        /// </summary>
        public StreamReader reader { get; private set; }
        /// <summary>
        /// Stream writer to write to the IRC communication stream.
        /// </summary>
        public StreamWriter writer { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public TcpClient tcpClient { get; private set; } = null;

        /// <summary>
        /// Constructor
        /// <para>Initialization is done in the OpenConnection() function call.</para>
        /// </summary>
        public Client()
        { }

        /// <summary>
        /// Initializes and opens the TcpClient connection.
        /// </summary>
        /// <param name="hostname">IRC host name that will be connected to.</param>
        /// <param name="port">Port to use to provide a gateway for connection.</param>
        public virtual void OpenConnection(string hostname, int port)
        {
            if (tcpClient != null)
            {
                //return;
                throw new InvalidOperationException("The connection is opened and must be closed before it can be reopened.  To close the connection use Client.CloseConnection().");
            }
            tcpClient = new TcpClient(hostname, port);
            m_Stream = tcpClient.GetStream();

            this.hostname = hostname;
            this.port = port;
            this.reader = new StreamReader(m_Stream);
            this.writer = new StreamWriter(m_Stream);
        }

        /// <summary>
        /// Closes the client connection stream.
        /// </summary>
        public void CloseConnection()
        {
            if (tcpClient == null) return;
            m_Stream.Close();
            tcpClient.Close();
            tcpClient = null;
        }

        /// <summary>
        /// Closes connection and stream.
        /// <para>IDisposable reqirements, user shold never have to call this method. Use CloseConnection() instead.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        void Dispose(bool flag)
        {
            CloseConnection();
        }
    }

}
