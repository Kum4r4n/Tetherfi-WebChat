using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Models
{
    public class ChatRoomModel
    {
        public Guid ParterId { get; set; }
        public string PartnerName { get; set; }
        public string PartnerConnectionId { get; set; }
        public List<ChatModel> Chats { get; set; }
    }
}
