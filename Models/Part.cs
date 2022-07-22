using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
