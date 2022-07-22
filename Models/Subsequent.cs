using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talamus_ContentManager.Models
{
    public class Subsequent
    {
        [Key]
        public int Id { get; set; }
        
        public Part Part { get; set; }
        public Part NextPart { get; set; }
    }
}
