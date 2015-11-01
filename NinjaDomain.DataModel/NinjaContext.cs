using NinjaDomain.Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaDomain.DataModel
{
    public class NinjaContext : DbContext, IDisposable
    {
       
        public DbSet<Ninja> Ninjas { get; set; }
        public DbSet<Clan> Clans { get; set; }
        public DbSet<NinjaEquipment> Equipment { get; set; }

        public override int SaveChanges()
        {
            foreach (var history in this.ChangeTracker.Entries().Where(e => e.Entity is IModificationHistory &&
                (e.State == EntityState.Added || e.State == EntityState.Modified )).Select(e=>e.Entity as IModificationHistory ))
            {
                history.DateModified = DateTime.Now;
                if (history.DateCreated == DateTime.MinValue) {
                    history.DateCreated = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}
