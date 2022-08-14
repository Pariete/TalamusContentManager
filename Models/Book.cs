using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Talamus_ContentManager.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Description { get; set; }
        /// <summary>
        ///  links from google drive, must be formatted like this: https://drive.google.com/uc?export=view&id= ID of image
        /// </summary>
        public string ImageUrl { get; set; }
        public long Price { get; set; } = 0;
        public List<Part> Parts { get; set; } = new List<Part>();

        public bool Hidden { get; set; } = false;
    }
}
