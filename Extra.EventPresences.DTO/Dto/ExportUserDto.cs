using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class ExportUserDto
    {
        public byte[] stream { get; set; }
        public string contentType { get; set; }
        public string fileDownloadName { get; set; }

    }
}
