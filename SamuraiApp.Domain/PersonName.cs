namespace SamuraiApp.Domain
{
    public class PersonName
    {
        private PersonName(string givenName, string surname)
        {
            Surname = surname;
            GivenName = givenName;
        }
        //private PersonName() { } // Allows EFCore to create object through reflection; not needed in EFCore 2.1?
        public string Surname { get; set; }
        public string GivenName { get; set; }
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

        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(GivenName) & string.IsNullOrEmpty(Surname);
        }

        #endregion
    }
}
