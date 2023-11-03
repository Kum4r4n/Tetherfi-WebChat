using Identity.Application.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Identity.Application.Models.Request
{
    public class LoginRequestModel : UserModel
    {
        [JsonIgnore]
        public override Guid? Id { get => base.Id; set => base.Id = value; }
        [JsonIgnore]
        public override string Name { get => base.Name; set => base.Name = value; }
    }
}
