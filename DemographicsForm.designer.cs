namespace CheatGameApp
{
    partial class DemographicsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ageNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.parentsCountryOfBirthLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.genderComboBox = new System.Windows.Forms.ComboBox();
            this.countryOfBirthComboBox = new System.Windows.Forms.ComboBox();
            this.educationTypeComboBox = new System.Windows.Forms.ComboBox();
            this.educationFieldComboBox = new System.Windows.Forms.ComboBox();
            this.parentsCountryOfBirthComboBox = new System.Windows.Forms.ComboBox();
            this.isStudentCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.ageNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(156, 12);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(237, 20);
            this.nameTextBox.TabIndex = 1;
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.Location = new System.Drawing.Point(318, 271);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 15;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Age";
            // 
            // ageNumericUpDown
            // 
            this.ageNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ageNumericUpDown.Location = new System.Drawing.Point(156, 44);
            this.ageNumericUpDown.Margin = new System.Windows.Forms.Padding(6);
            this.ageNumericUpDown.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.ageNumericUpDown.Minimum = new decimal(new int[] {
            18,
            0,
            0,
            0});
            this.ageNumericUpDown.Name = "ageNumericUpDown";
            this.ageNumericUpDown.Size = new System.Drawing.Size(237, 20);
            this.ageNumericUpDown.TabIndex = 3;
            this.ageNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ageNumericUpDown.Value = new decimal(new int[] {
            24,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Gender:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Country of Birth:";
            // 
            // parentsCountryOfBirthLabel
            // 
            this.parentsCountryOfBirthLabel.AutoSize = true;
            this.parentsCountryOfBirthLabel.Enabled = false;
            this.parentsCountryOfBirthLabel.Location = new System.Drawing.Point(21, 146);
            this.parentsCountryOfBirthLabel.Name = "parentsCountryOfBirthLabel";
            this.parentsCountryOfBirthLabel.Size = new System.Drawing.Size(123, 13);
            this.parentsCountryOfBirthLabel.TabIndex = 8;
            this.parentsCountryOfBirthLabel.Text = "Parents\' Country of Birth:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 179);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Education:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 212);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Field:";
            // 
            // genderComboBox
            // 
            this.genderComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.genderComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.genderComboBox.FormattingEnabled = true;
            this.genderComboBox.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.genderComboBox.Location = new System.Drawing.Point(156, 76);
            this.genderComboBox.Margin = new System.Windows.Forms.Padding(6);
            this.genderComboBox.Name = "genderComboBox";
            this.genderComboBox.Size = new System.Drawing.Size(237, 21);
            this.genderComboBox.TabIndex = 5;
            // 
            // countryOfBirthComboBox
            // 
            this.countryOfBirthComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.countryOfBirthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.countryOfBirthComboBox.FormattingEnabled = true;
            this.countryOfBirthComboBox.Location = new System.Drawing.Point(156, 109);
            this.countryOfBirthComboBox.Margin = new System.Windows.Forms.Padding(6);
            this.countryOfBirthComboBox.Name = "countryOfBirthComboBox";
            this.countryOfBirthComboBox.Size = new System.Drawing.Size(237, 21);
            this.countryOfBirthComboBox.TabIndex = 7;
            this.countryOfBirthComboBox.SelectedIndexChanged += new System.EventHandler(this.countryOfBirthComboBox_SelectedIndexChanged);
            // 
            // educationTypeComboBox
            // 
            this.educationTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.educationTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.educationTypeComboBox.FormattingEnabled = true;
            this.educationTypeComboBox.Location = new System.Drawing.Point(156, 175);
            this.educationTypeComboBox.Margin = new System.Windows.Forms.Padding(6);
            this.educationTypeComboBox.Name = "educationTypeComboBox";
            this.educationTypeComboBox.Size = new System.Drawing.Size(237, 21);
            this.educationTypeComboBox.TabIndex = 11;
            // 
            // educationFieldComboBox
            // 
            this.educationFieldComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.educationFieldComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.educationFieldComboBox.FormattingEnabled = true;
            this.educationFieldComboBox.Location = new System.Drawing.Point(156, 208);
            this.educationFieldComboBox.Margin = new System.Windows.Forms.Padding(6);
            this.educationFieldComboBox.Name = "educationFieldComboBox";
            this.educationFieldComboBox.Size = new System.Drawing.Size(237, 21);
            this.educationFieldComboBox.TabIndex = 13;
            // 
            // parentsCountryOfBirthComboBox
            // 
            this.parentsCountryOfBirthComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parentsCountryOfBirthComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parentsCountryOfBirthComboBox.Enabled = false;
            this.parentsCountryOfBirthComboBox.FormattingEnabled = true;
            this.parentsCountryOfBirthComboBox.Location = new System.Drawing.Point(156, 142);
            this.parentsCountryOfBirthComboBox.Margin = new System.Windows.Forms.Padding(6);
            this.parentsCountryOfBirthComboBox.Name = "parentsCountryOfBirthComboBox";
            this.parentsCountryOfBirthComboBox.Size = new System.Drawing.Size(237, 21);
            this.parentsCountryOfBirthComboBox.TabIndex = 9;
            // 
            // isStudentCheckBox
            // 
            this.isStudentCheckBox.AutoSize = true;
            this.isStudentCheckBox.Location = new System.Drawing.Point(156, 241);
            this.isStudentCheckBox.Margin = new System.Windows.Forms.Padding(6);
            this.isStudentCheckBox.Name = "isStudentCheckBox";
            this.isStudentCheckBox.Size = new System.Drawing.Size(93, 17);
            this.isStudentCheckBox.TabIndex = 14;
            this.isStudentCheckBox.Text = "I am a student";
            this.isStudentCheckBox.UseVisualStyleBackColor = true;
            // 
            // DemographicsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 303);
            this.Controls.Add(this.isStudentCheckBox);
            this.Controls.Add(this.educationFieldComboBox);
            this.Controls.Add(this.educationTypeComboBox);
            this.Controls.Add(this.parentsCountryOfBirthComboBox);
            this.Controls.Add(this.countryOfBirthComboBox);
            this.Controls.Add(this.genderComboBox);
            this.Controls.Add(this.ageNumericUpDown);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.parentsCountryOfBirthLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DemographicsForm";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Demographics";
            ((System.ComponentModel.ISupportInitialize)(this.ageNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown ageNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label parentsCountryOfBirthLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox genderComboBox;
        private System.Windows.Forms.ComboBox countryOfBirthComboBox;
        private System.Windows.Forms.ComboBox educationTypeComboBox;
        private System.Windows.Forms.ComboBox educationFieldComboBox;
        private System.Windows.Forms.ComboBox parentsCountryOfBirthComboBox;
        private System.Windows.Forms.CheckBox isStudentCheckBox;
    }
}