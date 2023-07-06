using Azure;
using Extra.EventPresences.DTO;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.DTO.Enums;
using Extra.EventPresences.Middleware.Classes;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Model;
using Extra.EventPresences.Model.Entities;
using Extra.EventPresences.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NPOI.OpenXmlFormats.Wordprocessing;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.XSSF.UserModel;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class UserManager : BaseManager, iUserManager
    {
        FunctionalityManager functionalityManager;
        public UserManager(DBDataContext dbdatacontext) : base(dbdatacontext)
        {
            functionalityManager = new FunctionalityManager(dbdatacontext);
        }
        public BaseResponseDto<UserDto> GetUserById(int UserId)
        {
            var retVal = new BaseResponseDto<UserDto>();
            retVal.Entity = new UserDto();
            try
            {

                var user = DataContext.Users.FirstOrDefault(x => x.ID == UserId && !x.Deleted);
                if (user != null)
                {
                    var TempDto = MapperManager.GetMapper().Map<UserDto>(user);
                    var setCheckResult = SetCheckInOutDateTime(TempDto);
                    if (setCheckResult.Success)
                    {
                        retVal.Entity = setCheckResult.Entity;
                        retVal.Message = "Procedura eseguita correttamente";
                        retVal.Success = true;
                    }
                    else
                    {
                        retVal.Message = setCheckResult.Message;
                        retVal.Success = false;
                    }
                }
                else
                {
                    retVal.Message = "Utente non trovato!";
                    retVal.Success = false;
                }
            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }

        public BaseResponseDto<List<UserDto>> GetUsers(GetUsersFilter Filter)
        {
            var retVal = new BaseResponseDto<List<UserDto>>();
            retVal.Entity = new List<UserDto>();
            List<UserDto> tmpList = new List<UserDto>();
            try
            {
                IQueryable<User> tempResult = DataContext.Users.Where(x => !x.Deleted);

                if (Filter.EventId != 0)
                {
                    tempResult = tempResult.Where(x => x.EventID == Filter.EventId);
                }
                if (Filter.UserType != null)
                {
                    if (Filter.UserType == eUserType.Invited)
                    {
                        tempResult = tempResult.Where(x => x.InvitedID == null);
                    }
                    else
                    {
                        tempResult = tempResult.Where(x => x.InvitedID != null);
                    }
                }
                if (Filter.InvitedID != 0)
                {
                    tempResult = tempResult.Where(x => x.InvitedID == Filter.InvitedID);
                }

                foreach (User user in tempResult.ToList())
                {
                    tmpList.Add(MapperManager.GetMapper().Map<UserDto>(user));
                }
                var SetCheckResult = SetCheckInOutDateTime(tmpList);
                if (SetCheckResult.Success)
                {
                    retVal.Entity = SetCheckResult.Entity;
                }
                else
                {
                    retVal.Success = false;
                    retVal.Message = SetCheckResult.Message;
                }


                //Ordino la lista per il campo Name
                retVal.Entity = retVal.Entity.OrderBy(x => x.Name).ToList();
                retVal.Message = "Procedura eseguita correttamente";
                retVal.Success = true;

            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }

        public BaseResponseDto<UserDto> UpdateUser(UserDto userdto)
        {
            var retVal = new BaseResponseDto<UserDto>();
            try
            {
                string check = CheckUser(userdto);
                if (!string.IsNullOrEmpty(check))
                {
                    retVal.Message = check;
                    retVal.Success = false;
                }
                else
                {
                    var objDB = DataContext.Users.FirstOrDefault(x => x.ID == userdto.ID && !x.Deleted);
                    if (objDB == null)
                    {
                        retVal.Message = "Utente non trovato con l'ID (" + userdto.ID.ToString() + ") ricevuto";
                        retVal.Success = false;
                    }
                    else
                    {
                        objDB = MapperManager.GetMapper().Map<UserDto, User>(userdto, objDB);
                        objDB.DateUpdate = DateTime.UtcNow;
                        DataContext.Entry(objDB).State = EntityState.Modified;
                        DataContext.SaveChanges();
                        retVal = GetUserById(objDB.ID);
                        retVal.Message = "Utente aggiornato con successo";
                        retVal.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }
        public BaseResponseDto<PresenceStatisticsDto> GetPresenceStatistics(int EventId)
        {
            var retVal = new BaseResponseDto<PresenceStatisticsDto>();
            try
            {
                retVal.Entity = new PresenceStatisticsDto();

                var EventUsers = DataContext.Users.Where(x => x.EventID == EventId && !x.Deleted).ToList();
                if (EventUsers.Count() > 0)
                {
                    retVal.Entity.UserInvitedTotalNum = EventUsers.Count();
                    retVal.Entity.UserPresences = EventUsers.Where(x => x.StatusId == (int)eUserStatus.Present).ToList().Count();
                    retVal.Entity.UserPresencesPerc = Math.Round((double)retVal.Entity.UserPresences / (double)retVal.Entity.UserInvitedTotalNum * 100.0, 2);
                    retVal.Entity.UserParticipants = EventUsers.Where(x => x.StatusId == (int)eUserStatus.Present || x.StatusId == (int)eUserStatus.Left).ToList().Count();
                    retVal.Entity.UserParticipantsPerc = Math.Round((double)retVal.Entity.UserParticipants / (double)retVal.Entity.UserInvitedTotalNum * 100.0, 2);
                }
                retVal.Message = "Statistiche estratte correttamente";
                retVal.Success = true;

            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }


        public BaseResponseDto<UserDto> AddUser(UserDto UserDto)
        {
            var retVal = new BaseResponseDto<UserDto>();
            IDbContextTransaction tran = null;
            try
            {
                string check = CheckUser(UserDto);
                if (!string.IsNullOrEmpty(check))
                {
                    retVal.Message = check;
                    retVal.Success = false;
                }
                else
                {

                    ///Setto lo stato a presente e valorizzo l'orario di checkin!
                    UserDto.StatusId = eUserStatus.Present;

                    var objDB = MapperManager.GetMapper().Map<User>(UserDto);
                    objDB.DateInsert = DateTime.UtcNow;

                    tran = DataContext.Database.BeginTransaction();
                    DataContext.Users.Add(objDB);
                    DataContext.SaveChanges();

                    EventFunctionalityUserDto EFUDto = new EventFunctionalityUserDto();
                    EFUDto.UserId = objDB.ID;
                    EFUDto.EventId = objDB.EventID;
                    EFUDto.OccursDateTime = DateTime.UtcNow;
                    EFUDto.Functionality = eFunctionality.CheckIn;
                    var AddEventFunctionalityUserResult = functionalityManager.AddEventFunctionalityUser(EFUDto);
                    if (AddEventFunctionalityUserResult.Success)
                    {
                        var getUserByIDResult = GetUserById(objDB.ID);
                        if (getUserByIDResult.Success)
                        {
                            retVal.Entity = getUserByIDResult.Entity;
                            retVal.Message = "Utente inserito correttamente";
                            retVal.Success = true;
                            tran.Commit();
                        }
                        else
                        {
                            retVal.Message = "La procedura non è stata eseguita correttamente. Operazione annullata!";
                            retVal.Success = false;
                            tran.Rollback();
                        }
                    }
                    else
                    {
                        tran.Rollback();
                        retVal.Message = AddEventFunctionalityUserResult.Message;
                        retVal.Success = false;
                    }
                }
            }
            catch (Exception ex)
            {
                if (tran != null)
                {
                    tran.Rollback();
                }
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }


        /// <summary>
        /// Makes the check-in procedure for a specific user
        /// </summary>
        /// <param name="userdto"></param>
        /// <returns></returns>
        public BaseResponseDto<UserDto> CheckIn(int UserId)
        {

            return CheckInOut(UserId, eCheckType.CheckIn);
        }

        public BaseResponseDto<List<UserDto>> CheckIn(List<int> UsersId)
        {

            return CheckInOut(UsersId, eCheckType.CheckIn);
        }

        /// <summary>
        /// Makes the check-out procedure for a specific user
        /// </summary>
        /// <param name="userdto"></param>
        /// <returns></returns>
        public BaseResponseDto<UserDto> CheckOut(int UserId)
        {

            return CheckInOut(UserId, eCheckType.CheckOut);
        }

        public BaseResponseDto<List<UserDto>> CheckOut(List<int> UsersId)
        {

            return CheckInOut(UsersId, eCheckType.CheckOut);
        }

        /// <summary>
        /// Makes the check-in procedure for a specific user
        /// </summary>
        /// <param name="userdto"></param>
        /// <returns></returns>
        public BaseResponseDto<UserDto> CancelCheckIn(int UserId)
        {

            return CancelCheckInOut(UserId, eCheckType.CheckIn);
        }

        public BaseResponseDto<List<UserDto>> CancelCheckIn(List<int> UsersId)
        {

            return CancelCheckInOut(UsersId, eCheckType.CheckIn);
        }

        /// <summary>
        /// Makes the check-out procedure for a specific user
        /// </summary>
        /// <param name="userdto"></param>
        /// <returns></returns>
        public BaseResponseDto<UserDto> CancelCheckOut(int UserId)
        {

            return CancelCheckInOut(UserId, eCheckType.CheckOut);
        }


        public BaseResponseDto<List<UserDto>> CancelCheckOut(List<int> UsersId)
        {

            return CancelCheckInOut(UsersId, eCheckType.CheckOut);
        }

        private BaseResponseDto<List<UserDto>> CheckInOut(List<int> UsersId, eCheckType CheckType)
        {
            var retVal = new BaseResponseDto<List<UserDto>>();
            List<UserDto> UsersDtoToReturn = new List<UserDto>();
            IDbContextTransaction tran = null;
            try
            {
                tran = DataContext.Database.BeginTransaction();
                bool allok = true;
                foreach (int UserId in UsersId)
                {
                    var ret = CheckInOut(UserId, CheckType,false);
                    if (!ret.Success)
                    {
                        tran.Rollback();
                        retVal.Success = false;
                        retVal.Message = ret.Message;
                        allok = false;
                        break;
                    }
                    else
                    {
                        UsersDtoToReturn.Add(ret.Entity);
                    }
                }
                if (allok)
                {
                    tran.Commit();
                    retVal.Entity = UsersDtoToReturn;
                    if (CheckType == eCheckType.CheckIn)
                    {
                        retVal.Message = "Check-in effettuato correttamente";
                    }
                    else
                    {
                        retVal.Message = "Check-out effettuato correttamente";
                    }
                    retVal.Success = true;
                }
            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                if (tran != null)
                {
                    tran.Rollback();
                }
            }
            return retVal;
        }

        private BaseResponseDto<List<UserDto>> CancelCheckInOut(List<int> UsersId, eCheckType CheckType)
        {
            var retVal = new BaseResponseDto<List<UserDto>>();
            List<UserDto> UsersDtoToReturn = new List<UserDto>();
            IDbContextTransaction tran = null;
            try
            {
                tran = DataContext.Database.BeginTransaction();
                bool allok = true;
                foreach (int UserId in UsersId)
                {
                    var ret = CancelCheckInOut(UserId, CheckType,false);
                    if (!ret.Success)
                    {
                        tran.Rollback();
                        retVal.Success = false;
                        retVal.Message = ret.Message;
                        allok = false;
                        break;
                    }
                    else
                    {
                        UsersDtoToReturn.Add(ret.Entity);
                    }
                }
                if (allok)
                {
                    tran.Commit();
                    retVal.Entity = UsersDtoToReturn;
                    if (CheckType == eCheckType.CheckIn)
                    {
                        retVal.Message = "Cancel Check-in effettuato correttamente";
                    }
                    else
                    {
                        retVal.Message = "Cancel Check-out effettuato correttamente";
                    }
                    retVal.Success = true;
                }
            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                if (tran != null)
                {
                    tran.Rollback();
                }
            }
            return retVal;
        }

        private BaseResponseDto<UserDto> CheckInOut(int UserId, eCheckType CheckType, bool LocalTran = true)
        {
            var retVal = new BaseResponseDto<UserDto>();
            IDbContextTransaction tran = null;
            try
            {
                var ret = GetUserById(UserId);
                if (!ret.Success)
                {
                    retVal.Success = false;
                    retVal.Message = ret.Message;
                }
                else
                {
                    var GetEventFunctionalityResult = functionalityManager.GetEventFunctionality(ret.Entity.EventID, (CheckType == eCheckType.CheckIn ? eFunctionality.CheckIn : eFunctionality.CheckOut));
                    if (GetEventFunctionalityResult.Success)
                    {

                        EventFunctionalityUserDto EFUDto = new EventFunctionalityUserDto();
                        EFUDto.OccursDateTime = DateTime.UtcNow;
                        EFUDto.UserId = UserId;
                        EFUDto.EventId = GetEventFunctionalityResult.Entity.EventID;
                        EFUDto.Functionality = GetEventFunctionalityResult.Entity.FunctionalityID;
                        EFUDto.OccursDateTime = DateTime.UtcNow;
                        if (LocalTran)
                        {
                            tran = DataContext.Database.BeginTransaction();
                        }

                        var AddEventFunctionalityUserDtoResult = functionalityManager.AddEventFunctionalityUser(EFUDto);
                        if (AddEventFunctionalityUserDtoResult.Success)
                        {
                            if (CheckType == eCheckType.CheckIn)
                            {
                                ret.Entity.CheckInDatetime = AddEventFunctionalityUserDtoResult.Entity.OccursDateTime;
                                ret.Entity.StatusId = eUserStatus.Present;
                            }
                            else
                            {
                                ret.Entity.CheckOutDatetime = AddEventFunctionalityUserDtoResult.Entity.OccursDateTime; ;
                                ret.Entity.StatusId = eUserStatus.Left;
                            }
                            retVal = UpdateUser(ret.Entity);
                            if (retVal.Success)
                            {
                                if (LocalTran)
                                {
                                    tran.Commit();
                                }
                                if (CheckType == eCheckType.CheckIn)
                                {
                                    retVal.Message = "Check-in effettuato correttamente";
                                }
                                else
                                {
                                    retVal.Message = "Check-out effettuato correttamente";
                                }
                                retVal.Success = true;
                            }
                            else
                            {
                                if (LocalTran)
                                {
                                    tran.Rollback();
                                }
                            }
                        }
                        else
                        {
                            if (LocalTran)
                            {
                                tran.Rollback();
                            }
                            retVal.Success = false;
                            retVal.Message = AddEventFunctionalityUserDtoResult.Message;
                        }
                    }
                    else
                    {
                        retVal.Success = false;
                        retVal.Message = GetEventFunctionalityResult.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                if (LocalTran && tran != null)
                {
                    tran.Rollback();
                }
            }
            return retVal;
        }

        private BaseResponseDto<UserDto> CancelCheckInOut(int UserId, eCheckType CheckType, bool LocalTran = true)
        {
            var retVal = new BaseResponseDto<UserDto>();
            IDbContextTransaction tran = null;
            try
            {
                var ret = GetUserById(UserId);
                if (!ret.Success)
                {
                    retVal.Success = false;
                    retVal.Message = ret.Message;
                }
                else
                {
                    EventFunctionalityUserDto eventFunctionalityUserDto = new EventFunctionalityUserDto();
                    eventFunctionalityUserDto.UserId = UserId;
                    eventFunctionalityUserDto.EventId = ret.Entity.EventID;
                    if (CheckType == eCheckType.CheckIn)
                    {
                        eventFunctionalityUserDto.Functionality = eFunctionality.CheckIn;
                        ret.Entity.StatusId = eUserStatus.Absent;
                    }
                    else
                    {
                        eventFunctionalityUserDto.Functionality = eFunctionality.CheckOut;
                        ret.Entity.StatusId = eUserStatus.Present;
                    }
                    if (LocalTran)
                    {
                        tran = DataContext.Database.BeginTransaction();
                    }
                    var DeleteEventFunctionalityUserResult = functionalityManager.DeleteEventFunctionalityUser(eventFunctionalityUserDto);
                    if (DeleteEventFunctionalityUserResult.Success)
                    {
                        retVal = UpdateUser(ret.Entity);
                        if (CheckType == eCheckType.CheckIn)
                        {
                            retVal.Message = "Annullamento Check-in effettuato correttamente";
                        }
                        else
                        {
                            retVal.Message = "Annullamento Check-out effettuato correttamente";
                        }
                        if (LocalTran)
                        {
                            tran.Commit();
                        }
                        retVal.Success = true;
                    }
                    else
                    {
                        if (LocalTran)
                        {
                            tran.Rollback();
                        }
                        retVal.Success = false;
                        retVal.Message = DeleteEventFunctionalityUserResult.Message;
                    }

                }
            }
            catch (Exception ex)
            {
                if (LocalTran && tran!=null)
                {
                    tran.Rollback();
                }
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }

        private string CheckUser(UserDto User)
        {
            if (User == null)
            {
                return "Nessun utente pervenuto!";
            }
            if (string.IsNullOrEmpty(User.Lastname))
            {
                return "Inserire il cognome";
            }
            if (!string.IsNullOrEmpty(User.Email))
            {
                try
                {
                    var emailAddress = new MailAddress(User.Email);
                }
                catch (Exception)
                {
                    return "Indirizzo email non valido";
                }
            }
            return string.Empty;
        }

        public BaseResponseDto<List<UserDto>> GetCompanions(int UserId)
        {
            BaseResponseDto<List<UserDto>> retVal = new BaseResponseDto<List<UserDto>>();
            retVal.Entity = new List<UserDto>();
            try
            {
                GetUsersFilter filter = new GetUsersFilter();
                filter.InvitedID = UserId;

                var resultGetUser = GetUsers(filter);

                if (resultGetUser.Success)
                {
                    //Ordino la lista per il campo Name
                    retVal.Entity = resultGetUser.Entity.OrderBy(x => x.Name).ToList();
                    retVal.Message = "Procedura eseguita correttamente";
                    retVal.Success = true;
                }
                else
                {
                    retVal.Message = resultGetUser.Message;
                    retVal.Success = false;
                }



            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }

        public BaseResponseDto<UserDto> GetInvited(int InvitedID)
        {
            BaseResponseDto<UserDto> retVal = new BaseResponseDto<UserDto>();
            try
            {

                var result = GetUserById(InvitedID);
                if (result.Success)
                {
                    retVal.Entity = result.Entity;
                    retVal.Success = true;
                }
                else
                {
                    retVal.Message = result.Message;
                    retVal.Success = false;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Success = false;
                throw;
            }
            return retVal;
        }

        public BaseResponseDto<List<UserDto>> GetInviteds(int EventId)
        {
            BaseResponseDto<List<UserDto>> retVal = new BaseResponseDto<List<UserDto>>();
            try
            {
                GetUsersFilter filter = new GetUsersFilter(EventId);
                filter.UserType = eUserType.Invited;
                var result = GetUsers(filter);
                if (result.Success)
                {
                    retVal.Entity = result.Entity;
                    retVal.Success = true;
                }
                else
                {
                    retVal.Message = result.Message;
                    retVal.Success = false;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Success = false;
                throw;
            }
            return retVal;
        }

        public MemoryStream ExportUsersDto(List<UserDto> usersdto)
        {
            var stream = new MemoryStream();
            var workbook = new XSSFWorkbook();
            var currentRowIndex = 0;
            var sheet = workbook.CreateSheet("Users");
            var fontBold = workbook.CreateFont();
            var styleBold = workbook.CreateCellStyle();

            #region HeadStyle
            XSSFCellStyle headStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            XSSFFont font = workbook.CreateFont() as XSSFFont;
            font.FontHeightInPoints = 20;
            font.IsBold = true;
            headStyle.SetFont(font);
            #endregion

            #region DatasStyle
            XSSFCellStyle DataStyle = workbook.CreateCellStyle() as XSSFCellStyle;
            XSSFFont areaFont = workbook.CreateFont() as XSSFFont;
            areaFont.FontHeightInPoints = 12;
            areaFont.IsBold = false;
            DataStyle.SetFont(areaFont);
            #endregion

            fontBold.IsBold = true;
            styleBold.SetFont(fontBold);

            #region Header

            var row = sheet.CreateRow(currentRowIndex);

            row.CreateCell((int)eExportColumn.ID).SetCellValue("ID");
            row.GetCell((int)eExportColumn.ID).CellStyle = headStyle;

            row.CreateCell((int)eExportColumn.FIRSTNAME).SetCellValue("NOME");
            row.GetCell((int)eExportColumn.FIRSTNAME).CellStyle = headStyle;


            row.CreateCell((int)eExportColumn.LASTNAME).SetCellValue("COGNOME");
            row.GetCell((int)eExportColumn.LASTNAME).CellStyle = headStyle;

            row.CreateCell((int)eExportColumn.COMPANY).SetCellValue("AZIENDA");
            row.GetCell((int)eExportColumn.COMPANY).CellStyle = headStyle;

            row.CreateCell((int)eExportColumn.NOTES).SetCellValue("NOTE");
            row.GetCell((int)eExportColumn.NOTES).CellStyle = headStyle;
            currentRowIndex++;

            row.CreateCell((int)eExportColumn.PRESENT).SetCellValue("PRESENTE");
            row.GetCell((int)eExportColumn.PRESENT).CellStyle = headStyle;

            #endregion

            #region Datad

            foreach (var user in usersdto.OrderBy(x => x.Lastname).ThenBy(x => x.Firstname))
            {
                row = sheet.CreateRow(currentRowIndex);

                row.CreateCell((int)eExportColumn.ID).SetCellValue(user.ID.ToString());
                row.GetCell((int)eExportColumn.ID).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumn.FIRSTNAME).SetCellValue(user.Firstname);
                row.GetCell((int)eExportColumn.FIRSTNAME).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumn.LASTNAME).SetCellValue(user.Lastname);
                row.GetCell((int)eExportColumn.LASTNAME).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumn.COMPANY).SetCellValue(user.Company);
                row.GetCell((int)eExportColumn.COMPANY).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumn.NOTES).SetCellValue(user.Notes);
                row.GetCell((int)eExportColumn.NOTES).CellStyle = DataStyle;

                //row.CreateCell((int)eExportColumns.PRESENT).SetCellValue(user.IsPresent.ToString());
                //row.GetCell((int)eExportColumns.PRESENT).CellStyle = DataStyle;

                currentRowIndex++;
            }

            #endregion

            sheet.AutoSizeColumn((int)eExportColumn.ID);
            sheet.AutoSizeColumn((int)eExportColumn.FIRSTNAME);
            sheet.AutoSizeColumn((int)eExportColumn.LASTNAME);
            sheet.AutoSizeColumn((int)eExportColumn.NOTES);
            sheet.AutoSizeColumn((int)eExportColumn.COMPANY);
            sheet.AutoSizeColumn((int)eExportColumn.PRESENT);

            using (stream = new MemoryStream())
            {
                workbook.Write(stream);
            }

            return stream;
        }

        public MemoryStream GetQRCode(int UserId)
        {
            var stream = new MemoryStream();
            var getUserResponse = GetUserById(UserId);
            if (getUserResponse.Success)
            {
                QRCodeUser QRCodeUser = new QRCodeUser(UserId);
                var jsonQRCodeUser = JsonConvert.SerializeObject(QRCodeUser);
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(jsonQRCodeUser, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(60);
                using (stream = new MemoryStream())
                {
                    QrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                }

            }
            return stream;
        }

        private BaseResponseDto<UserDto> SetCheckInOutDateTime(UserDto userDto)
        {
            BaseResponseDto<UserDto> retVal = new BaseResponseDto<UserDto>();
            try
            {
                var getByUserResult = functionalityManager.GetUserFunctionalities(userDto.ID);
                if (getByUserResult.Success)
                {
                    foreach (var function in getByUserResult.Entity)
                    {
                        switch (function.Functionality)
                        {
                            case eFunctionality.CheckIn:
                                userDto.CheckInDatetime = function.OccursDateTime;
                                break;
                            case eFunctionality.CheckOut:
                                userDto.CheckOutDatetime = function.OccursDateTime;
                                break;
                        }
                    }
                    retVal.Entity = userDto;
                    retVal.Success = true;
                }
                else
                {
                    retVal.Message = getByUserResult.Message;
                    retVal.Success = false;

                }
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Success = false;
                throw;
            }
            return retVal;
        }

        private BaseResponseDto<List<UserDto>> SetCheckInOutDateTime(List<UserDto> usersDto)
        {
            BaseResponseDto<List<UserDto>> retVal = new BaseResponseDto<List<UserDto>>();
            try
            {
                if (usersDto != null && usersDto.Count > 0)
                {
                    //I Assume that all user are part of the sme EventId!!
                    var UsersFunctionalitiesResult = functionalityManager.GetUsersFunctionalities(usersDto.First().EventID);
                    if (UsersFunctionalitiesResult.Success)
                    {
                        foreach (var function in UsersFunctionalitiesResult.Entity)
                        {
                            var tmpUser = usersDto.FirstOrDefault(x => x.ID == function.UserId);
                            if (tmpUser != null)
                            {
                                switch (function.Functionality)
                                {
                                    case eFunctionality.CheckIn:
                                        tmpUser.CheckInDatetime = function.OccursDateTime;
                                        break;
                                    case eFunctionality.CheckOut:
                                        tmpUser.CheckOutDatetime = function.OccursDateTime;
                                        break;
                                }
                            }
                        }
                        retVal.Entity = usersDto;
                        retVal.Success = true;
                    }
                    else
                    {
                        retVal.Message = UsersFunctionalitiesResult.Message;
                        retVal.Success = false;

                    }
                }
            }
            catch (Exception ex)
            {
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
                retVal.Success = false;
                throw;
            }
            return retVal;
        }
    }
}