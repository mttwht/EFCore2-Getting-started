using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp.Domain;
using SamuraiApp.Data;
using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main( string[] args )
        {
            //InsertSamurai();
            //InsertMultipleSamurais();
            //SimpleSamuraiQuery();
            //MoreQueries();
            //RetrieveAndUpdateSamurai();
            //RetrieveAndUpdateMultipleSamurais();
            //InsertBattle();
            //QueryAndUpdateBattle_Disconnected();
            //DeleteWhileTracked();
            //DeleteMany();
            //DeleteWhileNotTracked();
            //DeleteById(3);
            //InsertNewPkFkGraph();
            //InsertNewPkFkGraphMultipleChildren();
            //AddChildToExistingObjectWhileTracked();
            //AddChildToExistingObjectWhileNotTracked(5);
            //EagerLoadSamuraiWithQuotes();
            //ProjectSomeProperties();
            //ProjectSamuraiWithQuotes();
            //FilterByRelatedData();
            //ModifyRelatedDataWhileTracked();
            ModifyRelatedDataWhileNotTracked();
        }

        private static void ModifyRelatedDataWhileNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            var quote = samurai.Quotes[0];
            quote.Text += " Did you hear that?";
            using(var newContext = new SamuraiContext()) {
                newContext.Quotes.Update(quote);
                newContext.SaveChanges();
            }
        }

        private static void ModifyRelatedDataWhileTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes).FirstOrDefault();
            samurai.Quotes[0].Text += " Did you hear that?";
            _context.SaveChanges();
        }

        private static void FilterByRelatedData()
        {
            var samurais = _context.Samurais
                    .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                    .ToList();
        }

        private static void ProjectSamuraiWithQuotes()
        {
            //var somePropertiesWithQuotes = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes }).ToList();

            //var somePropertiesWithQuoteCount = _context.Samurais.Select(s => new { s.Id, s.Name, s.Quotes.Count }).ToList();

            //var somePropertiesWithSomeQuotes = _context.Samurais
            //        .Select(s => new {
            //            s.Id, s.Name,
            //            HappyQuotes =s.Quotes.Where(q=>q.Text.Contains("happy"))
            //        }).ToList();

            // This currently has a bug where HappyQuotes are returned but Samurai object is unaware of their quotes
            //var samuraisWithSomeQuotes = _context.Samurais
            //        .Select(s => new {
            //            Samurai = s,
            //            HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy"))
            //        }).ToList();

            // This does make Samurais aware of their own happy quotes
            var samurais = _context.Samurais.ToList();
            var happyQuotes = _context.Quotes.Where(q => q.Text.Contains("happy")).ToList();
        }

        public struct IdAndName
        {
            public IdAndName(int id, string name)
            {
                Id = id;
                Name = name;
            }
            public int Id;
            public string Name;
        }

        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
            var idsAndNames = _context.Samurais.Select(s => new IdAndName(s.Id, s.Name)).ToList();
            //return someProperties.ToList<dynamic>(); // Allows objects to exist outside of this method
        }

        private static void EagerLoadSamuraiWithQuotes()
        {
            var samuraiWithQuotes = _context.Samurais
                    .Include(s => s.Quotes)
                    //.Include(s => s.SecretIdentity) // eager load another child
                    //.ThenInclude(q => q.Translations) // retrieve grandchildren also
                    //.Include(s => s.Quotes.Translations) // retrieve ONLY grandchildren
                    .ToList();
        }

        private static void AddChildToExistingObjectWhileNotTracked(int samuraiId)
        {
            // Easiest way to create children offline is by manually setting FK
            var quote = new Quote {
                Text = "Now that I've saved you, will you feed me dinner?",
                SamuraiId = samuraiId
            };
            using( var newContext = new SamuraiContext() ) {
                newContext.Quotes.Add(quote);
                newContext.SaveChanges();
            }
        }

        private static void AddChildToExistingObjectWhileTracked()
        {
            var samurai = _context.Samurais.First();
            samurai.Quotes.Add(new Quote {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraphMultipleChildren()
        {
            // Three INSERTs are generated (supposed to be two?)
            var samurai = new Samurai {
                Name = "Kyuzo",
                Quotes = new List<Quote> {
                    new Quote {Text = "Watch out for my sharp sword!"},
                    new Quote {Text = "I told you to watch out for the sharp sword! Oh well."},
                },
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertNewPkFkGraph()
        {
            // Two INSERTs are generated
            var samurai = new Samurai {
                Name = "Kambei Shimada",
                Quotes = new List<Quote> {
                    new Quote {Text = "I've come to save you."}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void InsertMultipleSamurais()
        {
            var samuraiMatt = new Samurai() { Name = "Matt" };
            var samuraiFreyja = new Samurai() { Name = "Freyja" };
            //_context.Samurais.Add(samuraiMatt);
            //_context.Samurais.Add(samuraiFreyja);
            _context.Samurais.AddRange(samuraiMatt, samuraiFreyja);
            _context.SaveChanges();
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai() { Name = "Matt" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void SimpleSamuraiQuery()
        {
            var samurais = _context.Samurais.ToList();
        }

        private static void MoreQueries()
        {
            // Does not parameterise
            //var samurais = _context.Samurais.Where(s => s.Name == "Matt").ToList();

            // Parameretise variables
            //var name = "Matt";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();

            // Get first result by name, or null on default
            //var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);

            // Get by ID
            //var samurai = _context.Samurais.Find(2);

            // Get using LIKE
            //var samurais = _context.Samurais.Where(s => EF.Functions.Like(s.Name, "M%")).ToList();
            // same as
            var samurais = _context.Samurais.Where(s => s.Name.Contains("M")).ToList();
        }

        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            if(!samurai.Name.Contains("San")) {
                samurai.Name += " San";
                _context.SaveChanges();
            }
        }

        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Where(s => !s.Name.Contains("San")).ToList();
            samurais.ForEach(s => s.Name += " San");
            _context.SaveChanges();
        }

        private static void InsertBattle() {
            _context.Battles.Add(new Battle() {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15),
            });
            _context.SaveChanges();
        }

        private static void QueryAndUpdateBattle_Disconnected() {
            var battle = _context.Battles.FirstOrDefault();
            battle.EndDate = new DateTime(1560, 06, 30);
            using(var newContextInstance = new SamuraiContext()) {
                newContextInstance.Battles.Update(battle);
                newContextInstance.SaveChanges();
            }
        }

        private static void DeleteWhileTracked() {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Matt");
            _context.Samurais.Remove(samurai);
            // Alternatives:
            //_context.Remove(samurai);
            //_context.Samurais.Remove(_context.Samurais.Find(1));
            _context.SaveChanges();
        }

        private static void DeleteMany()
        {
            var samurais = _context.Samurais.Where(s => s.Name == "Matt");
            _context.Samurais.RemoveRange(samurais);
            // Alternative:
            //_context.RemoveRange(samurais);
            _context.SaveChanges();
        }

        private static void DeleteWhileNotTracked() {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Matt");
            using(var newContextInstance = new SamuraiContext()) {
                newContextInstance.Samurais.Remove(samurai);
                // Alternative:
                //newContextInstance.Entry(samurai).State = EntityState.Deleted;
                newContextInstance.SaveChanges();
            }
        }

        private static void DeleteById(int samuraiId) {
            var samurai = _context.Samurais.Find(samuraiId);
            _context.Remove(samurai);
            _context.SaveChanges();
            // Alternative to fetching before deleting
            //_context.Database.ExecuteSqlCommand("DELETE FROM Samurais WHERE Id={0}", samuraiId);
            // OR call a sproc
        }

    }
}
