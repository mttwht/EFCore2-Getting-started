using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    [Owned]
    public class PersonName
    {
        private PersonName(string givenName, string surname)
        {
            Surname = surname;
            GivenName = givenName;
        }
        //private PersonName() { } // Allows EFCore to create object through reflection; not needed in EFCore 2.1?
        public string Surname { get; private set; }
        public string GivenName { get; private set; }
        public string FullName => $"{GivenName} {Surname}";
        public string FullNameReverse => $"{Surname}, {GivenName}";

        // For EFCore 2.0/2.1 needing owned types to always be instantiated
        #region Workarounds

        public static PersonName Create(string givenName, string surname)
        {
            return new PersonName(givenName, surname);
        }

        public static PersonName Empty()
        {
            return new PersonName(string.Empty, string.Empty);
        }

        #endregion

        public override bool Equals(object obj)
        {
            var name = obj as PersonName;
            return name != null &&
                   Surname == name.Surname &&
                   GivenName == name.GivenName;
        }

        public override int GetHashCode()
        {
            var hashCode = -409411797;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Surname);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(GivenName);
            return hashCode;
        }

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(GivenName) & string.IsNullOrEmpty(Surname);
        }

        public static bool operator ==(PersonName name1, PersonName name2)
        {
            return EqualityComparer<PersonName>.Default.Equals(name1, name2);
        }

        public static bool operator !=(PersonName name1, PersonName name2)
        {
            return !(name1 == name2);
        }

    }
}
