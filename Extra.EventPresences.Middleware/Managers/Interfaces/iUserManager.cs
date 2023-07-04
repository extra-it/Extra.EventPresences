using Extra.EventPresences.DTO;
using Extra.EventPresences.Middleware.Classes;
using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers.Interfaces
{
    public interface iUserManager
    {
        public BaseResponseDto<UserDto> GetUserById(int UserId);

        public BaseResponseDto<List<UserDto>> GetUsers(GetUsersFilter Filter);

        public BaseResponseDto<UserDto> UpdateUser(UserDto userdto);

        public BaseResponseDto<PresenceStatisticsDto> GetPresenceStatistics(int EventId);

        public BaseResponseDto<UserDto> AddUser(UserDto User);

        public MemoryStream ExportUsersDto(List<UserDto> UsersId);

        public BaseResponseDto<UserDto> CheckIn(int UserId);

        public BaseResponseDto<List<UserDto>> CheckIn(List<int> UsersId);

        public BaseResponseDto<UserDto> CheckOut(int UserId);

        public BaseResponseDto<List<UserDto>> CheckOut(List<int> UsersId);

        public BaseResponseDto<UserDto> CancelCheckIn(int UserId);
        public BaseResponseDto<UserDto> CancelCheckOut(int UserId);

        public BaseResponseDto<List<UserDto>> CancelCheckIn(List<int> UsersId);
        public BaseResponseDto<List<UserDto>> CancelCheckOut(List<int> UsersId);
        public BaseResponseDto<List<UserDto>> GetCompanions(int UserId);
        public BaseResponseDto<UserDto> GetInvited(int InvitedID);

        public MemoryStream GetQRCode(int UserId);
    }
}
