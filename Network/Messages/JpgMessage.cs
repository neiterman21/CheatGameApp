using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace CheatGameModel.Network.Messages
{
    public sealed class JpgMessage : Message
    {
        private Image m_image;

        public string Value { get; set; }

        public JpgMessage(XmlDocument xml)
            : base(xml)
        {
        }
        public JpgMessage(Image image)
        {
            m_image = image;
        }
        protected override void AppendProperties()
        {
            if (string.IsNullOrEmpty(this.Value))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    //save with high qualityu
                    lock (m_image)
                        m_image.Save(ms, JpegEncoding.Codec, JpegEncoding.EncoderParams100);

                    this.Value = Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
                    m_image.Dispose();
                }
            }
            base.AppendProperties();
        }
        private MemoryStream m_memoryStream;
        public Image GetImage()
        {
            if (m_image == null)
            {
                byte[] bytes = Convert.FromBase64String(this.Value);
                m_memoryStream = new MemoryStream(bytes);
                m_image = new Bitmap(m_memoryStream, false);
            }
            return m_image;
        }
        public void DisposeImage()
        {
            if (m_image != null)
                m_image.Dispose();
            if (m_memoryStream != null)
                m_memoryStream.Dispose();

        }
    }
}
