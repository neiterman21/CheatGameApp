using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ComponentModel;

namespace CheatGameModel.Network.Messages
{
    public abstract class Message
    {
        //public const string SOM = "<Message ";
        //public const string EOM = " />";
        public static readonly byte[] EOM = new byte[] { 4, 4, 4, 4, 4, 4 };
        public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        protected XmlDocument m_xml;

        public string Type { get; private set; }

        public Message(XmlDocument xml)
        {
            m_xml = xml;
            LoadProperties(this);
        }
        public Message()
        {
            Type = GetType().Name;
            //init xml
            m_xml = new XmlDocument();
            XmlElement root = m_xml.CreateElement("Message");
            m_xml.AppendChild(root);
        }

        protected string GetStringValue(string name)
        {
            return m_xml.DocumentElement.GetAttribute(name);
        }
        protected DateTime GetDateTimeValue(string name)
        {
            string s = GetStringValue(name);
            return DateTime.ParseExact(s, Message.DateTimeFormat, null);
        }
        protected T GetValue<T>(string name)
        {
            return (T)GetValue(name, typeof(T));
        }
        protected object GetValue(string name, Type type)
        {
            if (type == typeof(DateTime))
                return GetDateTimeValue(name);

            string s = GetStringValue(name);
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            object value = converter.ConvertFromString(s);
            return value;
        }

        protected void Append(string name, string value)
        {
            m_xml.DocumentElement.SetAttribute(name, value);
        }
        protected void Append(string name, object value)
        {
            if (value is DateTime)
            {
                Append(name, (DateTime)value);
                return;
            }

            TypeConverter converter = TypeDescriptor.GetConverter(value.GetType());
            string s = converter.ConvertToString(value);
            Append(name, s);
        }
        protected void Append(string name, DateTime value)
        {
            Append(name, value.ToString(Message.DateTimeFormat));
        }
        protected void AppendProperties(object item)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            foreach (PropertyDescriptor property in properties)
            {
                object value = property.GetValue(item);
                this.Append(property.Name, value);
            }
        }
        protected void LoadProperties(object item)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
            foreach (PropertyDescriptor property in properties)
            {
                object value = this.GetValue(property.Name, property.PropertyType);
                property.SetValue(item, value);
            }
        }
        protected virtual void AppendProperties()
        {
            m_xml.DocumentElement.RemoveAllAttributes();
            AppendProperties(this);
        }
        public byte[] GetBytes()
        {
            AppendProperties();
            return UTF8Encoding.UTF8.GetBytes(m_xml.OuterXml);
        }
    }
}
