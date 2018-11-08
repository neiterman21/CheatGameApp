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
            this.SuspendLayout();
            // 
            // deckLabel1
            // 
            this.deckLabel1.BackColor = System.Drawing.Color.Transparent;
            this.deckLabel1.DeckCards = "1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0";
            this.deckLabel1.Location = new System.Drawing.Point(55, 78);
            this.deckLabel1.Name = "deckLabel1";
            this.deckLabel1.Size = new System.Drawing.Size(166, 150);
            this.deckLabel1.TabIndex = 0;
            // 
            // UnitTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.deckLabel1);
            this.Name = "UnitTest";
            this.Text = "UnitTest";
            this.ResumeLayout(false);

        }

        #endregion

        private DeckLabel deckLabel1;
    }
}