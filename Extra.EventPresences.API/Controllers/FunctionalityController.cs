using Extra.EventPresences.DTO;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Middleware.Services.Interfaces;
using Extra.EventPresences.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Extra.EventPresences.API.Controllers
{
    [ApiController]
    [Route("v1/Functionality/[action]")]
    public class FunctionalityController : ApiBaseController
    {

        private iFunctionalityService FunctionalityService;
        private IWebHostEnvironment _hostingEnvironment;
        public FunctionalityController(iLogManager logmanager, iFunctionalityService Functionalityservice, IWebHostEnvironment environment) : base(logmanager)
        {
            FunctionalityService = Functionalityservice;
            _hostingEnvironment = environment;
        }

        /// <summary>
        /// Returns the GetFunctionalities of a specific Event
        /// </summary>
        /// <param name="EventID">Event'ID</param>
        /// <returns></returns>
        [HttpGet]
        public BaseResponseApiDto<List<FunctionalityDto>> GetFunctionalities(int EventID)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetFunctionalities,
                Request = JsonConvert.SerializeObject(EventID)
            };
            BaseResponseApiDto<List<FunctionalityDto>> response = new BaseResponseApiDto<List<FunctionalityDto>>();
            try
            {
                response = FunctionalityService.GetFunctionalitiesByEvent(EventID);

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
        public BaseResponseApiDto SetUserFunctionality(EventFunctionalityUserDto EventFunctionalityUser)
        {
            var logDto = new WebApiLogDto()
            {
                Method = eApiMethod.GetFunctionalities,
                Request = JsonConvert.SerializeObject(EventFunctionalityUser)
            };
            BaseResponseApiDto response = new BaseResponseApiDto();
            try
            {
                response = FunctionalityService.SetUserFunctionality(EventFunctionalityUser);

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
        

    }
}
