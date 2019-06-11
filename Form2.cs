using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CheatGameApp
{
    public partial class Form2 : Form
    {
        WaveStream my_record_strem;
        public bool need_to_report = false;
        public Form2(WaveStream _record_strem, DeckLabel lastClaimDeckLabel )
        {
            InitializeComponent();
            my_claim.Deck = lastClaimDeckLabel.Deck;
            my_record_strem = _record_strem;
            
        }
        protected void OnPlaybackStopped(object obj, StoppedEventArgs e)
        {
            Playrecording(my_record_strem);
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

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void close_Button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void repport_button_Click(object sender, EventArgs e)
        {
            need_to_report = true;
            DialogResult = System.Windows.Forms.DialogResult.Yes;
        }

        public bool GetDispute()
        {
            return need_to_report;
        }
    }
}
