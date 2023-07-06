using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extra.EventPresences.Model.Enums;

namespace Extra.EventPresences.Middleware.Managers.Interfaces
{
    public interface iFunctionalityManager
    {
        public BaseResponseDto<List<FunctionalityDto>> GetFunctionalitiesByEvent(int EventId);
        public BaseResponseDto<List<EventFunctionalityUserDto>> GetUserFunctionalities(int UserId);
        public BaseResponseDto<List<EventFunctionalityUserDto>> GetUsersFunctionalities(int EventId);
        public BaseResponseDto<int> GetEventFunctionalityNumOccurs(int EventId, eFunctionality functionality);


        public BaseResponseDto<int> GetEventFunctionalityAlreadyOccurs(int EventId, int UserId, eFunctionality functionality);

        public BaseResponseDto<EventFunctionalityUserDto> AddEventFunctionalityUser(EventFunctionalityUserDto eventFunctionalityUserDto);
    }
}
