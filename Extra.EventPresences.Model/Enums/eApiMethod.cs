using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Enums
{
    public enum eApiMethod
    {
        GetUsers = 50,
        UpdateUser = 51,
        GetPresenceStatistics = 52,
        ExportUsers = 53,
        AddUser = 54,
        CheckIn=55,
        CheckOut=56,
        CancelCheckIn=57,
        CancelCheckOut=58,
        GetCompanions=59,
        CheckInMultiple = 60,
        CheckOutMultiple = 61,
        GetInvited = 62,
        CancelCheckOutMultiple=63,
        CancelCheckInMultiple=64,
        GetUserById = 65,
        GetInviteds=66,
        GetQRCode=67,
        GetFunctionalities=68,
    }
}
