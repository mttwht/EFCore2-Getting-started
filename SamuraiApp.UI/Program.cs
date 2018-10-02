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

            // Many-to-many
            // Create
            //JoinSamuraiAndBattle();
            //EnlistSamuraiInBattle();
            //EnlistSamuraiInBattleUntracked();
            //AddNewSamuraiViaDisconnectedBattleObject();
            // Query
            //GetSamuraiWithBattles();
            // Modify
            //RemoveJoinBetweenSamuraiAndBattleSimple();
            //RemoveBattleFromSamurai();
            //RemoveBattleFromSamuraiDisconnected();

            // One-to-one
            // Create
            //AddNewSamuraiWithSecretIdentity();
            //AddSecretIdentityUsingSamuraiId();
            //AddSecretIdentityToExistingSamurai();
            //AddSecretIdentityToExistingSamuraiDisconnected();
            // Modify
            //EditSecretIdentity();
            //ReplaceSecretIdentity();
            //ReplaceSecretIdentityNotTracked();
            //ReplaceSecretIdentityNotInMemory();

            // Shadow properties
            CreateSamurai();
        }

        private static void CreateSamurai()
        {
            var samurai = new Samurai { Name = "Ronin" };
            _context.Samurais.Add(samurai);
            var timestamp = DateTime.Now;
            _context.Entry(samurai).Property("CreatedAt").CurrentValue = timestamp;
            _context.Entry(samurai).Property("UpdatedAt").CurrentValue = timestamp;
            _context.SaveChanges();
        }

        private static void ReplaceSecretIdentityNotInMemory()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.SecretIdentity != null);
            // Old secret identity is not tracked so this fails
            samurai.SecretIdentity = new SecretIdentity { RealName = "Matt" };
            _context.SaveChanges();
        }
        private static void ReplaceSecretIdentityNotTracked()
        {
            Samurai samurai;
            using(var newContext = new SamuraiContext()) {
                samurai = newContext.Samurais.Include(s => s.SecretIdentity)
                                             .FirstOrDefault(s => s.Id == 1);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Freyja" };
            // This will not know to delete the old secret identity!!
            _context.Samurais.Attach(samurai);
            _context.SaveChanges();
        }
        private static void ReplaceSecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                           .FirstOrDefault(s => s.Id == 1);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Matt" };
            _context.SaveChanges();
        }
        private static void EditSecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                           .FirstOrDefault(s => s.Id == 1);
            samurai.SecretIdentity.RealName = "M-dawg";
            _context.SaveChanges();
        }
        private static void AddSecretIdentityToExistingSamuraiDisconnected()
        {
            Samurai samurai;
            using(var newContext = new SamuraiContext()) {
                samurai = _context.Samurais.Find(3);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Matthew" };
            _context.Samurais.Attach(samurai);
            _context.SaveChanges();
        }
        private static void AddSecretIdentityToExistingSamurai()
        {
            var samurai = _context.Samurais.Find(2);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Matt" };
            _context.SaveChanges();
        }
        private static void AddSecretIdentityUsingSamuraiId()
        {
            // Only works once; when Samurai[1] does not yet have a SecretIdentity
            var identity = new SecretIdentity { SamuraiId = 1 };
            _context.Add(identity);
            _context.SaveChanges();
        }
        private static void AddNewSamuraiWithSecretIdentity()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void RemoveBattleFromSamuraiDisconnected()
        {
            // Goal is to remove join between Shichiroji(3) and Okehazama(1)
            Samurai samurai;
            using(var newContext = new SamuraiContext()) {
                samurai = newContext.Samurais.Include(s => s.SamuraiBattles)
                                             .ThenInclude(sb => sb.Battle)
                                             .SingleOrDefault(s => s.Id == 3);
            }
            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);

            // This doesn't work as the tracker won't be aware of the removed object
            //samurai.SamuraiBattles.Remove(sbToRemove);
            //_context.Attach(samurai);

            // This does work because we start tracking before removing the item, so not really disconnected anymore
            //_context.Attach(samurai);
            //samurai.SamuraiBattles.Remove(sbToRemove);

            // This makes the tracker aware of the change
            samurai.SamuraiBattles.Remove(sbToRemove);
            _context.Remove(sbToRemove);

            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }
        private static void RemoveBattleFromSamurai()
        {
            // Goal is to remove join between Shichiroji(3) and Okehazama(1)
            var samurai = _context.Samurais.Include(s => s.SamuraiBattles)
                                           .ThenInclude(sb => sb.Battle)
                                           .SingleOrDefault(s => s.Id == 3);
            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);
            samurai.SamuraiBattles.Remove(sbToRemove); // Remove via List<T>
            //_context.Remove(sbToRemove); // Remove via DbContext
            _context.ChangeTracker.DetectChanges(); // Only here for debugging
            _context.SaveChanges();
        }
        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 1, SamuraiId = 8 };
            _context.Remove(join);
            _context.SaveChanges();
        }
        private static void GetSamuraiWithBattles()
        {
            var samurai = _context.Samurais
                    .Include(s => s.SamuraiBattles)
                    .ThenInclude(sb => sb.Battle)
                    .FirstOrDefault(s => s.Id == 1);
            // get a single battle
            var battle = samurai.SamuraiBattles.First().Battle;
            // get all battles
            var allBattles = new List<Battle>();
            foreach(var sb in samurai.SamuraiBattles) {
                allBattles.Add(sb.Battle);
            }
            // good practice alternative:
            //allBattles = samurai.Battles();
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
