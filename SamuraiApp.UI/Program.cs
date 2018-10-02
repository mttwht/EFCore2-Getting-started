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
            //PrePopulateSamuraisAndBattles();
            //JoinSamuraiAndBattle();
            //EnlistSamuraiInBattle();
            //EnlistSamuraiInBattleUntracked();
            AddNewSamuraiViaDisconnectedBattleObject();
        }

        private static void AddNewSamuraiViaDisconnectedBattleObject()
        {
            Battle battle;
            using(var newContext = new SamuraiContext()) {
                battle = newContext.Battles.Find(1);
            }
            var samurai = new Samurai { Name = "Matt" };
            battle.SamuraiBattles.Add(new SamuraiBattle { Samurai = samurai });
            _context.Battles.Attach(battle);
            _context.SaveChanges();
        }

        private static void EnlistSamuraiInBattleUntracked()
        {
            Battle battle;
            using( var newContext = new SamuraiContext() ) {
                battle = newContext.Battles.Find(1);
            }
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 2 });
            _context.Battles.Attach(battle);
            _context.ChangeTracker.DetectChanges(); // Not necessary; just to show debugging info
            _context.SaveChanges();
        }

        private static void EnlistSamuraiInBattle()
        {
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 3 });
            _context.SaveChanges();
        }

        private static void JoinSamuraiAndBattle()
        {
            var sb = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };
            _context.Add(sb);
            _context.SaveChanges();
        }

        private static void PrePopulateSamuraisAndBattles()
        {
            _context.AddRange(
                    new Samurai { Name = "Kikuchiyo" },
                    new Samurai { Name = "Makbei Shimada" },
                    new Samurai { Name = "Shichiroji" },
                    new Samurai { Name = "Katsushiro Okamoto" },
                    new Samurai { Name = "Heihachi Hayashida" },
                    new Samurai { Name = "Kyuzo" },
                    new Samurai { Name = "Gorobei Katayama" }
            );

            _context.AddRange(
                    new Battle { Name = "Battle of Okehazama",
                        StartDate = new DateTime(1560, 5, 1),
                        EndDate = new DateTime(1560, 6, 15)
                    },
                    new Battle { Name = "Battle of Shiroyama",
                        StartDate = new DateTime(1877, 9, 24),
                        EndDate = new DateTime(1877, 9, 24)
                    },
                    new Battle { Name = "Siege of Osaka",
                        StartDate = new DateTime(1614, 1, 1),
                        EndDate = new DateTime(1615, 12, 31)
                    },
                    new Battle { Name = "Boshin War",
                        StartDate = new DateTime(1868, 1, 1),
                        EndDate = new DateTime(1869, 1, 1)
                    }
                );

            _context.SaveChanges();
        }
    }
}
