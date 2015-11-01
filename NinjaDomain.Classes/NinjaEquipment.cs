using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NinjaDomain.Classes
{
    public class NinjaEquipment : IModificationHistory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public EquipmentType Type { get; set; }
        [Required]
        public Ninja Ninja { get; set; }
        public int NinjaId { get; set; }


        public DateTime DateModified { get; set; }
        public DateTime DateCreated { get; set; }

    }
}
