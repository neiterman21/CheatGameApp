using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheatGameModel.Players;
using System.Xml;
using System.ComponentModel;

namespace CheatGameModel.Network.Messages
{
    public sealed class DemographicsMessage : Message
    {
        private Demographics m_demographics;

        public DemographicsMessage(XmlDocument xml)
            : base(xml)
        {
            m_demographics = new Demographics();
            LoadProperties(m_demographics);
        }
        public DemographicsMessage(Demographics demographics)
            : base()
        {
            m_demographics = demographics;
        }

        protected override void AppendProperties()
        {
            base.AppendProperties();
            AppendProperties(m_demographics);
        }

        public Demographics GetDemographics()
        {
            return m_demographics;
        }
    }
}
