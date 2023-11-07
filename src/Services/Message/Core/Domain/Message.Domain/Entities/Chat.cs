using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Domain.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ChatRoomId { get; set; }
        public Guid SenderId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
