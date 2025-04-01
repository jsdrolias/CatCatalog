using CatCatalog.Abstractions;
using CatCatalog.Contexts;
using CatCatalog.DTO;
using CatCatalog.Models;
using CatCatalog.Resources;
using Microsoft.EntityFrameworkCore;

namespace CatCatalog.Services;

public class CatProcessingService : ICatProcessingService
{
    private readonly ICatWebClientService _catWebClientService;
    private readonly CatDbContext _context;
    private readonly BlobStorageService _blobStorageService;
    private readonly ILogger<CatProcessingService> _logger;

    public CatProcessingService(
        ICatWebClientService catWebClientService,
        CatDbContext catDbContext,
        BlobStorageService blobStorageService,
        ILogger<CatProcessingService> logger)
    {
        _catWebClientService = catWebClientService;
        _context = catDbContext;
        _blobStorageService = blobStorageService;
        _logger = logger;
    }

    public async Task<CatResponse?> GetById(int id)
    {
        var cat = await _context.Cat
            .Include(c => c.Tags)
            .SingleOrDefaultAsync(x => x.Id == id);

        if (cat is null)
        {
            return null;
        }

        var response = new CatResponse
        {
            Id = cat.Id,
            CatId = cat.CatId,
            Height = cat.Height,
            Image = cat.Image,
            Created = cat.Created,
            Width = cat.Width,
            Tags = cat.Tags.Select(b => b.Name).ToList()
        };
        return response;
    }

    public async Task<IEnumerable<CatResponse>> GetAll(int page, int pageSize, string? tag)
    {
        var query = _context.Cat
            .Include(c => c.Tags)
            .Skip(pageSize * (page - 1))
            .Take(pageSize);

        if (!string.IsNullOrWhiteSpace(tag))
        {
            query = query
                .Where(b => b.Tags.Exists(b => b.Name == tag));
        }

        query = query
            .OrderBy(b => b.Created);

        var queryData = await query
            .ToListAsync();

        var response = queryData.Select(cat => new CatResponse
        {
            Id = cat.Id,
            CatId = cat.CatId,
            Height = cat.Height,
            Image = cat.Image,
            Created = cat.Created,
            Width = cat.Width,
            Tags = cat.Tags.Select(b => b.Name).ToList()
        });
        return response;
    }

    public async Task FetchImages()
    {
        try
        {
            var webCats = await _catWebClientService.FetchCatImagesAsync(25);

            var createdDate = DateTime.UtcNow;
            var distinctTags = GetDistinctLowercaseTags(webCats);
            await AddTagsIfNotExist(distinctTags, createdDate);

            foreach (var cat in webCats)
            {
                var catTags = GetTags(cat.Breeds);

                var dbCat = await _context.Cat
                    .Include(b => b.Tags)
                    .SingleOrDefaultAsync(b => b.CatId == cat.ImageId);

                if (dbCat is null)
                {
                    string blobUrl = await _blobStorageService.DownloadAndUploadImageAsync(cat.ImageUrl, "images", cat.ImageId + ".jpg");

                    var newCat = new Models.Cat
                    {
                        CatId = cat.ImageId,
                        Height = cat.Height,
                        Width = cat.Width,
                        Image = blobUrl,
                        Created = createdDate
                    };

                    foreach (var tag in catTags)
                    {
                        var lowerCaseTag = distinctTags.Single(b => b == tag.ToLower());
                        var dbTag = _context.Tag.Single(b => b.Name == lowerCaseTag);
                        newCat.Tags.Add(dbTag);
                    }

                    _context.Cat.Add(newCat);
                }

            }
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(ex.Message);
        }

    }

    private async Task AddTagsIfNotExist(List<string> tags, DateTime createdDate)
    {
        var existingTags = await _context.Tag
            .Where(b => tags.Contains(b.Name))
            .ToListAsync();

        var missingTags = tags
            .Where(b => !existingTags.Select(b => b.Name).Contains(b))
            .ToList();

        _context.Tag.AddRange(missingTags.Select(b => new Tag { Name = b, Created = createdDate }));

        await _context.SaveChangesAsync();
    }

    private List<string> GetDistinctTags(List<CatFromWebDTO> cats)
    {
        var catTags = new List<string>();
        foreach (var cat in cats)
        {
            var tags = GetTags(cat.Breeds);
            catTags.AddRange(tags);
        }

        return catTags
            .GroupBy(n => n, StringComparer.OrdinalIgnoreCase)
            .Select(g => g.First())
            .ToList();
    }

    private List<string> GetDistinctLowercaseTags(List<CatFromWebDTO> cats)
    {
        var catTags = new List<string>();
        foreach (var cat in cats)
        {
            var tags = GetTags(cat.Breeds);
            catTags.AddRange(tags);
        }

        return catTags
            .Select(g => g.ToLowerInvariant())
            .Distinct()
            .ToList();
    }

    private string[] GetTags(List<BreedFromWebDTO> breeds)
    {
        var temperaments = string.Join(",", breeds.Select(b => b.Temperament));
        var temperamentArray = temperaments.Split(",");
        return temperamentArray.Select(b => b.TrimStart().TrimEnd()).ToArray();
    }
}
