using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Common.Options
{
    public class JwtOptions
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public int ValidMinutes { get; set; }
    }
}
