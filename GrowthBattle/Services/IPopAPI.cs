using System.Collections.Generic;
using System.Threading.Tasks;
using Refit;
using GrowthBattle.Models;


namespace GrowthBattle
{
    [Headers("Content-Type: application/json")]
    public interface IPopAPI
    {
        [Get("/countries")]
        Task<CountryList> GetCountries();

        [Get("/population/{country}/today-and-tomorrow/")]
        Task<RootObject> GetPopulation(string country);
    }
}
