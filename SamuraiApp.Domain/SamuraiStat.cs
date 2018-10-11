using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamuraiApp.Domain
{
    public class SamuraiStat
    {
        public int SamuraiId { get; private set; }
        public string Name { get; private set; }
        public int NumberOfBattles { get; private set; }
        public DateTime EarliestBattle { get; private set; }
    }
}
