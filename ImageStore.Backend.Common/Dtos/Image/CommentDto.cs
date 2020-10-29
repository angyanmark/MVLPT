using FluentValidation;
using System;

namespace ImageStore.Backend.Common.Dtos.Image
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Commenter { get; set; }
    }
}
