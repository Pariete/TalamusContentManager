using System.ComponentModel.DataAnnotations;

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
