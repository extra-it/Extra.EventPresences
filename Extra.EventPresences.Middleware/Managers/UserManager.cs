using Azure;
using Extra.EventPresences.DTO;
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
        public UserManager(DBDataContext dbdatacontext) : base(dbdatacontext)
        {
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
                    retVal.Entity = MapperManager.GetMapper().Map<UserDto>(user);
                    retVal.Message = "Procedura eseguita correttamente";
                    retVal.Success = true;
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
            try
            {
                IQueryable<User> tempResult = DataContext.Users.Where(x=> !x.Deleted);

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
                    retVal.Entity.Add(MapperManager.GetMapper().Map<UserDto>(user));
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
                    UserDto.CheckInDatetime = DateTime.UtcNow;
                    UserDto.StatusId = eUserStatus.Present;

                    var objDB = MapperManager.GetMapper().Map<User>(UserDto);
                    objDB.DateInsert = DateTime.UtcNow;


                    DataContext.Users.Add(objDB);
                    DataContext.SaveChanges();

                    retVal.Entity = MapperManager.GetMapper().Map<UserDto>(objDB);
                    retVal.Message = "Utente inserito correttamente";
                    retVal.Success = true;
                }
            }
            catch (Exception ex)
            {
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
                    var ret = CheckInOut(UserId, CheckType);
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
                    var ret = CancelCheckInOut(UserId, CheckType);
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

        private BaseResponseDto<UserDto> CheckInOut(int UserId, eCheckType CheckType)
        {
            var retVal = new BaseResponseDto<UserDto>();
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
                    if (CheckType == eCheckType.CheckIn)
                    {
                        ret.Entity.CheckInDatetime = DateTime.UtcNow;
                        ret.Entity.StatusId = eUserStatus.Present;
                    }
                    else
                    {
                        ret.Entity.CheckOutDatetime = DateTime.UtcNow;
                        ret.Entity.StatusId = eUserStatus.Left;
                    }
                    retVal = UpdateUser(ret.Entity);
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
            }
            return retVal;
        }

        private BaseResponseDto<UserDto> CancelCheckInOut(int UserId, eCheckType CheckType)
        {
            var retVal = new BaseResponseDto<UserDto>();
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
                    if (CheckType == eCheckType.CheckIn)
                    {
                        ret.Entity.CheckInDatetime = null;
                        ret.Entity.StatusId = eUserStatus.Absent;
                    }
                    else
                    {
                        ret.Entity.CheckOutDatetime = null;
                        ret.Entity.StatusId = eUserStatus.Present;
                    }
                    retVal = UpdateUser(ret.Entity);
                    if (CheckType == eCheckType.CheckIn)
                    {
                        retVal.Message = "Annullamento Check-in effettuato correttamente";
                    }
                    else
                    {
                        retVal.Message = "Annullamento Check-out effettuato correttamente";
                    }
                    retVal.Success = true;
                }
            }
            catch (Exception ex)
            {
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

            row.CreateCell((int)eExportColumns.ID).SetCellValue("ID");
            row.GetCell((int)eExportColumns.ID).CellStyle = headStyle;

            row.CreateCell((int)eExportColumns.FIRSTNAME).SetCellValue("NOME");
            row.GetCell((int)eExportColumns.FIRSTNAME).CellStyle = headStyle;


            row.CreateCell((int)eExportColumns.LASTNAME).SetCellValue("COGNOME");
            row.GetCell((int)eExportColumns.LASTNAME).CellStyle = headStyle;

            row.CreateCell((int)eExportColumns.COMPANY).SetCellValue("AZIENDA");
            row.GetCell((int)eExportColumns.COMPANY).CellStyle = headStyle;

            row.CreateCell((int)eExportColumns.NOTES).SetCellValue("NOTE");
            row.GetCell((int)eExportColumns.NOTES).CellStyle = headStyle;
            currentRowIndex++;

            row.CreateCell((int)eExportColumns.PRESENT).SetCellValue("PRESENTE");
            row.GetCell((int)eExportColumns.PRESENT).CellStyle = headStyle;

            #endregion

            #region Datad

            foreach (var user in usersdto.OrderBy(x => x.Lastname).ThenBy(x => x.Firstname))
            {
                row = sheet.CreateRow(currentRowIndex);

                row.CreateCell((int)eExportColumns.ID).SetCellValue(user.ID.ToString());
                row.GetCell((int)eExportColumns.ID).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumns.FIRSTNAME).SetCellValue(user.Firstname);
                row.GetCell((int)eExportColumns.FIRSTNAME).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumns.LASTNAME).SetCellValue(user.Lastname);
                row.GetCell((int)eExportColumns.LASTNAME).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumns.COMPANY).SetCellValue(user.Company);
                row.GetCell((int)eExportColumns.COMPANY).CellStyle = DataStyle;

                row.CreateCell((int)eExportColumns.NOTES).SetCellValue(user.Notes);
                row.GetCell((int)eExportColumns.NOTES).CellStyle = DataStyle;

                //row.CreateCell((int)eExportColumns.PRESENT).SetCellValue(user.IsPresent.ToString());
                //row.GetCell((int)eExportColumns.PRESENT).CellStyle = DataStyle;

                currentRowIndex++;
            }

            #endregion

            sheet.AutoSizeColumn((int)eExportColumns.ID);
            sheet.AutoSizeColumn((int)eExportColumns.FIRSTNAME);
            sheet.AutoSizeColumn((int)eExportColumns.LASTNAME);
            sheet.AutoSizeColumn((int)eExportColumns.NOTES);
            sheet.AutoSizeColumn((int)eExportColumns.COMPANY);
            sheet.AutoSizeColumn((int)eExportColumns.PRESENT);

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
                var jsonUserDto= JsonConvert.SerializeObject(getUserResponse.Entity);
                QRCodeGenerator QrGenerator = new QRCodeGenerator();
                QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(jsonUserDto, QRCodeGenerator.ECCLevel.Q);
                QRCode QrCode = new QRCode(QrCodeInfo);
                Bitmap QrBitmap = QrCode.GetGraphic(60);
                using ( stream = new MemoryStream())
                {
                    QrBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    
                }

            }
            return stream;

        }
    }
}