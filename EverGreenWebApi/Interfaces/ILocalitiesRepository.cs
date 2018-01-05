using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface ILocalitiesRepository : IDisposable
    {
        IEnumerable<LocalitiesModel> GetAllLocalities(int PageSize, int PageNumber);
        IEnumerable<LocalitiesModel> GetAllLocalitiesByCity(int cityid);
        LocalitiesModel AddNewLocalityName(LocalitiesModel model);
        IEnumerable<LocalitiesModel> GetAllLocalitiesWithOutPagging();
        IEnumerable<LocalitiesModel> RemoveLocality(int id);
        LocalitiesModel GetLocalityById(int id);
    }
}