using Extra.EventPresences.DTO;
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

        public BaseResponseApiDto<UserDto> GetUserById(int UserId);

        public BaseResponseApiDto UpdateUser(UserDto User);
        public BaseResponseApiDto<PresenceStatisticsDto> GetPresenceStatistics(int EventId);

        public BaseResponseApiDto<UserDto> AddUser(UserDto User);

        BaseResponseApiDto<FileDto> ExportUsers(int EventId);

        public BaseResponseApiDto<UserDto> CheckIn(int UserId);
        public BaseResponseApiDto<List<UserDto>> CheckIn(List<int> UsersId);

        public BaseResponseApiDto<UserDto> CheckOut(int UserId);

        public BaseResponseApiDto<List<UserDto>> CheckOut(List<int> UsersId);

        public BaseResponseApiDto<UserDto> CancelCheckIn(int UserId);
        public BaseResponseApiDto<UserDto> CancelCheckOut(int UserId);

        public BaseResponseApiDto<List<UserDto>> CancelCheckIn(List<int> UsersId);
        public BaseResponseApiDto<List<UserDto>> CancelCheckOut(List<int> UsersId);

        public BaseResponseApiDto<List<UserDto>> GetCompanions(int UserId);
        public BaseResponseApiDto<UserDto> GetInvited(int InvitedId);
        public BaseResponseApiDto<List<UserDto>> GetInviteds(int InvitedId);

        public BaseResponseApiDto<FileDto> GetQRCode(int UserId);

    }
}
