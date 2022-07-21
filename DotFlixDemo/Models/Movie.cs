using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotFlixDemo.Models
{
    public class Movie
    {
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public long AuthorId { get; set; }
        
        public Author Author { get; set; }
    }
}
