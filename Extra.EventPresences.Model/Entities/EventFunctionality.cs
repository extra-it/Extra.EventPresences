using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Entities
{

    [Table("EventFunctionalities", Schema = "dbo")]
    public class EventFunctionality
    {
        [Key]
        public int ID { get; set; }
        public int EventID { get; set; }
        public int FunctionalityID { get; set; }

        public int NumOccurs { get; set; }
    }
}
