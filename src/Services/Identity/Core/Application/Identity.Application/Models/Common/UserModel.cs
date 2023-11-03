namespace Identity.Application.Models.Common
{
    public class UserModel
    {
        public virtual Guid? Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
    }
}
