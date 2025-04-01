namespace CatCatalog.Resources;

public class CatResponse
{
    public int Id { get; set; }

    public string CatId { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public string Image { get; set; }

    public DateTime Created { get; set; }

    public List<string> Tags { get; set; } = new();
}
