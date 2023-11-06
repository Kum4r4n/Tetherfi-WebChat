using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Domain.Entities
{
    public class UserConnectionInfo
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }
    }
}
