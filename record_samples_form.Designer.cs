namespace CheatGameApp
{
    partial class record_samples_form
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
            this.card_count = new System.Windows.Forms.ComboBox();
            this.card_value = new System.Windows.Forms.ComboBox();
            this.RecordButton = new System.Windows.Forms.Button();
            this.ReplayButton = new System.Windows.Forms.Button();
            this.my_claim = new CheatGameApp.DeckLabel();
            this.SuspendLayout();
            // 
            // card_count
            // 
            this.card_count.FormattingEnabled = true;
            this.card_count.Items.AddRange(new object[] {
            "One",
            "Two",
            "Three",
            "Four"});
            this.card_count.Location = new System.Drawing.Point(155, 212);
            this.card_count.Name = "card_count";
            this.card_count.Size = new System.Drawing.Size(150, 21);
            this.card_count.TabIndex = 22;
            this.card_count.SelectedIndexChanged += new System.EventHandler(this.card_count_SelectedIndexChanged);
            // 
            // card_value
            // 
            this.card_value.FormattingEnabled = true;
            this.card_value.Items.AddRange(new object[] {
            "Ace",
            "Two",
            "Three",
            "Four",
            "Five",
            "Six",
            "Seven",
            "Eight",
            "Nine",
            "Ten",
            "Jack",
            "Queen",
            "King"});
            this.card_value.Location = new System.Drawing.Point(374, 212);
            this.card_value.Name = "card_value";
            this.card_value.Size = new System.Drawing.Size(157, 21);
            this.card_value.TabIndex = 23;
            this.card_value.SelectedIndexChanged += new System.EventHandler(this.card_count_SelectedIndexChanged);
            // 
            // RecordButton
            // 
            this.RecordButton.Location = new System.Drawing.Point(581, 209);
            this.RecordButton.Name = "RecordButton";
            this.RecordButton.Size = new System.Drawing.Size(75, 23);
            this.RecordButton.TabIndex = 24;
            this.RecordButton.Text = "Record";
            this.RecordButton.UseVisualStyleBackColor = true;
            this.RecordButton.Click += new System.EventHandler(this.RecordButton_Click);
            // 
            // ReplayButton
            // 
            this.ReplayButton.Location = new System.Drawing.Point(684, 208);
            this.ReplayButton.Name = "ReplayButton";
            this.ReplayButton.Size = new System.Drawing.Size(75, 23);
            this.ReplayButton.TabIndex = 25;
            this.ReplayButton.Text = "Replay";
            this.ReplayButton.UseVisualStyleBackColor = true;
            this.ReplayButton.Click += new System.EventHandler(this.ReplayButton_Click);
            // 
            // my_claim
            // 
            this.my_claim.BackColor = System.Drawing.Color.Transparent;
            this.my_claim.CardSize = new System.Drawing.Size(55, 76);
            this.my_claim.DeckCards = "0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0";
            this.my_claim.Location = new System.Drawing.Point(255, 39);
            this.my_claim.Name = "my_claim";
            this.my_claim.Size = new System.Drawing.Size(251, 82);
            this.my_claim.Style = CheatGameApp.DeckLabel.LayoutStyle.Horizontal;
            this.my_claim.TabIndex = 21;
            this.my_claim.Visible = false;
            // 
            // record_samples_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ReplayButton);
            this.Controls.Add(this.RecordButton);
            this.Controls.Add(this.card_value);
            this.Controls.Add(this.card_count);
            this.Controls.Add(this.my_claim);
            this.Name = "record_samples_form";
            this.Text = "record_samples_form";
            this.Load += new System.EventHandler(this.record_samples_form_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DeckLabel my_claim;
        private System.Windows.Forms.ComboBox card_count;
        private System.Windows.Forms.ComboBox card_value;
        private System.Windows.Forms.Button RecordButton;
        private System.Windows.Forms.Button ReplayButton;
    }
}