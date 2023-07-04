using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO
{
    public class BaseResponseApiDto
    {
        public BaseResponseApiDto()
        {
            Status = eResponseStatus.Error;
            Timestamp = DateTime.UtcNow;
        }

        public string Message { get; set; }
        public eResponseStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class BaseResponseApiDto<T> : BaseResponseApiDto
    {
        public T Item { get; set; }
    }
}
