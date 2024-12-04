using CountriesProject.Data;
using CountriesProject.Models.Country;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Services
{
    public class CountryService
    {
        private readonly CountryDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;

        public CountryService(CountryDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            _httpClient = new HttpClient(handler);
        }

        public async Task<List<CountryDetails>> GetCountries()
        {
            try
            {
                if (_cache.TryGetValue("countries", out List<CountryDetails> cachedCountries))
                {
                    return cachedCountries;
                }

                var countriesFromDb = await _context.Countries.ToListAsync();

                if (countriesFromDb.Any())
                {
                    var countryResponses = countriesFromDb.Select(c => new CountryDetails
                    {
                        CommonName = c.CommonName,
                        Capital = c.Capital,
                        Borders = c.Borders?.Split(',').ToList()
                    }).ToList();

                    _cache.Set("countries", countryResponses);
                    return countryResponses;
                }

                var request = new HttpRequestMessage(HttpMethod.Get, "https://restcountries.com/v3.1/currency/eur");
                HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException("Error retrieving countries.");
                }

                string json = await response.Content.ReadAsStringAsync();
                var countryInfoList = await MapAndSaveCountries(json);
                _cache.Set("countries", countryInfoList);

                return countryInfoList;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Could not retrieve country data. Please try again later.");
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while processing your request.");
            }
        }

        private async Task<List<CountryDetails>> MapAndSaveCountries(string json)
        {
            var countries = JsonConvert.DeserializeObject<List<Country>>(json);
            var countryInfoList = new List<CountryDetails>();

            foreach (var country in countries)
            {
                var countryInfo = new CountryDetails
                {
                    CommonName = country.Name.Common,
                    Capital = country.Capital?.FirstOrDefault(),
                    Borders = country.Borders ?? new List<string>()
                };

                countryInfoList.Add(countryInfo);
            }

            await SaveCountriesToDatabase(countryInfoList);
            return countryInfoList;
        }

        private async Task SaveCountriesToDatabase(List<CountryDetails> countryInfoList)
        {
            foreach (var info in countryInfoList)
            {
                var countryDbModel = new CountryDbModel
                {
                    CommonName = info.CommonName,
                    Capital = info.Capital,
                    Borders = string.Join(",", info.Borders)
                };

                await _context.Countries.AddAsync(countryDbModel);
            }

            await _context.SaveChangesAsync();
        }
    }
}
