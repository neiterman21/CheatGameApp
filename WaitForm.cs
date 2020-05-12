using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CheatGameApp
{
    public partial class WaitForm : Form
    {
      public WaitForm()
      {
        InitializeComponent();
        this.StartPosition = FormStartPosition.CenterParent;
      }
      public WaitForm(Form parent)
      {
        InitializeComponent();
        if (parent != null)
        {
          this.StartPosition = FormStartPosition.Manual;
          this.Location = new Point(parent.Location.X + parent.Width / 2 - this.Width / 2, parent.Location.Y + parent.Height / 2 - this.Height / 2);
        }
        else
        {
          this.StartPosition = FormStartPosition.CenterParent;
        }
      }
      public void CloseLoadingForm()
      {
        this.DialogResult = DialogResult.OK;
        this.Close();
        if (label1.Image != null)
        {
          label1.Image.Dispose();
        }
      }
    }

 

    public class WaitWndFun
    {
      WaitForm loadingForm = null;
      Thread loadthread;
      bool can_close = false;
      /// <summary>
      /// 显示等待框
      /// </summary>
      public void Show()
      {
        loadthread = new Thread(new ThreadStart(LoadingProcessEx));
        loadthread.Start();
      }
      /// <summary>
      /// 显示等待框
      /// </summary>
      /// <param name="parent">父窗体</param>
      public void Show(Form parent)
      {
        loadthread = new Thread(new ParameterizedThreadStart(LoadingProcessEx));
        loadthread.Start(parent);
      }
      public void Close()
      {
            while (!can_close) ;
            
            loadingForm.BeginInvoke(new System.Threading.ThreadStart(loadingForm.CloseLoadingForm));
            loadingForm = null;
            loadthread = null;
            
      }
      private void LoadingProcessEx()
      {
        loadingForm = new WaitForm();
        loadingForm.Shown += LoadingForm_Shown;
        loadingForm.ShowDialog();
      }

        private void LoadingForm_Shown(object sender, EventArgs e)
        {
            can_close = true;
        }

        private void LoadingProcessEx(object parent)
      {
        Form Cparent = parent as Form;
        loadingForm = new WaitForm(Cparent);
        loadingForm.ShowDialog();
      }
    }
}
