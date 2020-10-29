using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ImageStore.Backend.Dal.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public User Uploader { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
