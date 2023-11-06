using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Models
{
    public class ChatUserModel
    {
        public Guid Id { get; set; }
        public string ConnectionId { get; set; }

        public string Name { get; set; }
    }
}
