using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Common.Options
{
    public class DbSeedOptions
    {
        public bool CreateDefaultAdmin { get; set; }
        public string DefaultAdminUserName { get; set; }
        public string DefaultAdminEmailAddress { get; set; }
        public string DefaultAdminPassword { get; set; }
    }
}
