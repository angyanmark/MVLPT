using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Common.Dtos
{
    public class ErrorDto
    {
        public string Title { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; }
    }
}
