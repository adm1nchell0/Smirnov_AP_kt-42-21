using System.Text.RegularExpressions;

namespace Smirnov_AP_kt_42_21.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }

        public bool isValidLastName()
        {
            return Regex.Match(LastName, @"^([А-Я]|Ё)([а-я]|ё)+").Success;
        }
        public bool isValidFirstName()
        {
            return Regex.Match(FirstName, @"^([А-Я]|Ё)([а-я]|ё)+").Success;
        }

        public bool isValidMiddleName()
        {
            return Regex.Match(MiddleName, @"^([А-Я]|Ё)([а-я]|ё)+").Success;
        }
    }
}
