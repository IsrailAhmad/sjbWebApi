using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface ICitiesRepository : IDisposable
    {
        IEnumerable<CitiesModel> GetAllCities(int PageSize, int PageNumber);
        IEnumerable<CitiesModel> GetCitiesByState(int stateId);

    }
}