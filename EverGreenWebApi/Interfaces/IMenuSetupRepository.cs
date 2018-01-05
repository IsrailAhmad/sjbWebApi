using EverGreenWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EverGreenWebApi.Interfaces
{
    public interface IMenuSetupRepository:IDisposable
    {
        IEnumerable<MenuSetUpMasterModel> SetupMenu(MenuSetupModel model);
    }
}