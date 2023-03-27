using Extra.EventPresences.DTO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers.Interfaces
{
    public interface iUserManager
    {
        public BaseResponseDto<List<UserDto>> GetUsers(int EventId);

        public BaseResponseDto UpdateUser(UserDto User);

        public BaseResponseDto<PresenceStatisticsDto> GetPresenceStatistics(int EventId);

        public BaseResponseDto<UserDto> AddUser(UserDto User);

        public MemoryStream ExportUsersDto(List<UserDto> usersdto);
    }
}
