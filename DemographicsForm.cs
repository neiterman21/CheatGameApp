using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheatGameModel.Players;
using CheatGameModel.Players.Enumarations;
using System.Collections;

namespace CheatGameApp
{
    public partial class DemographicsForm : Form
    {
        private int m_israelIndex = new ArrayList(Enum.GetValues(typeof(Counties))).IndexOf(Counties.Israel);
        public DemographicsForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            genderComboBox.DataSource = Enum.GetValues(typeof(Genders));
            countryOfBirthComboBox.DataSource = Enum.GetValues(typeof(Counties));
            countryOfBirthComboBox.BindingContext = new BindingContext();
            parentsCountryOfBirthComboBox.DataSource = Enum.GetValues(typeof(Counties));
            parentsCountryOfBirthComboBox.BindingContext = new BindingContext();

            educationTypeComboBox.DataSource = Enum.GetValues(typeof(EducationType));
            educationFieldComboBox.DataSource = Enum.GetValues(typeof(EducationFields));

            countryOfBirthComboBox.SelectedIndex = m_israelIndex;
            parentsCountryOfBirthComboBox.SelectedIndex = -1;
            educationTypeComboBox.SelectedIndex = -1;
            educationFieldComboBox.SelectedIndex = -1;
            genderComboBox.SelectedIndex = -1;
        }

        public Demographics GetDemographics()
        {
            Demographics d = new Demographics();
            d.FullName = nameTextBox.Text;
            d.Age = (int)ageNumericUpDown.Value;
            d.Gender = (Genders)genderComboBox.SelectedValue;

            d.CountryOfBirth = (Counties)countryOfBirthComboBox.SelectedValue;
            if (d.CountryOfBirth == Counties.Israel)
                d.ParentsCountryOfBirth = (Counties)parentsCountryOfBirthComboBox.SelectedValue;

            d.EducationType = (EducationType)educationTypeComboBox.SelectedValue;
            d.EducationField = (EducationFields)educationFieldComboBox.SelectedValue;
            d.IsStudent = isStudentCheckBox.Checked;

            return d;
        }
        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.KeyCode == Keys.Escape)
            {
                string title = "Exit";
                string text = "Are you sure you want to exit the game?" + Environment.NewLine + "Any data that was unsaved will be lost.";
                if (DialogResult.Yes == MessageBox.Show(text, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    Application.Exit();
            }
        }

        private void countryOfBirthComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isFromIsrael = (Counties)countryOfBirthComboBox.SelectedValue == Counties.Israel;
            parentsCountryOfBirthComboBox.Enabled =
                parentsCountryOfBirthLabel.Enabled = isFromIsrael;

            if (!isFromIsrael)
                parentsCountryOfBirthComboBox.SelectedIndex = -1;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            StringBuilder errorSB = new StringBuilder();
            if (nameTextBox.Text.Trim() == string.Empty)
                errorSB.AppendLine("Please fill in a valid name.");



            if (genderComboBox.SelectedIndex < 0)
                errorSB.AppendLine("Please fill in your gender.");


            if (countryOfBirthComboBox.SelectedIndex < 0)
                errorSB.AppendLine("Please fill in Country of Birth.");

            object country = countryOfBirthComboBox.SelectedValue;
            if (country != null && (Counties)country == Counties.Israel && parentsCountryOfBirthComboBox.SelectedIndex < 0)
                errorSB.AppendLine("Please fill in Parent's Country of Birth.");
            if (educationFieldComboBox.SelectedIndex < 0)
                errorSB.AppendLine("Please fill Education.");

            if (educationTypeComboBox.SelectedIndex < 0)
                errorSB.AppendLine("Please fill in Education Field.");

            if (errorSB.Length > 0)
            {
                errorSB.Insert(0, "There one or more missing fields:" + Environment.NewLine);
                MessageBox.Show(this, errorSB.ToString(), "Missing Fields", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }
    }
}
