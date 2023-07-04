using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO
{
    public class PresenceStatisticsDto
    {

        /// <summary>
        /// Total Number of invite users
        /// </summary>
        public int UserInvitedTotalNum { get; set; }


        /// <summary>
        /// Number of users with a checkin datetime
        /// </summary>
        public int UserParticipants { get; set; }
        /// <summary>
        /// Number of users with a checkin datetime but not a checkout datetime
        /// </summary>
        public double UserParticipantsPerc { get; set; }

        /// <summary>
        /// % of  users with a checkin datetime but not a checkout datetime
        /// </summary>
        public int UserPresences { get; set; }
        public double UserPresencesPerc { get; set; }
    }
}
