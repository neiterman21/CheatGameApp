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

namespace CentipedeModel.Network.Messages
{
  public sealed class AudioMessage : Message
  {
    private WaveStream ms;
    

    public string Value { get; set; }

    public AudioMessage(XmlDocument xml)
      : base(xml)
    {
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
