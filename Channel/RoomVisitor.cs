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
using IRCnect.Utils;
using System.IO;

namespace IRCnect.Channel
{
    /// <summary>
    /// Abstract class providing main functionality to connect to chat channels.
    /// </summary>
    public abstract class RoomVisitor
    {

        /// <summary>
        /// Who is connecting, nick name.
        /// </summary>
        public string nick { get; private set; }
        /// <summary>
        /// Stream writer to write to the IRC comunication stream.
        /// </summary>
        public StreamWriter writer { get; private set; }
        /// <summary>
        /// Real name, recommended to use an alias and NOT a Real name.
        /// </summary>
        public string realName { get; private set; }

        int mode { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="nick">Who is connecting, nick name.</param>
        /// <param name="writer">Stream writer to write to the IRC comunication stream.</param>
        /// <param name="realName">Real name, recommended to use an alias and NOT a Real name.</param>
        /// <param name="mode">Connection mode (Leave alone if you are not sure).</param>
        public RoomVisitor(string nick, StreamWriter writer, string realName = "", int mode = Protocol.MODE)
        {
            this.nick = nick;
            this.writer = writer;
            this.realName = realName;
            this.mode = mode;
        }

        /// <summary>
        /// Connect to the IRC server/host.
        /// </summary>
        /// <param name="pass">Authorization key or password.</param>
        public virtual void Connect(string pass)
        {
            StreamIRC.WriteAllLines(this.writer, Protocol.NickConnectionStrings(nick, pass, mode, realName));
        }

        /// <summary>
        /// Join a chat room.
        /// </summary>
        /// <param name="channel">Channel to join.</param>
        public void JoinRoom(string channel)
        {
            WriteMessage(string.Concat(Protocol.JOIN, channel));
        }

        /// <summary>
        /// Leave a chat room.
        /// </summary>
        /// <param name="channel">Channel room to part.</param>
        public void PartRoom(string channel)
        {
            WriteMessage(string.Concat(Protocol.PART, channel));
        }

        void WriteMessage(string message)
        {
            StreamIRC.WriteAllLines(this.writer, message);
        }
    }
}
