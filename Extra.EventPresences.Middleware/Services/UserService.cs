using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Middleware.Extentions;
using Extra.EventPresences.Middleware.Managers;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Middleware.Services.Interfaces;
using Extra.EventPresences.Model.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Extra.EventPresences.Middleware.Services
{
    public class UserService : iUserService
    {
        private iUserManager UserManager = null;
        private iConfigurationManager configurationManager = null;

        public UserService(iUserManager usermanager, iConfigurationManager configurationmanager)
        {
            UserManager = usermanager;
            configurationManager = configurationmanager;
        }
        public BaseResponseApiDto<List<UserDto>> GetUsers(int EventId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                var result = UserManager.GetUsers(EventId);
                if (result.Success)
                {
                    retVal.Item = result.Entity;
                    retVal.Status = eResponseStatus.Success;
                }
                else
                {
                    retVal.Message = result.Message;
                    retVal.Status = eResponseStatus.Error;
                }
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }

        public BaseResponseApiDto UpdateUser(UserDto User)
        {
            BaseResponseApiDto retVal = new BaseResponseApiDto();
            try
            {
                var result = UserManager.UpdateUser(User);
                if (result.Success)
                {
                    retVal.Status = eResponseStatus.Success;
                    retVal.Message = "Utente aggiornato con successo";
                }
                else
                {
                    retVal.Message = result.Message;
                    retVal.Status = eResponseStatus.Error;
                }
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }

        public BaseResponseApiDto<PresenceStatisticsDto> GetPresenceStatistics(int EventId)
        {
            BaseResponseApiDto<PresenceStatisticsDto> retVal = new BaseResponseApiDto<PresenceStatisticsDto>();
            try
            {
                var result = UserManager.GetPresenceStatistics(EventId);
                if (result.Success)
                {
                    retVal.Item = result.Entity;
                    retVal.Status = eResponseStatus.Success;
                }
                else
                {
                    retVal.Message = result.Message;
                    retVal.Status = eResponseStatus.Error;
                }
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }
        public BaseResponseApiDto<UserDto> AddUser(UserDto User)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {
                var result = UserManager.AddUser(User);
                if (result.Success)
                {
                    retVal.Item = result.Entity;
                    retVal.Status = eResponseStatus.Success;
                }
                else
                {

                    retVal.Status = eResponseStatus.Error;
                }
                retVal.Message = result.Message;
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }

        public BaseResponseApiDto<ExportUserDto> ExportUsers(int EventId)
        {
            BaseResponseApiDto<ExportUserDto> retVal = new BaseResponseApiDto<ExportUserDto>();
            retVal.Item=new ExportUserDto();

            try
            {
                var friendlyname = StringExtensions.UrlNormalizeNoMinus("EventId" + EventId.ToString());
                retVal.Item.fileDownloadName = $"{friendlyname}-users.xlsx";
                retVal.Item.contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var UsersDto = UserManager.GetUsers(EventId);

                var memoryStream = UserManager.ExportUsersDto(UsersDto.Entity);
                retVal.Item.stream = memoryStream.ToArray();
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }
    }
}
