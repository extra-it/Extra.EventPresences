using Extra.EventPresences.DTO;
using Extra.EventPresences.DTO.Dto;
using Extra.EventPresences.Middleware.Classes;
using Extra.EventPresences.Middleware.Managers;
using Extra.EventPresences.Middleware.Managers.Interfaces;
using Extra.EventPresences.Middleware.Services.Interfaces;
using Extra.EventPresences.Model.Entities;
using Extra.EventPresences.Model.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Services
{

    public class FunctionalityService : iFunctionalityService
    {
        private iFunctionalityManager FunctionalityManager = null;
        private iConfigurationManager configurationManager = null;
        private iUserManager UserManager = null;

        public FunctionalityService(iFunctionalityManager functionalitymanager, iConfigurationManager configurationmanager, iUserManager usermanager)
        {
            FunctionalityManager = functionalitymanager;
            configurationManager = configurationmanager;
            UserManager = usermanager;
        }
        public BaseResponseApiDto<List<FunctionalityDto>> GetFunctionalitiesByEvent(int EventId)
        {
            BaseResponseApiDto<List<FunctionalityDto>> retVal = new BaseResponseApiDto<List<FunctionalityDto>>();
            try
            {
                var result = FunctionalityManager.GetFunctionalitiesByEvent(EventId);
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


        public BaseResponseApiDto SetUserFunctionality(EventFunctionalityUserDto EventFunctionalityUser)
        {
            BaseResponseApiDto retVal = new BaseResponseApiDto();
            try
            {
                ///Verifico che l'utente non abbia già raffiunto il numero massimo di volte che può fare questa funzionalità
                var GetEventFunctionalityNumOccursResult = FunctionalityManager.GetEventFunctionalityNumOccurs(EventFunctionalityUser.EventId, EventFunctionalityUser.Functionality);
                if (GetEventFunctionalityNumOccursResult.Success)
                {
                    var GetEventFunctionalityAlreadyOccursResult = FunctionalityManager.GetEventFunctionalityAlreadyOccurs(EventFunctionalityUser.EventId, EventFunctionalityUser.UserId, EventFunctionalityUser.Functionality);
                    if (GetEventFunctionalityAlreadyOccursResult.Success)
                    {
                        ///Limite non raggiunto! SI può procedere
                        if (GetEventFunctionalityAlreadyOccursResult.Entity < GetEventFunctionalityNumOccursResult.Entity)
                        {
                            switch (EventFunctionalityUser.Functionality)
                            {
                                case eFunctionality.CheckIn:
                                    var resultCheckIn = UserManager.CheckIn(EventFunctionalityUser.UserId);
                                    retVal.Message = resultCheckIn.Message;
                                    if (resultCheckIn.Success)
                                    {
                                        retVal.Status = eResponseStatus.Success;
                                    }
                                    else
                                    {
                                        retVal.Status = eResponseStatus.Error;
                                    }
                                    break;
                                case eFunctionality.CheckOut:
                                    var resultCheckOut = UserManager.CheckOut(EventFunctionalityUser.UserId);
                                    retVal.Message = resultCheckOut.Message;
                                    if (resultCheckOut.Success)
                                    {
                                        retVal.Status = eResponseStatus.Success;
                                    }
                                    else
                                    {
                                        retVal.Status = eResponseStatus.Error;
                                    }
                                    break;

                                case eFunctionality.PresentReceived:
                                case eFunctionality.FreeDrink:
                                    EventFunctionalityUserDto EFUDto = new EventFunctionalityUserDto();
                                    EFUDto.OccursDateTime = DateTime.UtcNow;
                                    EFUDto.UserId = EventFunctionalityUser.UserId;
                                    EFUDto.EventId = EventFunctionalityUser.EventId;
                                    EFUDto.Functionality = EventFunctionalityUser.Functionality;

                                    var AddEventFunctionalityUserDtoResult = FunctionalityManager.AddEventFunctionalityUser(EFUDto);
                                    retVal.Message = AddEventFunctionalityUserDtoResult.Message;
                                    if (AddEventFunctionalityUserDtoResult.Success)
                                    {
                                        retVal.Status = eResponseStatus.Success;
                                    }
                                    else
                                    {
                                        retVal.Status = eResponseStatus.Error;
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            retVal.Message = "Limite massimo raggiunto per la funzionalità";
                            retVal.Status = eResponseStatus.Error;
                        }
                    }
                    else
                    {
                        retVal.Message = GetEventFunctionalityAlreadyOccursResult.Message;
                        retVal.Status = eResponseStatus.Error;
                    }
                }
                else
                {
                    retVal.Message = GetEventFunctionalityNumOccursResult.Message;
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
    }
}
