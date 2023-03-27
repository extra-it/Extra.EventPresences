using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class PresenceStatisticsDto
    {
        public int UserTotalNum { get; set; }
        public int UserPresences { get; set; }
        public double UserPresencesPerc { get; set; }
    }
}
