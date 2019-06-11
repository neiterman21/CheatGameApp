namespace CheatGameApp
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.close_Button = new System.Windows.Forms.Button();
            this.Replay_button = new System.Windows.Forms.Button();
            this.repport_button = new System.Windows.Forms.Button();
            this.my_claim = new CheatGameApp.DeckLabel();
            this.Claim_Label = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // close_Button
            // 
            this.close_Button.Location = new System.Drawing.Point(437, 162);
            this.close_Button.Name = "close_Button";
            this.close_Button.Size = new System.Drawing.Size(98, 33);
            this.close_Button.TabIndex = 0;
            this.close_Button.Text = "Close";
            this.close_Button.UseVisualStyleBackColor = true;
            this.close_Button.Click += new System.EventHandler(this.close_Button_Click);
            // 
            // Replay_button
            // 
            this.Replay_button.Location = new System.Drawing.Point(243, 162);
            this.Replay_button.Name = "Replay_button";
            this.Replay_button.Size = new System.Drawing.Size(93, 33);
            this.Replay_button.TabIndex = 1;
            this.Replay_button.Text = "Replay Claim";
            this.Replay_button.UseVisualStyleBackColor = true;
            // 
            // repport_button
            // 
            this.repport_button.Location = new System.Drawing.Point(46, 162);
            this.repport_button.Name = "repport_button";
            this.repport_button.Size = new System.Drawing.Size(106, 33);
            this.repport_button.TabIndex = 2;
            this.repport_button.Text = "report";
            this.repport_button.UseVisualStyleBackColor = true;
            this.repport_button.Click += new System.EventHandler(this.repport_button_Click);
            // 
            // my_claim
            // 
            this.my_claim.BackColor = System.Drawing.Color.Transparent;
            this.my_claim.CardSize = new System.Drawing.Size(55, 76);
            this.my_claim.DeckCards = "0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 0";
            this.my_claim.Location = new System.Drawing.Point(46, 25);
            this.my_claim.Name = "my_claim";
            this.my_claim.Size = new System.Drawing.Size(80, 131);
            this.my_claim.TabIndex = 20;
            this.my_claim.Visible = false;
            // 
            // Claim_Label
            // 
            this.Claim_Label.AutoSize = true;
            this.Claim_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Claim_Label.Location = new System.Drawing.Point(42, 0);
            this.Claim_Label.Name = "Claim_Label";
            this.Claim_Label.Size = new System.Drawing.Size(94, 22);
            this.Claim_Label.TabIndex = 21;
            this.Claim_Label.Text = "Your claim";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(207, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(328, 110);
            this.textBox1.TabIndex = 22;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // Form2
            // 
            this.ClientSize = new System.Drawing.Size(570, 211);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.Claim_Label);
            this.Controls.Add(this.my_claim);
            this.Controls.Add(this.repport_button);
            this.Controls.Add(this.Replay_button);
            this.Controls.Add(this.close_Button);
            this.Name = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button close_Button;
        private System.Windows.Forms.Button Replay_button;
        private System.Windows.Forms.Button repport_button;
        private DeckLabel my_claim;
        private System.Windows.Forms.Label Claim_Label;
        private System.Windows.Forms.TextBox textBox1;
    }
}
