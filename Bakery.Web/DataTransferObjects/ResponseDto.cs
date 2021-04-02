using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bakery.Web.DataTransferObjects
{
    public class ResponseDto
    {
        public string UserName { get; set; }
        public string JwtToken { get; set; }
    }
}
