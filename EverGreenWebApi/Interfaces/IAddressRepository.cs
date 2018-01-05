using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Interfaces
{
    public interface IAddressRepository:IDisposable
    {
        AddressModel AddNewAddress(AddressModel model);
        AddressModel UpdateAddress(AddressModel model);
        IEnumerable<AddressModel> GetAllAddress(int loginId);
        ResponseStatus DeleteAddress(int addressid);


    }
}