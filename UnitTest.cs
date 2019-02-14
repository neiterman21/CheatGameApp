using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NAudio.Wave;

namespace CheatGameApp
{
    public partial class UnitTest : Form
    {
        public UnitTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            Form1 test = new Form1();
            WaveStream ws = test.CaptureAudio();
            WaveFileWriter.CreateWaveFile(@"C: \Users\neite\OneDrive\Desktop\recordings\unittest2.wav", ws);
            using (WaveOut waveOut = new WaveOut(WaveCallbackInfo.FunctionCallback()))
            {
                waveOut.Init(ws);
                waveOut.Play();
                while (waveOut.PlaybackState == PlaybackState.Playing)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
