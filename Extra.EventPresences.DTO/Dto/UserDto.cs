using Extra.EventPresences.DTO.Enums;
using Extra.EventPresences.DTO.Extentions;
using Extra.EventPresences.Middleware.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extra.EventPresences.DTO
{
    public class UserDto
    {
        public int ID { get; set; }
        public int EventID { get; set; }

        private string? firstname = string.Empty;
        public string? Firstname
        {
            get
            {
                return firstname.UppercaseWords();
            }
            set { firstname = value; }
        }
        private string lastname = string.Empty;
        public string Lastname
        {
            get
            {
                return lastname.UppercaseWords();
            }
            set { lastname = value; }
        }
        public string? Email { get; set; }

        public string? Company { get; set; }
        public string? Notes { get; set; }


        public DateTime? CheckInDatetime { get; set; }

        public DateTime? CheckOutDatetime { get; set; }

        public eUserStatus StatusId { get; set; }
        public string? Status { get { return StatusId.GetEnumDescription(); } }

        public string Name
        {
            get
            {
                return (string.IsNullOrEmpty(Lastname) ? "": Lastname + " ") + (Firstname ?? "");
            }
        }

        /// <summary>
        /// Contiene la concatenazione di tutti i campi descrittivi dell'utente. Questo campo viene utilizzato per effettuare la ricarca lato mobile App.
        /// </summary>
        public string FullDescription
        {
            get
            {
                return ((Lastname ?? " ") + " " +(Firstname ?? " ") + " " + (Email ?? " ") + " " + (Company ?? " ") + " " + (Notes ?? " ")).ToUpper();
            }
        }

        public int? InvitedID { get; set; }

        public bool IsInvited { get { return InvitedID == null; } }
        public bool Deleted { get; set; }


    }
}
