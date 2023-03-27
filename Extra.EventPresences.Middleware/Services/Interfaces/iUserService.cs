using Extra.EventPresences.DTO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Services.Interfaces
{
    public interface iUserService
    {
        public BaseResponseApiDto<List<UserDto>> GetUsers(int EventId);
        public BaseResponseApiDto UpdateUser(UserDto User);
        public BaseResponseApiDto<PresenceStatisticsDto> GetPresenceStatistics(int EventId);

        public BaseResponseApiDto<UserDto> AddUser(UserDto User);

        BaseResponseApiDto<ExportUserDto> ExportUsers(int EventId);
    }
}
