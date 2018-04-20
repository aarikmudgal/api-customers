using System;
using System.Collections.Generic;
using System.Text;

namespace eshop.api.customer.dal.Models
{
    public class ReturnResult
    {
        public string StatusMessage { get; set; }
        public Customer UpdatedCustomer { get; set; }
    }
}
