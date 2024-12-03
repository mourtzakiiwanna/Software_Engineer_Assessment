using System.Net;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Models;
using Services;
using Xunit;

namespace CountriesProject.Tests
{
    public class CountryServiceTest
    {
        private readonly CountryDbContext _context;
        private readonly IMemoryCache _mockCache;
        private readonly CountryService _countryService;

        public CountryServiceTest()
        {
            var options = new DbContextOptionsBuilder<CountryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new CountryDbContext(options);
            _mockCache = new MemoryCache(new MemoryCacheOptions());
            var mockHttpClient = new HttpClient(new MockHttpMessageHandler());

            _countryService = new CountryService(_context, _mockCache, mockHttpClient);
        }

        [Fact]
        public async Task GetCountries_ReturnsCachedCountries_WhenCacheIsNotEmpty()
        {
            // Arrange
            var cachedCountries = new List<CountryDetails>
            {
                new CountryDetails { CommonName = "Cached Country", Capital = "Cached Capital", Borders = []}
            };

            _mockCache.Set("countries", cachedCountries);

            // Act
            var result = await _countryService.GetCountries();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(cachedCountries, result);
        }

        [Fact]
        public async Task GetCountries_ReturnsCountriesFromDatabase_WhenCacheIsEmpty()
        {
            // Arrange
            var countriesFromDb = new List<CountryDbModel>
            {
                new CountryDbModel { CommonName = "Country A", Capital = "Capital A", Borders = "Borders A" },
                new CountryDbModel { CommonName = "Country B", Capital = "Capital B", Borders = "Borders B"}
            };

            _context.Countries.AddRange(countriesFromDb);
            await _context.SaveChangesAsync();

            // Clear cache to simulate empty cache
            _mockCache.TryGetValue("countries", out _);

            // Act
            var result = await _countryService.GetCountries();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count); 

            // Check if the cache was populated
            _mockCache.TryGetValue("countries", out var cachedCountries);
            Assert.NotNull(cachedCountries);
            Assert.IsType<List<CountryDetails>>(cachedCountries);
        }

        [Fact]
        public async Task GetCountries_CallsExternalApi_WhenCacheAndDbAreEmpty()
        {
            // Act
            var result = await _countryService.GetCountries();

            // Assert
            Assert.NotEmpty(result);

            var cachedCountries = new List<CountryDetails>();
            bool cacheExists = _mockCache.TryGetValue("countries", out cachedCountries);

            Assert.True(cacheExists);
            Assert.NotNull(cachedCountries);
        }

        // MockHttpMessageHandler for simulating an API response
        public class MockHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("[{\"name\":{\"common\":\"Country A\"},\"capital\":[\"Capital A\"],\"borders\":[]}]")
                };
                return Task.FromResult(response);
            }
        }
    }
}