using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Services.Interfaces
{
    public interface iFunctionalityService
    {
        public BaseResponseApiDto<List<FunctionalityDto>> GetFunctionalitiesByEvent(int EventId);

        public BaseResponseApiDto SetUserFunctionality(EventFunctionalityUserDto EventFunctionalityUser);
    }
}
