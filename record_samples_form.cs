using CheatGameApp.Model;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CheatGameApp
{
    public partial class record_samples_form : Form
    {
        public String Record_dir = "Records";
        
        public record_samples_form()
        {
            InitializeComponent();
        }

        public String selectdFileName()
        {
            if (card_count.SelectedIndex < 0 || card_value.SelectedIndex < 0) return "";
            String f = card_count.Items[card_count.SelectedIndex].ToString() + "_" + card_value.Items[card_value.SelectedIndex].ToString() + ".wav";
            return Path.Combine(Record_dir, f);
        }

        private void RecordButton_Click(object sender, EventArgs e)
        {
            soundTestForm helper = new soundTestForm();
            WaveStream audio = helper.CaptureAudio();    
            WaveFileWriter.CreateWaveFile(selectdFileName(), audio);

            int tmp = card_value.SelectedIndex;
            tmp++;
            tmp %= card_value.Items.Count;
            card_value.SelectedIndex = tmp; 
            if(card_value.SelectedIndex == 0)
            {
                tmp = card_count.SelectedIndex;
                tmp++;
                tmp %= card_count.Items.Count;
                card_count.SelectedIndex = tmp;
            }
        }

        private void ReplayButton_Click(object sender, EventArgs e)
        {
            soundTestForm helper = new soundTestForm();
            WaveStream record_strem = new WaveFileReader(selectdFileName());
            helper.Playrecording(record_strem);
            record_strem.Dispose();
        }

        private void card_count_SelectedIndexChanged(object sender, EventArgs e)
        {
            Deck claim1 = new Deck();
            for (int i = 0; i < card_count.SelectedIndex + 1; i++)
            {
                claim1.Add(new Card(card_value.SelectedIndex +1, CardType.Heart));
            }

            my_claim.Deck = claim1;
            my_claim.FacingUp = true;
            if (File.Exists( selectdFileName()))
                ReplayButton.Enabled = true;
            else
                ReplayButton.Enabled = false;
        }

        private void record_samples_form_Load(object sender, EventArgs e)
        {
            card_value.SelectedIndex = 0;
            card_count.SelectedIndex = 0;
            my_claim.Visible = true;
            if (!Directory.Exists(Record_dir)) Directory.CreateDirectory(Record_dir);
        }
    }
}
