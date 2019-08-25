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

      if (CopyButton.Visible)
      {
        Size s = new Size(CodeLable.Size.Width + CopyButton.Size.Width + 100, this.Size.Height);
        this.Size = s;
      }
    }

    private const int CP_NOCLOSE_BUTTON = 0x200;
    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams myCp = base.CreateParams;
        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        return myCp;
      }
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
