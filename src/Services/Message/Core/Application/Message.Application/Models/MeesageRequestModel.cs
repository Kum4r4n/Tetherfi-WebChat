using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Models
{
    public class MeesageRequestModel
    {
        public string Message { get; set; }
        public Guid PartnerId { get; set; }
    }
}
