using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Enums
{
    public enum eUserStatus
    {
        [Description("Assente")]
        Absent = 1,
        [Description("Presente")]
        Present = 2,
        [Description("Uscito")]
        Left = 3
    }
}
