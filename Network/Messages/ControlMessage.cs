using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheatGameApp;
using System.Xml;
using System.Collections.ObjectModel;

namespace CheatGameModel.Network.Messages
{
    // Server (mostly, see remark ahead) may send the following commands:
    public enum ControlCommandType
    {
        Start = 0,  // 'Start' is also sent by the client to notify pressing the start button
        Tick = 1,
        End = 2,
        Report = 4,
        EndMatch = 5,
        OpponentDisconected =6,
  }

    public sealed class ControlMessage : Message
    {
        public ControlCommandType Commmand { get; set; }
        public string msg { get; set; }

        public ControlMessage(ControlCommandType Commmand)
        : base()
        {
            this.Commmand = Commmand;
            this.msg = string.Empty;
        }

        public ControlMessage(ControlCommandType Commmand , string _msg)
        : base()
        {
            this.Commmand = Commmand;
            this.msg = _msg;
        }

        public ControlMessage(XmlDocument xml)
            : base(xml)
        {
        }



    }
}
