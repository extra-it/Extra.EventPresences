using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO
{
    public class BaseResponseDto
    {
        public BaseResponseDto()
        {
            Success = false;
            ValidationsErrors = new List<KeyValuePair<string, string>>();
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }
        public List<KeyValuePair<string, string>> ValidationsErrors { get; set; }
    }
    public class BaseResponseDto<T> : BaseResponseDto
    {
        public T Entity { get; set; }
    }
}
