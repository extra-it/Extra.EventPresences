using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Model;
using Extra.EventPresences.Model.Entities;
using Extra.EventPresences.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Crypt.Dsig;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class UserManager : BaseManager, iUserManager
    {
        public UserManager(DBDataContext dbdatacontext) : base(dbdatacontext)
        {
        }
        public BaseResponseDto<List<UserDto>> GetUsers(int EventId)
        {
            var retVal = new BaseResponseDto<List<UserDto>>();
            retVal.Entity = new List<UserDto>();
            try
            {

                foreach (User user in DataContext.Users.Where(x => x.EventID == EventId && !x.Deleted).ToList())
                {
                    retVal.Entity.Add(MapperManager.GetMapper().Map<UserDto>(user));
                }
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

        public BaseResponseDto UpdateUser(UserDto userdto)
        {
            var retVal = new BaseResponseDto();
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
                    retVal.Entity.UserTotalNum = EventUsers.Count();
                    retVal.Entity.UserPresences = EventUsers.Where(x => x.IsPresent).ToList().Count();
                    retVal.Entity.UserPresencesPerc = Math.Round((double)retVal.Entity.UserPresences / (double)retVal.Entity.UserTotalNum * 100.0, 2);
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


        public BaseResponseDto<UserDto> AddUser(UserDto userdto)
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
                    var objDB = MapperManager.GetMapper().Map<User>(userdto);
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


        private string CheckUser(UserDto User)
        {
            if (User == null)
            {

                return "Nessun utente pervenuto!";
            }
            else if (string.IsNullOrEmpty(User.Firstname))
            {
                return "Inserire il nome";
            }
            else if (string.IsNullOrEmpty(User.Lastname))
            {
                return "Inserire il cognome";
            }
            return string.Empty;
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

            foreach (var user in usersdto.OrderBy(x=>x.Lastname).ThenBy(x=>x.Firstname))
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

                row.CreateCell((int)eExportColumns.PRESENT).SetCellValue(user.IsPresent.ToString());
                row.GetCell((int)eExportColumns.PRESENT).CellStyle = DataStyle;

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
    }
}