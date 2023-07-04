using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Classes
{
    public class GetUsersFilter
    {
        public GetUsersFilter() { }

        public GetUsersFilter(int EventId)
        {
            this.EventId = EventId;
        }

        public int EventId { get; set; }
        public eUserType? UserType { get; set; }

        public int InvitedID { get; set; }
    }
}
