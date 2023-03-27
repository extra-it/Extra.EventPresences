using Azure;
using Extra.EventPresences.DTO.Dto;
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

        [HttpPost]
        public BaseResponseApiDto UpdateUser(UserDto userdto)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.UpdateUser,
                Request = JsonConvert.SerializeObject(userdto)
            };
            BaseResponseApiDto response = new BaseResponseApiDto();
            try
            {
                response = UserService.UpdateUser(userdto);
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

        [HttpGet]
        public ActionResult ExportUsers(int EventId)
        {

            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.ExportUsers,
                Request = JsonConvert.SerializeObject(EventId)
            };
            BaseResponseApiDto<ExportUserDto> response = new BaseResponseApiDto<ExportUserDto>();
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
    }
}
