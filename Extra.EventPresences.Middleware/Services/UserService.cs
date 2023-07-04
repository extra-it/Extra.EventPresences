using Extra.EventPresences.DTO;
using Extra.EventPresences.DTO.Extentions;
using Extra.EventPresences.Middleware.Classes;
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
                GetUsersFilter filter = new GetUsersFilter(EventId);
                var result = UserManager.GetUsers(filter);
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

        public BaseResponseApiDto<UserDto> GetUserById(int UserId)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {
                var result = UserManager.GetUserById(UserId);
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

        public BaseResponseApiDto<FileDto> ExportUsers(int EventId)
        {
            BaseResponseApiDto<FileDto> retVal = new BaseResponseApiDto<FileDto>();
            retVal.Item = new FileDto();

            try
            {
                var friendlyname = StringExtensions.UrlNormalizeNoMinus("EventId" + EventId.ToString());
                retVal.Item.fileDownloadName = $"{friendlyname}-users.xlsx";
                retVal.Item.contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                GetUsersFilter filter = new GetUsersFilter(EventId);
                var UsersDto = UserManager.GetUsers(filter);

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

        public BaseResponseApiDto<UserDto> CheckIn(int UserId)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {
                var result = UserManager.CheckIn(UserId);
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

        public BaseResponseApiDto<List<UserDto>> CheckIn(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                var result = UserManager.CheckIn(UsersId);
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

        public BaseResponseApiDto<UserDto> CheckOut(int UserId)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {
                var result = UserManager.CheckOut(UserId);
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

        public BaseResponseApiDto<List<UserDto>> CheckOut(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                var result = UserManager.CheckOut(UsersId);
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

        public BaseResponseApiDto<UserDto> CancelCheckIn(int UserId)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {
                var result = UserManager.CancelCheckIn(UserId);
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
        public BaseResponseApiDto<List<UserDto>> CancelCheckIn(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                var result = UserManager.CancelCheckIn(UsersId);
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
        public BaseResponseApiDto<UserDto> CancelCheckOut(int UserId)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {
                var result = UserManager.CancelCheckOut(UserId);
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

        public BaseResponseApiDto<List<UserDto>> CancelCheckOut(List<int> UsersId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                var result = UserManager.CancelCheckOut(UsersId);
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

        public BaseResponseApiDto<List<UserDto>> GetCompanions(int UserId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                var result = UserManager.GetCompanions(UserId);
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

        public BaseResponseApiDto<UserDto> GetInvited(int InvitedID)
        {
            BaseResponseApiDto<UserDto> retVal = new BaseResponseApiDto<UserDto>();
            try
            {

                var result = UserManager.GetInvited(InvitedID);
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
                return retVal;
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }

        public BaseResponseApiDto<List<UserDto>> GetInviteds(int EventId)
        {
            BaseResponseApiDto<List<UserDto>> retVal = new BaseResponseApiDto<List<UserDto>>();
            try
            {
                GetUsersFilter filter = new GetUsersFilter(EventId);
                filter.UserType = eUserType.Invited;
                var result = UserManager.GetUsers(filter);
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
                return retVal;
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Status = eResponseStatus.Error;
                throw;
            }
            return retVal;
        }

        public BaseResponseApiDto<FileDto> GetQRCode(int UserId)
        {
            BaseResponseApiDto<FileDto> retVal = new BaseResponseApiDto<FileDto>();
            retVal.Item = new FileDto();

            try
            {
                var friendlyname = StringExtensions.UrlNormalizeNoMinus("UserId" + UserId.ToString());
                retVal.Item.fileDownloadName = $"{friendlyname}-qrcode.png";
                retVal.Item.contentType = "image/x-png";
                var memoryStream = UserManager.GetQRCode(UserId);
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
