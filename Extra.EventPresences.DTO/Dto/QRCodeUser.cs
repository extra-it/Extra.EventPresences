using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class QRCodeUser
    {
        public QRCodeUser(int userid) { UserID = userid; }
        public int UserID { get; set; }
    }
}
