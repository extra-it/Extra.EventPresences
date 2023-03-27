using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class UserDto
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsPresent { get; set; }        
        public string? Company { get; set; }
        public string? Notes { get; set; }
        public bool Deleted { get; set; }
    }
}
