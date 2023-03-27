using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Enums
{
    public enum eResponseStatus
    {
        Success = 10,
        ResponseStatus = 11,
        //ExpiredToken = 12,
        Exception = 13,
        Error = 14,
        NotProcessed = 15
    }
}
