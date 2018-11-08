using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheatGameApp
{
    public partial class WaitForm : Form
    {
        public string WaitMessage { get { return label1.Text; } set { label1.Text = value; } }
        public WaitForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                string title = "Exit";
                string text = "Are you sure you want to exit the game?" + Environment.NewLine + "Any data that was unsaved will be lost.";
                if (DialogResult.Yes == MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Application.Exit();
            }
        }
    }
}
