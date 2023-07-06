using Extra.EventPresences.DTO;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.DTO.Enums;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Middleware.Services.Interfaces;
using Extra.EventPresences.Model;
using Extra.EventPresences.Model.Entities;
using Extra.EventPresences.Model.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers
{
    public class FunctionalityManager : BaseManager, iFunctionalityManager
    {
        public FunctionalityManager(DBDataContext dbdatacontext) : base(dbdatacontext)
        {
        }

        public BaseResponseDto<List<FunctionalityDto>> GetFunctionalitiesByEvent(int EventId)
        {
            var retVal = new BaseResponseDto<List<FunctionalityDto>>();
            retVal.Entity = new List<FunctionalityDto>();
            try
            {

                var functionalities = (from eventFunctionalityes in DataContext.EventFunctionalities.Where(x => x.EventID == EventId)
                                       join Functionality in DataContext.Functionalities.Where(x => !x.Deleted)
                                        on eventFunctionalityes.FunctionalityID equals Functionality.ID
                                       select new FunctionalityDto { ID = Functionality.ID, Name = Functionality.Name, NumOccurs = eventFunctionalityes.NumOccurs }).ToList();

                if (functionalities.Count > 0)
                {
                    retVal.Entity = functionalities;
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

        public BaseResponseDto<List<EventFunctionalityUserDto>> GetUserFunctionalities(int UserId)
        {
            var retVal = new BaseResponseDto<List<EventFunctionalityUserDto>>();
            retVal.Entity = new List<EventFunctionalityUserDto>();
            try
            {

                var functionalities = (
                    from eventFunctionalityUsers in DataContext.EventFunctionalityUsers.Where(x => x.UserID == UserId)
                    join EventFunctionalities in DataContext.EventFunctionalities
                        on eventFunctionalityUsers.EventFunctionalityID equals EventFunctionalities.ID
                    join Functionality in DataContext.Functionalities.Where(x => !x.Deleted)
                         on EventFunctionalities.FunctionalityID equals Functionality.ID
                    select new EventFunctionalityUserDto
                    {
                        Functionality = (eFunctionality)Functionality.ID,
                        UserId = eventFunctionalityUsers.UserID,
                        OccursDateTime = eventFunctionalityUsers.OccursDateTime
                    }).ToList();

                if (functionalities.Count > 0)
                {
                    retVal.Entity = functionalities;
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
        public BaseResponseDto<int> GetEventFunctionalityNumOccurs(int EventId, eFunctionality functionality)
        {
            var retVal = new BaseResponseDto<int>();
            try
            {

                var EventFunctionality = DataContext.EventFunctionalities.FirstOrDefault(x => x.EventID == EventId && x.FunctionalityID == (int)functionality);

                if (EventFunctionality != null)
                {
                    retVal.Entity = EventFunctionality.NumOccurs;
                    retVal.Message = "Procedura eseguita correttamente";
                    retVal.Success = true;
                }
                else
                {
                    retVal.Message = "Il binomio Evento funzionalità non è stato trovato nel sistema";
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

        public BaseResponseDto<int> GetEventFunctionalityAlreadyOccurs(int EventId, int UserId, eFunctionality functionality)
        {
            var retVal = new BaseResponseDto<int>();
            try
            {

                var queryResult = (from EventFunctionalityUser in DataContext.EventFunctionalityUsers.Where(x => x.UserID == UserId)
                                   join EventFunctionality in DataContext.EventFunctionalities.Where(x => x.EventID == EventId && x.FunctionalityID == (int)functionality)
                                   on EventFunctionalityUser.EventFunctionalityID equals EventFunctionality.ID
                                   select EventFunctionalityUser).Count();

                retVal.Entity = queryResult;
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

        public BaseResponseDto<List<EventFunctionalityUserDto>> GetUsersFunctionalities(int EventId)
        {
            var retVal = new BaseResponseDto<List<EventFunctionalityUserDto>>();
            retVal.Entity = new List<EventFunctionalityUserDto>();
            try
            {

                var functionalities = (
                    from eventFunctionalityUsers in DataContext.EventFunctionalityUsers
                    join EventFunctionalities in DataContext.EventFunctionalities.Where(x => x.EventID == EventId)
                        on eventFunctionalityUsers.EventFunctionalityID equals EventFunctionalities.ID
                    join Functionality in DataContext.Functionalities.Where(x => !x.Deleted)
                         on EventFunctionalities.FunctionalityID equals Functionality.ID
                    select new EventFunctionalityUserDto
                    {
                        Functionality = (eFunctionality)Functionality.ID,
                        UserId = eventFunctionalityUsers.UserID,
                        OccursDateTime = eventFunctionalityUsers.OccursDateTime
                    }).ToList();

                if (functionalities.Count > 0)
                {
                    retVal.Entity = functionalities;
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

        public BaseResponseDto<EventFunctionalityUserDto> AddEventFunctionalityUser(EventFunctionalityUserDto eventFunctionalityUserDto)
        {
            var retVal = new BaseResponseDto<EventFunctionalityUserDto>();
            try
            {
                var objDB = MapperManager.GetMapper().Map<EventFunctionalityUser>(eventFunctionalityUserDto);
                var GetEventFunctionalityResult = GetEventFunctionality(eventFunctionalityUserDto.EventId, eventFunctionalityUserDto.Functionality);
                if (GetEventFunctionalityResult.Success)
                {
                    objDB.EventFunctionalityID = GetEventFunctionalityResult.Entity.ID;

                    DataContext.EventFunctionalityUsers.Add(objDB);
                    DataContext.SaveChanges();

                    retVal.Entity = MapperManager.GetMapper().Map<EventFunctionalityUserDto>(objDB);
                    switch (eventFunctionalityUserDto.Functionality)
                    {
                        case eFunctionality.PresentReceived:
                            retVal.Message = "Regalo registrato correttamente";
                            break;
                        case eFunctionality.FreeDrink:
                            retVal.Message = "FreeDrink registrato correttamente";
                            break;
                        case eFunctionality.CheckIn:
                            retVal.Message = "Check-In registrato correttamente";
                            break;
                        case eFunctionality.CheckOut:
                            retVal.Message = "Check-Out registrato correttamente";
                            break;
                        default:
                            retVal.Message = "Event-Functionality-User inserita correttamente";
                            break;
                    }
                    
                    retVal.Success = true;
                }
                else
                {
                    retVal.Message = GetEventFunctionalityResult.Message;
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


        public BaseResponseDto DeleteEventFunctionalityUser(EventFunctionalityUserDto eventFunctionalityUserDto)
        {
            var retVal = new BaseResponseDto();
            try
            {
                var GetEventFunctionalityResult = GetEventFunctionality(eventFunctionalityUserDto.EventId, eventFunctionalityUserDto.Functionality);
                if (GetEventFunctionalityResult.Success)
                {

                    var objToDelete = DataContext.EventFunctionalityUsers.FirstOrDefault(x => x.UserID == eventFunctionalityUserDto.UserId && x.EventFunctionalityID == GetEventFunctionalityResult.Entity.ID);
                    if (objToDelete != null)
                    {
                        DataContext.Entry(objToDelete).State = EntityState.Deleted;
                        DataContext.SaveChanges();
                    }
                    retVal.Message = "Cancellazione eseguita con successo";
                    retVal.Success = true;
                }
                else
                {
                    retVal.Success = false;
                    retVal.Message = GetEventFunctionalityResult.Message;
                }
            }
            catch (Exception ex)
            {
                retVal.Success = false;
                retVal.Message = "La procedura ha generato il seguente errore: " + ex.Message;
            }
            return retVal;
        }

        public BaseResponseDto<EventFunctionalityDto> GetEventFunctionality(int EventId, eFunctionality functionality)
        {
            var retVal = new BaseResponseDto<EventFunctionalityDto>();
            retVal.Entity = new EventFunctionalityDto();
            try
            {

                var dbItem = DataContext.EventFunctionalities.FirstOrDefault(x => x.EventID == EventId && x.FunctionalityID == (int)functionality);

                if (dbItem != null)
                {
                    retVal.Entity = MapperManager.GetMapper().Map<EventFunctionalityDto>(dbItem);
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
    }
}
