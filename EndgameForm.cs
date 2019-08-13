using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CheatGameApp
{
  public partial class EndgameForm : Form
  {
    public EndgameForm(string msg , string code = "")
    {
      InitializeComponent();
      MessageLable.Text = msg;
      CodeLable.Text = code;
      if (code != "")
      {
        CopyButton.Visible = true;
      }
      else CloseButton.Visible = true;
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Clipboard.SetText(CodeLable.Text);
      CloseButton.Visible = true;
    }

    private void CloseButton_Click(object sender, EventArgs e)
    {
      Close();
    }
  }
}
