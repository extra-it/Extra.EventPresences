using Azure.Core;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Extra.EventPresences.Model.Enums;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Extra.EventPresences.DTO.Dto
{
    public class WebApiLogDto : CustomHeaderDto
    {
        public WebApiLogDto()
        {
            StartDate = DateTime.UtcNow;
            Request = "";
        }

        public WebApiLogDto(CustomHeaderDto header) : this()
        {
            Platform = header.Platform;
            DeviceIdiom = header.DeviceIdiom;
            DeviceManufacture = header.DeviceManufacture;
            DeviceModel = header.DeviceModel;
            DeviceName = header.DeviceName;
            DeviceOSVersionNumber = header.DeviceOSVersionNumber;
            DeviceType = header.DeviceType;
            Culture = header.Culture;
            AppVersion = header.AppVersion;
            ReqTimestamp = header.ReqTimestamp;
            Token = header.Token;
            Request = JsonConvert.SerializeObject(header);
        }

        public long ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public eApiMethod Method { get; set; }
        public long? UserID { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public eResponseStatus Status { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public eLogLevel LogLevel { get; set; }

        public void SetResponse(BaseResponseApiDto response)
        {
            Status = response.Status;
            Message = response.Message;
        }
    }
    public class WebApiLogDto<TEntity> : WebApiLogDto
    {
        public WebApiLogDto(BaseRequestApiDto<TEntity> requestDto) : base()
        {
            Request = JsonConvert.SerializeObject(requestDto.Item);
        }

        public WebApiLogDto(CustomHeaderDto header) : base(header)
        {

        }

        public WebApiLogDto(CustomHeaderDto header, BaseRequestApiDto<TEntity> requestDto) : base(header)
        {
            Request = JsonConvert.SerializeObject(requestDto.Item);
        }

        public void SetResponse<TResponse>(BaseResponseApiDto<TResponse> response)
        {
            Status = response.Status;
            Response = response.Item != null ? JsonConvert.SerializeObject(response.Item) : null;
            Message = response.Message;
        }
    }
}
