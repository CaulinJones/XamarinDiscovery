using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GrowthBattle.Models
{
    public class TotalPopulation
    {
        public string date { get; set; }
        public int population { get; set; }
    }

    public class RootObject
    {
        public List<TotalPopulation> total_population { get; set; }
    }

    public class CountryList 
    {
        public IList<string> Countries { get; set; }

    }

}