using NinjaDomain.Classes;
using NinjaDomain.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new NullDatabaseInitializer<NinjaContext>());
            //*** Inserts
            //InsertNinja();
            //SimpleNinjaQueries();
            //FindAName();
            //FindByDateOfBirth();


            //*** Updates to Database
            //QueryAndUpdateNinja();
            //QueryAndUpdateNinjaDisconnected();

            //*** Find
           //QueryForFind();

            //*** Deleting
            DeleteNinja();

            //*** SingleOrDefault demands that there is only records in the results. It does select top(2) rows if 2 rows are found then
            //query fails. Where as FirstOrDefault is different


            //*** Inserting Related Data
            //InsertNinjaWithEquipment();

            //*** Graph queries
            //SimpleNinjaGraphQueries();
            //SimpleNinjaGraphQueriesLazy();

            //*** Projection queries
            ProjectQuery();

            Console.WriteLine("We made a console app");
            Console.ReadKey();
        }

        private static void ProjectQuery()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninjas = context.Ninjas
                               .Select(x => new { x.Name, x.DateOfBirth, x.EquipmentOwned, x.Clan.ClanName }).ToList();
            }
        }

        // Eagar loading
        private static void SimpleNinjaGraphQueries()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Include(x => x.EquipmentOwned).FirstOrDefault(x => x.Name.StartsWith("Kacy"));
            }
        }

        // Lazy loading
        private static void SimpleNinjaGraphQueriesLazy()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault(x => x.Name.StartsWith("Kacy"));
                Console.WriteLine("Ninja Retrieved = " + ninja.Name);
                //Now load the related data
                //context.Entry(ninja).Collection(n => n.EquipmentOwned).Load();
                Console.WriteLine("Ninja Equipment Count = " + ninja.EquipmentOwned.Count());
            }
        }

        private static void InsertNinjaWithEquipment()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = new Ninja
                {
                  Name = "Kacy Catanzaro" , ServedInOniwaban = true, DateOfBirth = new DateTime (1975,7,2), ClanId = 1                  
                };
                var muscles = new NinjaEquipment { Name = "Muscles", Type = EquipmentType.Blade };
                var spunk = new NinjaEquipment { Name = "Spunk", Type = EquipmentType.Sword };

                context.Ninjas.Add(ninja);
                ninja.EquipmentOwned.Add(muscles);
                ninja.EquipmentOwned.Add(spunk);
                context.SaveChanges();
            }
        }

        private static void DeleteNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var n = context.Ninjas.FirstOrDefault();
                context.Entry(n).State = EntityState.Modified; 
                context.Ninjas.Remove(n);
                context.SaveChanges();
            }
        }

        private static void QueryForFind()
        {
            using (var context = new NinjaContext())
            {
                //Do find 2 times. The query is run only one time since context has it in memory
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.Find(4);
                Console.WriteLine("Find#1 " + ninja.Name);

                //Doesn't shoot another query
                var Someninja = context.Ninjas.Find(4);
                Console.WriteLine("Find#2 " + Someninja.Name);
            }
        }

        private static void QueryAndUpdateNinjaDisconnected()
        {
            Ninja n;
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                n = context.Ninjas.FirstOrDefault();
            }

            n.ServedInOniwaban = (!n.ServedInOniwaban);

            //Notice you have a new context so its not aware of any tracking
            //Hence you can use Add, but that will add regardless
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Add(n); //This is add regadless though
                context.Ninjas.Attach(n);
                context.Entry(n).State = EntityState.Modified;  //But that will pass all the values
                context.SaveChanges();
            }

        }

        /// <summary>
        /// UPDATE DOESNT DO ANYTHING HERE, but when you flip the flag it does
        /// </summary>
        private static void QueryAndUpdateNinja()
        {
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                var ninja = context.Ninjas.FirstOrDefault();
                ninja.ServedInOniwaban = (!ninja.ServedInOniwaban);
                context.SaveChanges();
            }
        }



        /// <summary>
        /// THIS WILL RETURN NULL or FIRST IF YOU USE FIRST
        /// </summary>
        private static void FindByDateOfBirth()
        {
            using (var context = new NinjaContext())
            {
                //FirstOrDefault will retrun a null if there are no results
                var ninjas = context.Ninjas.Where(x => x.DateOfBirth == new DateTime(1980,1,1)).FirstOrDefault();

                //This will throw exception if there are no results
                // var ninjas = context.Ninjas.Where(x => x.DateOfBirth == new DateTime(1980, 1, 1)).First();
            }
        }
        
        //
        //
        /// <summary>
        /// QUERY
        /// </summary>
        private static void SimpleNinjaQueries()
        {
            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.ToList();
            }
        }
        /// <summary>
        /// FINDS ALL THE NAMES THAT CONTAIN SHAN
        /// </summary>
        private static void FindAName()
        {
            using (var context = new NinjaContext())
            {
                var ninjas = context.Ninjas.Where(x=>x.Name.Contains("Shan"));
            }
        }

        
        //
        //   INSERT
        //
        private static void InsertNinja()
        {
            var ninja = new Ninja
            {
                Name = "TerriSan", ServedInOniwaban = false, DateOfBirth = new DateTime(1980,1,1), ClanId = 1
            };

            var ninja2 = new Ninja
            {
                Name = "JulieLerman",  ServedInOniwaban = false, DateOfBirth = new DateTime(1970, 1, 1), ClanId = 1
            };

            //now lets use EF to insert this object
            using (var context = new NinjaContext())
            {
                context.Database.Log = Console.WriteLine;
                //context.Ninjas.Add(ninja);
                //context.Ninjas.Add(ninja2);
                context.Ninjas.AddRange(new List<Ninja> { ninja, ninja2 });
                context.SaveChanges();
            }
        }



    }
}
