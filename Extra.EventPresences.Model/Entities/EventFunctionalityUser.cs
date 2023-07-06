using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Entities
{
    [Table("EventFunctionalityUsers", Schema = "dbo")]
    public class EventFunctionalityUser
    {
        [Key]
        public int ID { get; set; }
        public int EventFunctionalityID { get; set; }
        public int UserID { get; set; }
        public DateTime OccursDateTime { get; set; }
    }
}
