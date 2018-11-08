using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CheatGameModel.Network.Messages
{
    public sealed class MessageEventArg : EventArgs
    {
        public Message Message { get; private set; }
        public MessageEventArg(Message msg)
        {
            this.Message = msg;
        }
    }
}
