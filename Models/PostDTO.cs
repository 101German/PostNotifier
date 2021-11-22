using System;
using System.Collections.Generic;

namespace PoemPost.Data.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PoemText { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
    }
}
