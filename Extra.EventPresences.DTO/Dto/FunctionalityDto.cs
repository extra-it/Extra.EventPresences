using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{

    public class FunctionalityDto
    {
  
        public int ID { get; set; }

        public string Name { get; set; }

        public int NumOccurs { get; set; }
    }
}
