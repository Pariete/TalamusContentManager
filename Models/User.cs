using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talamus_ContentManager.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public string? Username { get; set; }
        public List<Saving> Savings { get; set; }
    }
}
