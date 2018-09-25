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
            QueryAndUpdateBattle_Disconnected();
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

    }
}
