using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Entities
{

    [Table("Functionalities", Schema = "dbo")]
    public class Functionality
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }    

        public DateTime DateInsert { get; set; }
        public DateTime? DateUpdate { get; set; }

        public int? InvitedID { get; set; }
        public bool Deleted { get; set; }
    }
}