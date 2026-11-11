using Microsoft.Extensions.Caching.Memory;
using Nexam.Business.Interfaces;
using Nexam.DataAccess.Interfaces;
using Nexam.Models.Entities;
using Nexam.Models.ViewModels;

namespace Nexam.Business;

public sealed class SubjectService(ISubjectRepository subjects, IMemoryCache cache) : ISubjectService
{
    public const int PageSize = 48;

    public Task<List<Subject>> GetAllAsync() => subjects.GetAllAsync();

    public Task<List<Subject>> GetGeneralAsync() => subjects.GetGeneralAsync();

    public async Task<List<CountryCount>> GetCountriesAsync() =>
        await cache.GetOrCreateAsync("subject-countries", async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
            var list = await subjects.GetCountriesAsync();
            // "General" first, then the countries A-Z
            return list.OrderBy(c => c.Country == "General" ? 0 : 1).ThenBy(c => c.Country, StringComparer.OrdinalIgnoreCase).ToList();
        });

    public async Task<SubjectsPageViewModel> PageAsync(string country, string text, int page)
    {
        page = Math.Max(page, 1);
        var (items, total) = await subjects.PageAsync(country, text, page, PageSize);
        var countries = await GetCountriesAsync();
        return new SubjectsPageViewModel
        {
            Items = items, Total = total, Page = page, PageSize = PageSize,
            Country = country, Text = text, Countries = countries, TotalSubjects = countries.Sum(c => c.Count)
        };
    }

    public async Task<List<Subject>> ByCountryAsync(string country) =>
        (await subjects.PageAsync(country, null, 1, 5000)).Items;

    public Task<bool> SaveAsync(Subject subject) => subjects.SaveAsync(subject);
}
