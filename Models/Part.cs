using System;
using System.ComponentModel.DataAnnotations;

namespace Talamus_ContentManager.Models
{
    public class Part
    {
        [Key]
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public Book Book { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// HTML formatted text
        /// </summary>
        public string Content { get; set; }

        [Required]
        public int PageNumber { get; set; }
        public DateTime? Created { get; set; } = DateTime.Now;
        public bool DemoEnd { get; set; } = false;
    }
}
