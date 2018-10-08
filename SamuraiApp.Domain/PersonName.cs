namespace SamuraiApp.Domain
{
    public class PersonName
    {
        public PersonName(string givenName, string surname)
        {
            Surname = surname;
            GivenName = givenName;
        }
        //private PersonName() { } // Allows EFCore to create object through reflection; not needed in EFCore 2.1?
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string FullName => $"{GivenName} {Surname}";
        public string FullNameReverse => $"{Surname}, {GivenName}";
    }
}
