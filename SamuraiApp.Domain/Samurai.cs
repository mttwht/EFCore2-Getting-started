using System.Collections.Generic;

namespace SamuraiApp.Domain
{
    public class Samurai
    {

        public Samurai()
        {
            Quotes = new List<Quote>();
            SamuraiBattles = new List<SamuraiBattle>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public PersonName BetterName { get; set; }
        public List<Quote> Quotes { get; set; }
        public List<SamuraiBattle> SamuraiBattles { get; set; }
        public SecretIdentity SecretIdentity { get; set; }

        public List<Battle> Battles()
        {
            var battles = new List<Battle>();
            foreach(var sb in SamuraiBattles) {
                battles.Add(sb.Battle);
            }
            return battles;
        }
    }
}
