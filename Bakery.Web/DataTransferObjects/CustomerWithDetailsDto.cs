using System;

namespace Bakery.Web.DataTransferObjects
{
    public class CustomerWithDetailsDto : CustomerDto 
    {
        public bool IsAdmin { get; set; }
        public DateTime RegisteredSince { get; set; }
        public int NrOfOrders { get; set; }
    }
}
