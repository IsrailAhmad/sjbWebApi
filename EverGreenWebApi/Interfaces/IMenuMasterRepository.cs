using EverGreenWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Interfaces
{
    public interface IMenuMasterRepository: IDisposable
    {
        IEnumerable<MenuMasterModel> GetAllMenuList(int StoreId);
        IEnumerable<MenuMasterModel> GetAllMenuListByStoreId(int storeid);
        IEnumerable<MenuMasterModel> GetAllMenuListByStoreIdForWeb(int storeid);
        MenuMasterModel AddNewMenu(MenuMasterModel model);
        IEnumerable<MenuMasterModel> RemoveMenu(int id);
        MenuMasterModel GetMenuById(int id);
    }
}