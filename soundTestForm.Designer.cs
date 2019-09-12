namespace CheatGameApp
{
  partial class soundTestForm
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
      this.Actionbutton = new System.Windows.Forms.Button();
      this.YesButton = new System.Windows.Forms.Button();
      this.NoButton = new System.Windows.Forms.Button();
      this.questionLable = new System.Windows.Forms.Label();
      this.recordingLable = new System.Windows.Forms.Label();
      this.growLabel1 = new CheatGameApp.Model.GrowLabel();
      this.SuspendLayout();
      // 
      // Actionbutton
      // 
      this.Actionbutton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Actionbutton.Location = new System.Drawing.Point(291, 101);
      this.Actionbutton.Name = "Actionbutton";
      this.Actionbutton.Size = new System.Drawing.Size(103, 48);
      this.Actionbutton.TabIndex = 0;
      this.Actionbutton.Text = "play sound";
      this.Actionbutton.UseVisualStyleBackColor = true;
      this.Actionbutton.Click += new System.EventHandler(this.Actionbutton_Click);
      // 
      // YesButton
      // 
      this.YesButton.Location = new System.Drawing.Point(118, 272);
      this.YesButton.Name = "YesButton";
      this.YesButton.Size = new System.Drawing.Size(88, 35);
      this.YesButton.TabIndex = 3;
      this.YesButton.Text = "Yes";
      this.YesButton.UseVisualStyleBackColor = true;
      this.YesButton.Visible = false;
      this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
      // 
      // NoButton
      // 
      this.NoButton.Location = new System.Drawing.Point(573, 272);
      this.NoButton.Name = "NoButton";
      this.NoButton.Size = new System.Drawing.Size(95, 35);
      this.NoButton.TabIndex = 4;
      this.NoButton.Text = "No, try again";
      this.NoButton.UseVisualStyleBackColor = true;
      this.NoButton.Visible = false;
      this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
      // 
      // questionLable
      // 
      this.questionLable.AutoSize = true;
      this.questionLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.questionLable.Location = new System.Drawing.Point(212, 205);
      this.questionLable.Name = "questionLable";
      this.questionLable.Size = new System.Drawing.Size(323, 29);
      this.questionLable.TabIndex = 6;
      this.questionLable.Text = "Did you hear the message?";
      // 
      // recordingLable
      // 
      this.recordingLable.AutoSize = true;
      this.recordingLable.BackColor = System.Drawing.SystemColors.HotTrack;
      this.recordingLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.recordingLable.ForeColor = System.Drawing.SystemColors.ButtonFace;
      this.recordingLable.Location = new System.Drawing.Point(242, 113);
      this.recordingLable.Name = "recordingLable";
      this.recordingLable.Size = new System.Drawing.Size(210, 48);
      this.recordingLable.TabIndex = 15;
      this.recordingLable.Text = "Recording";
      this.recordingLable.Visible = false;
      // 
      // growLabel1
      // 
      this.growLabel1.AutoSize = true;
      this.growLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.growLabel1.Location = new System.Drawing.Point(31, 27);
      this.growLabel1.Name = "growLabel1";
      this.growLabel1.Size = new System.Drawing.Size(628, 29);
      this.growLabel1.TabIndex = 8;
      this.growLabel1.Text = "To check you headphone please play a short message";
      // 
      // soundTestForm
      // 
      this.ClientSize = new System.Drawing.Size(755, 356);
      this.Controls.Add(this.recordingLable);
      this.Controls.Add(this.growLabel1);
      this.Controls.Add(this.questionLable);
      this.Controls.Add(this.NoButton);
      this.Controls.Add(this.YesButton);
      this.Controls.Add(this.Actionbutton);
      this.DoubleBuffered = true;
      this.Name = "soundTestForm";
      this.ShowIcon = false;
      this.Text = "Sound test";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button Actionbutton;
    private System.Windows.Forms.Button YesButton;
    private System.Windows.Forms.Button NoButton;
    private System.Windows.Forms.Label questionLable;
    private Model.GrowLabel growLabel1;
    private System.Windows.Forms.Label recordingLable;
  }
}
