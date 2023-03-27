using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Model.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Extra.EventPresences.API.Controllers
{
    public class ApiBaseController : ControllerBase
    {
        private iLogManager LogManager = null;
        public ApiBaseController(iLogManager logmanager)
        {
            LogManager = logmanager;
        }
        //public UserDto CurrentUser { get; set; }
        // public CustomHeaderDto RequestHeader { get; set; }

        #region Exception Management

        protected BaseResponseApiDto ManageException(Exception ex, BaseResponseApiDto response, WebApiLogDto logDto)
        {
            try
            {
                logDto.Status = eResponseStatus.Exception;
                response.Status = eResponseStatus.Exception;
                response.Message = ex.Message;
                //logDto.SetResponse(response);
                LogError(logDto);
                response.Message = "An error occurs executing request";
            }
            catch (Exception) { }
            return response;
        }
        ///Dario: da ottimizzare per vedere se si riesce a fare un unico metodo
        protected BaseResponseApiDto ManageException<TResponse>(Exception ex, BaseResponseApiDto response, WebApiLogDto logDto)
        {
            try
            {
                if (response == null)
                {
                    response = new BaseResponseApiDto<TResponse>();
                }
                logDto.Status = eResponseStatus.Exception;
                response.Status = eResponseStatus.Exception;
                response.Message = ex.ToString();
                logDto.SetResponse(response);
                LogError(logDto);
                response.Message = "An error occurs executing request";
            }
            catch (Exception) { }
            return response;
        }
        protected BaseResponseApiDto<TResponse> ManageException<TResponse, TLogDto>(Exception ex, BaseResponseApiDto<TResponse> response, WebApiLogDto<TLogDto> logDto)
        {
            try
            {
                if (response == null)
                {
                    response = new BaseResponseApiDto<TResponse>();
                }
                logDto.Status = eResponseStatus.Exception;
                response.Status = eResponseStatus.Exception;
                response.Message = ex.ToString();
                logDto.SetResponse(response);
                LogError(logDto);
                response.Message = "An error occurs executing request";
            }
            catch (Exception) { }
            return response;
        }
        protected BaseResponseApiDto ManageException<TLogDto>(Exception ex, BaseResponseApiDto response, WebApiLogDto<TLogDto> logDto)
        {
            try
            {
                if (response == null)
                {
                    response = new BaseResponseApiDto();
                }
                response.Status = eResponseStatus.Exception;
                response.Message = ex.ToString();
                logDto.SetResponse(response);
                LogError(logDto);
                response.Message = "An error occurs executing request";
            }
            catch (Exception) { }
            return response;
        }
        protected BaseResponseApiDto<TResponse> ManageException<TResponse>(Exception ex, BaseResponseApiDto<TResponse> response, WebApiLogDto logDto)
        {
            try
            {
                if (response == null)
                {
                    response = new BaseResponseApiDto<TResponse>();
                }
                response.Status = eResponseStatus.Exception;
                response.Message = ex.ToString();
                logDto.SetResponse(response);
                LogError(logDto);
                response.Message = "An error occurs executing request";
            }
            catch (Exception) { }
            return response;
        }

        #endregion
        protected void LogDebug(WebApiLogDto logDto)
        {
            logDto.EndDate = DateTime.UtcNow;
            LogManager.LogDebug(logDto);
        }
        protected void LogInfo(WebApiLogDto logDto)
        {
            logDto.EndDate = DateTime.UtcNow;
            LogManager.LogInfo(logDto);
        }
        protected void LogWarn(WebApiLogDto logDto)
        {
            logDto.EndDate = DateTime.UtcNow;
            LogManager.LogWarn(logDto);
        }
        protected void LogError(WebApiLogDto logDto)
        {
            logDto.EndDate = DateTime.UtcNow;
            LogManager.LogError(logDto);
        }
    }
}
