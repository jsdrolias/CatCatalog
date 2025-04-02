using CatCatalog.Abstractions;
using CatCatalog.Contexts;
using CatCatalog.DTO;
using CatCatalog.Models;
using CatCatalog.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NSubstitute;

namespace CatCatalog.Tests;

public class CatProcessingServiceTest
{
    private readonly CatProcessingService _sut;
    private readonly IBlobStorageService _blobStorageService;
    private readonly ICatWebClientService _catWebClientService;
    private readonly ILogger<CatProcessingService> _logger;
    private readonly Mock<CatDbContext> _catDbContext;
    private Mock<DbSet<Cat>> _dbCat;
    private Mock<DbSet<Tag>> _dbTag;

    public CatProcessingServiceTest()
    {
        var options = new DbContextOptionsBuilder<CatDbContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;

        _catWebClientService = Substitute.For<ICatWebClientService>();

        _dbTag = CreateMockDbSet(new List<Tag>());
        _dbCat = CreateMockDbSet(new List<Cat>());

        var mockContext = new Mock<CatDbContext>();
        mockContext.Setup(c => c.Tag).Returns(_dbTag.Object);
        mockContext.Setup(c => c.Cat).Returns(_dbCat.Object);
        _catDbContext = mockContext;

        _blobStorageService = Substitute.For<IBlobStorageService>();
        _logger = Substitute.For<ILogger<CatProcessingService>>();

        _sut = new CatProcessingService(
            _catWebClientService,
            mockContext.Object,
            _blobStorageService,
            _logger);
    }

    [Theory]
    [AutoNSubstituteData]
    public async Task FetchImages_NoTags_StoresCat(
      CatFromWebDTO cat1,
      CatFromWebDTO cat2,
      string url1,
      string url2)
    {
        // Arrange
        cat1.Breeds.Clear();
        cat2.Breeds.Clear();
        List<CatFromWebDTO> cats = new()
        {
            cat1,
            cat2
        };
        _catWebClientService.FetchCatImagesAsync(Arg.Any<int>())
            .Returns(cats);
        _blobStorageService.DownloadAndUploadImageAsync(cat1.ImageUrl, cat1.ImageId + ".jpg")
            .Returns(url1);
        _blobStorageService.DownloadAndUploadImageAsync(cat2.ImageUrl, cat2.ImageId + ".jpg")
            .Returns(url2);

        // Act
        await _sut.FetchImages();

        // Assert
        var expectedCat1 = _catDbContext.Object.Cat.First();
        expectedCat1.CatId.Should().Be(cat1.ImageId);
        expectedCat1.Height.Should().Be(cat1.Height);
        expectedCat1.Width.Should().Be(cat1.Width);
        expectedCat1.Image.Should().Be(url1);
        expectedCat1.Tags.Should().BeEmpty();

        var expectedCat2 = _catDbContext.Object.Cat.Last();
        expectedCat2.CatId.Should().Be(cat2.ImageId);
        expectedCat2.Height.Should().Be(cat2.Height);
        expectedCat2.Width.Should().Be(cat2.Width);
        expectedCat2.Image.Should().Be(url2);
        expectedCat2.Tags.Should().BeEmpty();
    }

    public static Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryableData = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryableData.GetEnumerator());

        mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
        return mockSet;
    }
}