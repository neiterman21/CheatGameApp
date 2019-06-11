using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CheatGameModel.Players.Enumarations;

namespace CheatGameModel.Players
{
    public sealed class Demographics
    {
        public string FullName { get; set; }
        public int Age { get; set; }
        public Genders Gender { get; set; }
        public Counties CountryOfBirth { get; set; }
        public Counties ParentsCountryOfBirth { get; set; }
        public EducationType EducationType { get; set; }
        public EducationFields EducationField { get; set; }
        public bool IsStudent { get; set; }
    }
}
