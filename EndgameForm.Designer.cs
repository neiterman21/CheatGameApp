namespace CheatGameApp
{
  partial class EndgameForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.CodeLable = new System.Windows.Forms.Label();
      this.MessageLable = new CheatGameApp.Model.GrowLabel();
      this.CopyButton = new System.Windows.Forms.Button();
      this.CloseButton = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // notifyIcon1
      // 
      this.notifyIcon1.Text = "notifyIcon1";
      this.notifyIcon1.Visible = true;
      // 
      // CodeLable
      // 
      this.CodeLable.AutoSize = true;
      this.CodeLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.CodeLable.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
      this.CodeLable.Location = new System.Drawing.Point(12, 169);
      this.CodeLable.Name = "CodeLable";
      this.CodeLable.Size = new System.Drawing.Size(53, 20);
      this.CodeLable.TabIndex = 1;
      this.CodeLable.Text = "label1";
      // 
      // MessageLable
      // 
      this.MessageLable.AutoSize = true;
      this.MessageLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.MessageLable.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
      this.MessageLable.Location = new System.Drawing.Point(12, 9);
      this.MessageLable.Name = "MessageLable";
      this.MessageLable.Size = new System.Drawing.Size(95, 20);
      this.MessageLable.TabIndex = 0;
      this.MessageLable.Text = "growLabel1";
      // 
      // CopyButton
      // 
      this.CopyButton.Location = new System.Drawing.Point(500, 164);
      this.CopyButton.Name = "CopyButton";
      this.CopyButton.Size = new System.Drawing.Size(104, 25);
      this.CopyButton.TabIndex = 2;
      this.CopyButton.Text = "Copy to clipboard";
      this.CopyButton.UseVisualStyleBackColor = true;
      this.CopyButton.Visible = false;
      this.CopyButton.Click += new System.EventHandler(this.button1_Click);
      // 
      // CloseButton
      // 
      this.CloseButton.Location = new System.Drawing.Point(279, 226);
      this.CloseButton.Name = "CloseButton";
      this.CloseButton.Size = new System.Drawing.Size(75, 23);
      this.CloseButton.TabIndex = 3;
      this.CloseButton.Text = "Close";
      this.CloseButton.UseVisualStyleBackColor = true;
      this.CloseButton.Visible = false;
      this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
      // 
      // EndgameForm
      // 
      this.ClientSize = new System.Drawing.Size(647, 261);
      this.ControlBox = false;
      this.Controls.Add(this.CloseButton);
      this.Controls.Add(this.CopyButton);
      this.Controls.Add(this.CodeLable);
      this.Controls.Add(this.MessageLable);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "EndgameForm";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private Model.GrowLabel MessageLable;
    private System.Windows.Forms.Label CodeLable;
    private System.Windows.Forms.Button CopyButton;
    private System.Windows.Forms.Button CloseButton;
  }
}
