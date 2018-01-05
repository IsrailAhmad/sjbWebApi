using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface IStoreRepository: IDisposable
    {
        IEnumerable<StoreModel> GetAllStoreByLocality(int localityid,int loginId);
        StoreModel GetStoreDetailsById(int storeid);
        StoreModel GetFavouriteStoreByUser(int loginid);
        StoreModel AddUpdateFavouriteStoreByUser(int loginid,int storeid);
        ResponseStatus RemoveFavouriteStoreByUser(int loginid);
        StoreModel AddNewStore(StoreModel model);
        IEnumerable<StoreModel> GetAllStoreList();
        IEnumerable<StoreModel> RemoveStore(int id);
        StoreModel GetStoreById(int id);
        StoreModel StoreOpen(int id);
        StoreModel StoreClose(int id);
    }
}