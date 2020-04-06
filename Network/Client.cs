using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace CheatGameModel.Network
{
    public sealed class Client : TcpConnectionBase
    {
        private IPEndPoint m_serverIPEndPoint;
        private IPEndPoint m_clientIPEndPoint;
        private TcpClient m_tcpClient;

        //public Client()
        //{
        //    m_clientIPEndPoint = IPEndPointParser.Parse("132.70.6.21:54321");
        //    m_serverIPEndPoint = IPEndPointParser.Parse("127.0.0.1:54321");
        //    m_tcpClient = new TcpClient(m_clientIPEndPoint);
        //}

        public override void SetIPEndPoints(string ServerIPEndPoint, string ClientIPEndPoint)
        {
            m_serverIPEndPoint = IPEndPointParser.Parse(ServerIPEndPoint);
            if (ClientIPEndPoint != null)
                m_clientIPEndPoint = IPEndPointParser.Parse(ClientIPEndPoint);
        }
        public override void Dispose()
        {
            m_tcpClient.Close();
            if(m_socket != null)
            m_socket.Close();
            //m_tcpClient.Client.Dispose();            
        }
        private void OnConnect(IAsyncResult result)
        {
            m_socket = m_tcpClient.Client;
            BeginReceive();
            RaiseStarted();
        }

        public void BeginStart1()
        {
            m_tcpClient = new TcpClient();
            m_tcpClient.Connect(m_serverIPEndPoint.Address, m_serverIPEndPoint.Port);
            m_socket = m_tcpClient.Client;
            m_socket.NoDelay = true;
            BeginReceive();
            RaiseStarted();
            conectionnumber++;
        }

        public override IAsyncResult BeginStart()
        {
            m_tcpClient = new TcpClient(m_clientIPEndPoint);

            return m_tcpClient.BeginConnect(m_serverIPEndPoint.Address, m_serverIPEndPoint.Port, new AsyncCallback(OnConnect), null);
        }
    }
}
