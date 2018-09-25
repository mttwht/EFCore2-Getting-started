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
            InsertSamurai();
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
