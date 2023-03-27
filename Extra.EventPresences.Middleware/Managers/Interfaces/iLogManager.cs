using Extra.EventPresences.DTO.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Middleware.Managers.Interfaces
{
    public interface iLogManager
    {
        public BaseResponseDto<long> LogDebug(WebApiLogDto itemDto);
        public BaseResponseDto<long> LogInfo(WebApiLogDto itemDto);

        public BaseResponseDto<long> LogWarn(WebApiLogDto itemDto);
        public BaseResponseDto<long> LogError(WebApiLogDto itemDto);
    }
}
