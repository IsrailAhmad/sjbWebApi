using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;
using EverGreenWebApi.DBHelper;

namespace EverGreenWebApi.Repository
{
    public class CitiesRepository : ICitiesRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CitiesModel> GetAllCities(int PageSize, int PageNumber)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                PageNumber = PageNumber > 0 ? PageNumber - 1 : 0;
                PageSize = PageSize > 0 ? PageSize : 0;

                var result = context.citymasters.OrderBy(c => c.CityId).Skip(PageNumber * PageSize).Take(PageSize);
                var data = result.Select(c => new CitiesModel()
                {
                    CityId = c.CityId,
                    CityName = c.CityName
                }).ToList();

                return data;
            }
        }

        public IEnumerable<CitiesModel> GetCitiesByState(int stateId)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.citymasters.Where(c=> c.StateId == stateId).OrderBy(c => c.CityName);
                var data = result.Select(c => new CitiesModel()
                {
                    CityId = c.CityId,
                    CityName = c.CityName
                }).ToList();
                return data;
            }
        }
    }
}