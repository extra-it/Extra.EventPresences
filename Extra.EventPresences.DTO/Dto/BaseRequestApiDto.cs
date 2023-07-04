using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO
{
    public class BaseRequestApiDto
    {
        public BaseRequestApiDto()
        {
        }
    }

    public class BaseRequestApiDto<T> : BaseRequestApiDto
    {
        public BaseRequestApiDto() : base()
        {
            this.Item = default;
        }
        public T Item { get; set; }
    }
}
