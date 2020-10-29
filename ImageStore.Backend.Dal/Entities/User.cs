using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Dal.Entities
{
    public class User : IdentityUser<int>
    {
        public List<Image> Images { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
