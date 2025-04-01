using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatCatalog.Models;

public class Cat
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(1000)]
    public string CatId { get; set; }

    [Required]
    public int Width { get; set; }

    [Required]
    public int Height { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Image { get; set; }

    [Required]
    public DateTime Created { get; set; }

    public List<Tag> Tags { get; set; } = [];
}
