using System;
using System.Collections.Generic;

namespace TmsApi.Models
{
    public partial class UserDatum
    {
        public string Username { get; set; } = null!;
        public string Passcode { get; set; } = null!;
    }
}
