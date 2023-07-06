using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Entities
{
    [Table("Users", Schema = "dbo")]
    public class User
    {
        [Key]
        public int ID { get; set; }
        public int EventID { get; set; }
        public string? Firstname { get; set; }
        public string Lastname { get; set; }
        public string? Email { get; set; }

        public string? Company { get; set; }
        public string? Notes { get; set; }

        //public DateTime? CheckInDatetime { get; set; }
        //public DateTime? CheckOutDatetime { get; set; }
        public int StatusId { get; set; }

        public DateTime DateInsert { get; set; }
        public DateTime? DateUpdate { get; set; }

        public int? InvitedID { get; set; }
        public bool Deleted { get; set; }
    }
}
