using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoomManagement.Models
{
    public class ResponseLogin
    {
        public bool authenticated { get; set; }
        public string created { get; set; }
        public string expiration { get; set; }
        public string accessToken { get; set; }
        public string message { get; set; }
        public User user { get; set; }
    }

}
