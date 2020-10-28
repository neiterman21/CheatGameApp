using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheatGameModel.Network.Messages;
using System.Net.Sockets;
using System.Xml;
using System.IO;
using System.Threading;
using CheatGameApp;

namespace CheatGameModel.Network
{
    public abstract class TcpConnectionBase : IDisposable
    {
        
        public abstract void SetIPEndPoints(string serverIPEndPoint, string ClientIPEndPoint);

        public event EventHandler Started;
        public static int conectionnumber = 0;
        protected void RaiseStarted()
        {
            IsStarted = true;
            if (Started == null)
                return;
            Started(this, new EventArgs());
        }

        public event EventHandler<MessageEventArg> MessageReceived;
        protected void RaiseMessageReceived(Message message)
        {
            if (MessageReceived == null)
                return;
            //Console.Write("raising event for con num= " + conectionnumber + "\n");
            MessageReceived(this, new MessageEventArg(message));
        }
        

        protected Socket m_socket = null;
        protected byte[] m_buffer = new byte[4096];
        protected List<byte> m_messageBuffer = new List<byte>();
        //protected StringBuilder m_messageSB = new StringBuilder();

        public bool IsStarted { get; protected set; }

        
        protected void BeginReceive()
        {
          try
          {
            if (m_socket.Connected)
              m_socket.BeginReceive(m_buffer, 0, m_buffer.Length, SocketFlags.None, new AsyncCallback(AcceptBytes), null);
          }
          catch (Exception ex)
          {
            string msg = "socket Error.  Error: " + ex.Message;
            System.Diagnostics.Debug.WriteLine(msg);
          }
    }

        private int IndexOf(byte[] array, byte[] value)
        {
            if (array.Length < value.Length)
                return -1;

            int valueLength = value.Length;
            int length = array.Length - valueLength + 1;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < valueLength; j++)
                {
                    if (array[i + j] != value[j])
                        break;

                    //found it
                    if (j == valueLength - 1)
                        return i;
                }

            }
            return -1;
        }
        private void AcceptBytes(IAsyncResult result)
        {
            try
            {
        
                int count = m_socket.EndReceive(result);           
                byte[] incomingMsg = new byte[count];
                Buffer.BlockCopy(m_buffer, 0, incomingMsg, 0, count);

                if (m_messageBuffer.Count > 0)
                {
                    
                    int copyLength = Math.Min(Message.EOM.Length - 1, m_messageBuffer.Count);
                    byte[] temp = new byte[incomingMsg.Length + copyLength];

                    for (int i = copyLength; i > 0; --i)
                        temp[copyLength - i] = m_messageBuffer[m_messageBuffer.Count - i];

                    //append posible EOM part from buffer
                    Buffer.BlockCopy(incomingMsg, 0, temp, copyLength, incomingMsg.Length);

                    //remove posible EOM part from buffer
                    m_messageBuffer.RemoveRange(m_messageBuffer.Count - copyLength, copyLength);

                    incomingMsg = temp;
                }
                int index = IndexOf(incomingMsg, Message.EOM);

                while (index >= 0)
                {
                    byte[] incomingMsgEndPart = new byte[index];
                    Buffer.BlockCopy(incomingMsg, 0, incomingMsgEndPart, 0, index);

                    byte[] incomingMsgStartPart = new byte[incomingMsg.Length - index - Message.EOM.Length];
                    if (incomingMsgStartPart.Length > 0)
                        Buffer.BlockCopy(incomingMsg, index + Message.EOM.Length, incomingMsgStartPart, 0, incomingMsgStartPart.Length);

                    //add untill the end of message
                    m_messageBuffer.AddRange(incomingMsgEndPart);

                    try
                    {
                        string messageString = UTF8Encoding.UTF8.GetString(m_messageBuffer.ToArray());

                        //read values by name
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(messageString);
                        //find message type
                        string msgTypeName = typeof(Message).AssemblyQualifiedName.Replace(".Message,", "." + xml.DocumentElement.GetAttribute("Type") + ",");
                        Type msgType = Type.GetType(msgTypeName);
                        Message message = Activator.CreateInstance(msgType, xml) as Message;

                        RaiseMessageReceived(message);
                    }
                    catch (Exception ex)
                    {
                        string msg = "Error in parsing message. Error: " + ex.Message;
                        System.Diagnostics.Debug.WriteLine(msg);
                    }

                    //start a new message
                    m_messageBuffer.Clear();

                    incomingMsg = incomingMsgStartPart;

                    //there is more data in incoming message then was appended to message buffer
                    index = IndexOf(incomingMsg, Message.EOM);
                };

                if (incomingMsg.Length > 0)
                {
                    m_messageBuffer.AddRange(incomingMsg);
                }

                BeginReceive();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in AcceptBytes(). Error: " + ex.Message);
            }
        }
       

        public abstract IAsyncResult BeginStart();
        public void BeginSend(Message message)
        {
            System.Threading.Tasks.Task.Factory.StartNew(new Action(() => Send(message)));
        }
        public void Send(Message message)
        {
            //get message bytes
           // Console.WriteLine("sending ");
            byte[] messageBytes = message.GetBytes();

            //framed bytes
            byte[] paddedMessageBytes = new byte[messageBytes.Length + Message.EOM.Length];

            //copy message bytes
            Buffer.BlockCopy(messageBytes, 0, paddedMessageBytes, 0, messageBytes.Length);

            //pad the message
            Buffer.BlockCopy(Message.EOM, 0, paddedMessageBytes, messageBytes.Length, Message.EOM.Length);

            //send padded message
            if (m_socket != null)
                if (m_socket.Connected)
                {
                    //File.AppendAllText("C:\\Liar\\log.txt", "before send " + message.Type + Environment.NewLine);
                    m_socket.BeginSend(paddedMessageBytes, 0, paddedMessageBytes.Length, SocketFlags.None, null, null);
                    //Console.WriteLine("sending " + paddedMessageBytes.Length);
                    //File.AppendAllText("C:\\Liar\\log.txt", "after send " + message.Type + Environment.NewLine);
                } 
        }

        public void Send()
        {
          byte[] tmp = new byte[1];
          try
          {
            m_socket.Send(tmp, 0, 0);
          }
          catch (Exception e)
          {

          }
         
        }
        public abstract void Dispose();
    }
}
