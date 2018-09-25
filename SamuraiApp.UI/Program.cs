using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SamuraiApp.Domain;
using SamuraiApp.Data;

namespace SamuraiApp.UI
{
    class Program
    {
        static void Main( string[] args )
        {
            //InsertSamurai();
            InsertMultipleSamurais();
        }

        private static void InsertMultipleSamurais()
        {
            var samuraiMatt = new Samurai() { Name = "Matt" };
            var samuraiFreyja = new Samurai() { Name = "Freyja" };
            using(var context = new SamuraiContext()) {
                //context.Samurais.Add(samuraiMatt);
                //context.Samurais.Add(samuraiFreyja);
                context.Samurais.AddRange(samuraiMatt, samuraiFreyja);
                context.SaveChanges();
            }
        }

        private static void InsertSamurai()
        {
            var samurai = new Samurai() { Name = "Matt" };
            using(var context = new SamuraiContext()) {
                context.Samurais.Add(samurai);
                context.SaveChanges();
            }
        }
    }
}
