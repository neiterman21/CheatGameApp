// Decompiled with JetBrains decompiler
// Type: CentipedeModel.Network.Messages.JpgMessage
// Assembly: LiarServerApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D9C86562-18F8-4555-90FE-AA8F248B8776
// Assembly location: C:\Users\neite\OneDrive\Documents\לימודים\Server\LiarServerApp.exe

using CheatGameModel.Network.Messages;
using NAudio.Wave;
using System;
using System.Drawing;
using System.IO;
using System.Xml;

namespace CheatGameModel.Network.Messages
{
  public class AudioMessage : Message
  {
    private WaveStream ms;
    

    public string Value { get; set; }

    public AudioMessage(XmlDocument xml)
      : base(xml)
    {
      byte[] raw = Convert.FromBase64String(xml.DocumentElement.GetAttribute("Value"));
      ms = new RawSourceWaveStream(raw, 0, raw.Length, new WaveFormat(16000, 1));
    }

    public AudioMessage(WaveStream recording_)
    {
      this.ms = recording_;
    }

    public WaveStream GetRecording()
    {
      return this.ms;
    }

    protected override void AppendProperties()
    {

      byte[] raw = new byte[ms.Length];
      ms.Read(raw , 0 , raw.Length);
      this.Value = Convert.ToBase64String(raw, 0, (int)ms.Length);       
      base.AppendProperties();
    }
  }
}
