using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NinjaDomain.Classes
{
    public class Clan
    {
        public Clan()
        {
            Ninjas = new List<Ninja>();
        }
        public int Id { get; set; }
        public string ClanName { get; set; }
        public List<Ninja> Ninjas { get; set; }
    }
}
