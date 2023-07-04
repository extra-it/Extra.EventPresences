using Azure;
using Extra.EventPresences.DTO;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Middleware.Services.Interfaces;
using Extra.EventPresences.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Extra.EventPresences.API.Controllers
{

    [ApiController]
    [Route("v1/User/[action]")]
    public class UserController : ApiBaseController
    {

        private iUserService UserService;
        private IWebHostEnvironment _hostingEnvironment;
        public UserController(iLogManager logmanager, iUserService Userservice, IWebHostEnvironment environment) : base(logmanager)
        {
            UserService = Userservice;
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Returns the Users of a specific Event
        /// </summary>
        /// <param name="EventID">Event'ID</param>
        /// <returns></returns>
        [HttpGet]
        public BaseResponseApiDto<List<UserDto>> GetUsers(int EventID)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetUsers,
                Request = JsonConvert.SerializeObject(EventID)
            };
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                response = UserService.GetUsers(EventID);

                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Update a User
        /// </summary>
        /// <param name="userdto">User to update</param>
        /// <returns>Return the updated user</returns>
        [HttpPost]
        public BaseResponseApiDto UpdateUser(UserDto UserDto)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.UpdateUser,
                Request = JsonConvert.SerializeObject(UserDto)
            };
            BaseResponseApiDto response = new BaseResponseApiDto();
            try
            {
                response = UserService.UpdateUser(UserDto);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }


        /// <summary>
        /// Return the statistics of a Event
        /// </summary>
        /// <param name="EventID">Event'ID</param>
        /// <returns></returns>
        [HttpGet]
        public BaseResponseApiDto<PresenceStatisticsDto> GetPresenceStatistics(int EventID)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetPresenceStatistics,
                Request = JsonConvert.SerializeObject(EventID)
            };
            BaseResponseApiDto<PresenceStatisticsDto> response = new BaseResponseApiDto<PresenceStatisticsDto>();
            try
            {
                response= UserService.GetPresenceStatistics(EventID);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Add a user inside the system
        /// </summary>
        /// <param name="User">User to Add</param>
        /// <returns></returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> AddUser(UserDto User)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.AddUser,
                Request = JsonConvert.SerializeObject(User)
            };
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            try
            {
                response= UserService.AddUser(User);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }


        /// <summary>
        /// Apply a Check-in procedure to a User
        /// </summary>
        /// <param name="UserId">User to check-In</param>
        /// <returns>Return the updated user</returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> CheckIn(int UserId)
        {
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CheckIn,
                Request = JsonConvert.SerializeObject(UserId)
            };

            try
            {
                response = UserService.CheckIn(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Apply a Check-in procedure to a List of Users
        /// </summary>
        /// <param name="UsersId">User List to check-In</param>
        /// <returns>Return the updated users</returns>
        [HttpPost]
        public BaseResponseApiDto<List<UserDto>> CheckInMultiple(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CheckInMultiple,
                Request = JsonConvert.SerializeObject(UsersId)
            };

            try
            {
                response = UserService.CheckIn(UsersId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Cancel the Check-in procedure to a list of Users
        /// </summary>
        /// <param name="UsersId">ListUser of User to Cancel check-In</param>
        /// <returns>Return the updated user List</returns>
        [HttpPost]
        public BaseResponseApiDto<List<UserDto>> CancelCheckInMultiple(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CancelCheckInMultiple,
                Request = JsonConvert.SerializeObject(UsersId)
            };

            try
            {
                response = UserService.CancelCheckIn(UsersId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }


        /// <summary>
        /// Apply a Check-Out procedure to a User
        /// </summary>
        /// <param name="UserId">User to Check-Out</param>
        /// <returns>Return the updated user</returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> CheckOut(int UserId)
        {
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CheckOut,
                Request = JsonConvert.SerializeObject(UserId)
            };

            try
            {
                response = UserService.CheckOut(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Apply a Check-Out procedure to a List of Users
        /// </summary>
        /// <param name="Users">User List to check-Out</param>
        /// <returns>Return the updated users</returns>
        [HttpPost]
        public BaseResponseApiDto<List<UserDto>> CheckOutMultiple(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CheckOutMultiple,
                Request = JsonConvert.SerializeObject(UsersId)
            };

            try
            {
                response = UserService.CheckOut(UsersId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Apply a Cancel Check-In to a user
        /// </summary>
        /// <param name="User">User to Cancel the Check-In procedure</param>
        /// <returns>Returns the data of the updated users</returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> CancelCheckIn(int UserId)
        {
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CancelCheckIn,
                Request = JsonConvert.SerializeObject(UserId)
            };

            try
            {
                response = UserService.CancelCheckIn(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Apply a Cancel Check-Out to a user
        /// </summary>
        /// <param name="User">User to Cancel the Check-Out procedure</param>
        /// <returns>Returns the data of the updated users</returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> CancelCheckOut(int UserId)
        {
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CancelCheckOut,
                Request = JsonConvert.SerializeObject(UserId)
            };

            try
            {
                response = UserService.CancelCheckOut(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Cancel the Check-Out procedure to a list of Users
        /// </summary>
        /// <param name="UsersId">ListUser of User to Cancel check-Out</param>
        /// <returns>Return the updated user List</returns>
        [HttpPost]
        public BaseResponseApiDto<List<UserDto>> CancelCheckOutMultiple(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.CancelCheckOutMultiple,
                Request = JsonConvert.SerializeObject(UsersId)
            };

            try
            {
                response = UserService.CancelCheckOut(UsersId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Returns a user based on his ID
        /// </summary>
        /// <param name="UserId">User's ID to return</param>
        /// <returns>Return a user</returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> GetUserById(int UserId)
        {
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetUserById,
                Request = JsonConvert.SerializeObject(UserId)
            };

            try
            {
                response = UserService.GetUserById(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }


        /// <summary>
        /// Returns the Companion List of a Invited user
        /// </summary>
        /// <param name="User">Invited User</param>
        /// <returns>Return Companion List</returns>
        [HttpPost]
        public BaseResponseApiDto<List<UserDto>> GetCompanions(int UserId)
        {
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetCompanions,
                Request = JsonConvert.SerializeObject(UserId)
            };

            try
            {
                response = UserService.GetCompanions(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Returns the Invited user of a Companion user
        /// </summary>
        /// <param name="InvitedId">Companion User</param>
        /// <returns>Returns Invited User</returns>
        [HttpPost]
        public BaseResponseApiDto<UserDto> GetInvited(int InvitedId)
        {
            BaseResponseApiDto<UserDto> response = new BaseResponseApiDto<UserDto>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetInvited,
                Request = JsonConvert.SerializeObject(InvitedId)
            };

            try
            {
                response = UserService.GetInvited(InvitedId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }

        /// <summary>
        /// Returns the Invited user of a Companion user
        /// </summary>
        /// <param name="EventId">Companion User</param>
        /// <returns>Returns Invited User</returns>
        [HttpPost]
        public BaseResponseApiDto<List<UserDto>> GetInviteds(int EventId)
        {
            BaseResponseApiDto<List<UserDto>> response = new BaseResponseApiDto<List<UserDto>>();
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetInviteds,
                Request = JsonConvert.SerializeObject(EventId)
            };

            try
            {
                response = UserService.GetInviteds(EventId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
            }
            return response;
        }
        [HttpGet]
        public ActionResult ExportUsers(int EventId)
        {

            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.ExportUsers,
                Request = JsonConvert.SerializeObject(EventId)
            };
            BaseResponseApiDto<FileDto> response = new BaseResponseApiDto<FileDto>();
            try
            {
                response = UserService.ExportUsers(EventId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
                return null;
            }
            return File(response.Item.stream, response.Item.contentType, response.Item.fileDownloadName);
        }

        [HttpGet]
        public ActionResult GetQRCode(int UserId)
        {

            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetQRCode,
                Request = JsonConvert.SerializeObject(UserId)
            };
            BaseResponseApiDto<FileDto> response = new BaseResponseApiDto<FileDto>();
            try
            {
                response = UserService.GetQRCode(UserId);
                logDto.Status = response.Status;
                logDto.Message = response.Message;
                logDto.Response = JsonConvert.SerializeObject(response);
                LogDebug(logDto);
            }
            catch (Exception ex)
            {
                response = ManageException(ex, response, logDto);
                return null;
            }
            return File(response.Item.stream, response.Item.contentType, response.Item.fileDownloadName);
        }
    }
}
