using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CheatGameApp
{
  public partial class soundTestForm : Form
  {
    // Define the output wav file of the recorded audio
    private bool stage = false;
    public bool testPassed = false;

    public WaveIn waveSource = null;
    public WaveFileWriter waveFile = null;
    MemoryStream ws = null;
    ManualResetEvent isRecordingEvent = new ManualResetEvent(false);
    System.Windows.Forms.Timer audioRecordTimer = new System.Windows.Forms.Timer();
    public soundTestForm()
    {
      InitializeComponent();
      this.KeyPreview = true;
      audioRecordTimer.Interval = 4000;
      audioRecordTimer.Tick += new EventHandler(audioRecordTimer_Tick);
    }

    public bool isTestPassed()
    {
      return testPassed;
    }
    protected void OnPlaybackStopped(object obj, StoppedEventArgs e)
    {
    }
    protected void Playrecording(WaveStream record_strem)
    {
      using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
      {
        record_strem.Position = 0;
        waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(OnPlaybackStopped);
        waveOut.Init(record_strem);
        waveOut.Play();
        while (waveOut.PlaybackState == PlaybackState.Playing)
        {
          System.Threading.Thread.Sleep(100);
        }
      }
    }
    void PlayWellcomeMessage()
    {
      using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
      {
        WaveStream WelcomeRecording = new RawSourceWaveStream(CheatGameApp.Properties.Resources.Welcome1, new WaveFormat(48000, 2));
        WelcomeRecording.Position = 0;
        waveOut.PlaybackStopped += new EventHandler<StoppedEventArgs>(OnPlaybackStopped);
        waveOut.Init(WelcomeRecording);
        waveOut.Play();
        while (waveOut.PlaybackState == PlaybackState.Playing)
        {
          System.Threading.Thread.Sleep(500);
        }
        this.YesButton.Visible = true;
        this.NoButton.Visible = true;
      }
    }

    public WaveStream CaptureAudio()
    {
      //Console.WriteLine("Now recording...");
      
      waveSource = new WaveIn();
      waveSource.WaveFormat = new WaveFormat(16000, 1);

      waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
      waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

      ws = new MemoryStream();
      waveFile = new WaveFileWriter(new IgnoreDisposeStream(ws), waveSource.WaveFormat);

      Thread recordingThread = new Thread(waveSource.StartRecording); 
      recordingThread.Start();
      recordingLable.Visible = true;
      audioRecordTimer.Start();
      audioRecordTimer.Enabled = true;
      while (!isRecordingEvent.WaitOne(200))
      {
        // NAudio requires the windows message pump to be operational
        // this works but you better raise an event
        Application.DoEvents();
      }
      recordingLable.Visible = false;
      return new RawSourceWaveStream(ws, new WaveFormat(16000, 1));
    }

    void audioRecordTimer_Tick(object sender, EventArgs e)
    {
      waveSource.StopRecording();
      waveSource.Dispose();
      waveSource = null;
      //waveFile.Close();
      //waveFile = null;


      //disable the timer here so it won't fire again...
      audioRecordTimer.Enabled = false;
      isRecordingEvent.Set();
    }

    void waveSource_DataAvailable(object sender, WaveInEventArgs e)
    {
      if (waveFile != null)
      {
        waveFile.Write(e.Buffer, 0, e.BytesRecorded);
        waveFile.Flush();
      }
    }

    void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
    {
      if (waveSource != null)
      {
        waveSource.Dispose();
        waveSource = null;
      }

      if (waveFile != null)
      {
        waveFile.Dispose();
        waveFile = null;
      }
    }

    void RecordAndPlay()
    {
      WaveStream ws = CaptureAudio();
      isRecordingEvent.Reset();
      Playrecording(ws);

      this.YesButton.Visible = true;
      this.NoButton.Visible = true;
    }
    private void Actionbutton_Click(object sender, EventArgs e)
    {
      if (!stage) PlayWellcomeMessage();
      else RecordAndPlay();
    }
    void moveToMicrophonCheck()
    {
      this.Actionbutton.Text = "start Recording";
      this.growLabel1.Text = "Now record yourself to check the microphone is conected.";
      this.questionLable.Text = "Did you hear yourself?";
      stage = true;
      this.YesButton.Visible = false;
      this.NoButton.Visible = false;
    }
    private void YesButton_Click(object sender, EventArgs e)
    {
      if (stage == true)
      {
        testPassed = true;
        DialogResult = System.Windows.Forms.DialogResult.OK;
      }
      moveToMicrophonCheck();
    }

    private void NoButton_Click(object sender, EventArgs e)
    {
      if (!stage) PlayWellcomeMessage();
      else RecordAndPlay();
    }
  }
}
