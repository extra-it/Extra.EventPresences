using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Entities
{
    [Table("FkGroups", Schema = "dbo")]
    public class FkGroups
    {
        [Key]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
