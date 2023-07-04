using Extra.EventPresences.DTO;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Model;
using Extra.EventPresences.Model.Entities;
using Extra.EventPresences.Model.Enums;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class LogManager : BaseManager, iLogManager
    {

        public LogManager(DBDataContext dbdatacontext) : base(dbdatacontext)
        {
        }

        protected eLogLevel Level
        {
            get
            {
                var Section = ConfigurationManager.GetInstance().GetSection("AppLogging");
                return Section.GetValue<eLogLevel>("Level");
            }
        }

        /// <summary>
        /// Save into database the Item Log
        /// </summary>
        /// <param name="itemDto"></param>
        /// <returns></returns>
        protected BaseResponseDto<long> Add(WebApiLogDto itemDto)
        {
            BaseResponseDto<long> response = new BaseResponseDto<long>();
            try
            {
                var objDB = MapperManager.GetMapper().Map<WebApiLog>(itemDto);
                objDB.DateInsert = DateTime.UtcNow;
                objDB.Deleted = false;
                DataContext.WebApiLogs.Add(objDB);
                DataContext.SaveChanges();
                response.Entity = objDB.ID;

            }
            catch (Exception ex)
            {
                HandleException(ex);
                response.Success = false;
                response.Exception = ex;
                response.Message = $"Exception in Login {ex.Message}";
            }
            return response;
        }

        public BaseResponseDto<long> LogDebug(WebApiLogDto itemDto)
        {
            BaseResponseDto<long> result = new BaseResponseDto<long>();
            if (Level <= eLogLevel.Debug)
            {
                itemDto.LogLevel = eLogLevel.Debug;
                result = Add(itemDto);
            }
            return result;
        }
        public BaseResponseDto<long> LogInfo(WebApiLogDto itemDto)
        {
            BaseResponseDto<long> result = new BaseResponseDto<long>();
            if (Level <= eLogLevel.Info)
            {
                itemDto.LogLevel = eLogLevel.Info;
                result = Add(itemDto);
            }
            return result;
        }
        public BaseResponseDto<long> LogWarn(WebApiLogDto itemDto)
        {
            BaseResponseDto<long> result = new BaseResponseDto<long>();
            if (Level <= eLogLevel.Warn)
            {
                itemDto.LogLevel = eLogLevel.Warn;
                result = Add(itemDto);
            }
            return result;
        }
        public BaseResponseDto<long> LogError(WebApiLogDto itemDto)
        {
            BaseResponseDto<long> result = new BaseResponseDto<long>();
            if (Level <= eLogLevel.Error)
            {
                itemDto.LogLevel = eLogLevel.Error;
                result = Add(itemDto);
            }
            return result;
        }
    }
}
