using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Domain.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ChatRoomId { get; set; }
        public Guid SenderId { get; set; }
        public string Message { get; set; }
        public bool IsAttachement { get; set; }
        public byte[]? ImageBytes { get; set; }
        public DateTime CreatedDateTime { get; set; }

    }
}
