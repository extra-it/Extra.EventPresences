using Extra.EventPresences.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO.Dto
{
    public class CustomHeaderDto
    {
        public ePlatform? Platform { get; set; }
        public string Culture { get; set; }
        public string Country
        {
            get
            {
                var parts = Culture?.Split('-');
                if (parts != null && parts.Length >= 2)
                {
                    return parts[1];
                }
                return null;
            }
        }
        public string Language
        {
            get
            {
                var parts = Culture?.Split('-');
                if (parts != null && parts.Length >= 1)
                {
                    return parts[0];
                }
                return null;
            }
        }
        public string AppVersion { get; set; }
        public DateTime? ReqTimestamp { get; set; }
        public string Token { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceManufacture { get; set; }
        public string DeviceName { get; set; }
        public string DeviceOSVersionNumber { get; set; }
        public string DeviceIdiom { get; set; }
        public string DeviceType { get; set; }
    }
}
