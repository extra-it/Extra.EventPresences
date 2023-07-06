using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class EventFunctionalityDto
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public eFunctionality FunctionalityID { get; set; }

        public int NumOccurs { get; set; }
    }
}
