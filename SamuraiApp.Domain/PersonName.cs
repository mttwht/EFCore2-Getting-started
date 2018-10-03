namespace SamuraiApp.Domain
{
    public class PersonName
    {
        public PersonName(string givenName, string surname)
        {
            Surname = surname;
            GivenName = givenName;
        }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        public string FullName => $"{GivenName} {Surname}";
        public string FullNameReverse => $"{Surname}, {GivenName}";
    }
}
