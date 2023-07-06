using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class EventFunctionalityUserDto
    {
        public int EventId { get; set; }
        public int UserId { get; set; }
        public eFunctionality Functionality { get; set; }

        public DateTime OccursDateTime{ get; set; }
}
}
