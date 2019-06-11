using NAudio.Wave;
using System.Windows.Forms;

namespace CheatGameApp

{
    partial class UnitTest
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
            this.deckLabel1 = new CheatGameApp.DeckLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // deckLabel1
            // 
            this.deckLabel1.BackColor = System.Drawing.Color.Transparent;
            this.deckLabel1.DeckCards = "1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.deckLabel1.Location = new System.Drawing.Point(82, 120);
            this.deckLabel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.deckLabel1.Name = "deckLabel1";
            this.deckLabel1.Size = new System.Drawing.Size(249, 231);
            this.deckLabel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(150, 65);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // UnitTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 403);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.deckLabel1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "UnitTest";
            this.Text = "UnitTest";
            this.ResumeLayout(false);

    }

        #endregion

        private DeckLabel deckLabel1;
        private Button button1;
    }
}