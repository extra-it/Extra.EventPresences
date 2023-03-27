using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.Model.Entities
{
    [Table("WebApiLogs", Schema = "dbo")]
    public class WebApiLog
    {
        [Key]
        public long ID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Method { get; set; }
        public int? Platform { get; set; }
        public int? SaleID { get; set; }
        public string Culture { get; set; }
        public string AppVersion { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacture { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOSVersionNumber { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceType { get; set; }

        public DateTime? ReqTimestamp { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public int LogLevel { get; set; }

        public DateTime DateInsert { get; set; }
        public DateTime? DateUpdate { get; set; }
        public bool Deleted { get; set; }
    }
}
