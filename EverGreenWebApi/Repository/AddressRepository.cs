using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Repository
{
    public class AddressRepository : IAddressRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public AddressModel AddNewAddress(AddressModel model)
        {
            AddressModel data = new AddressModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                addressmaster a = new addressmaster();
                a.CompleteAddress = model.CompleteAddress;       
                a.ZipCode = model.ZipCode;
                a.LandMark = model.LandMark;
                a.LoginID = model.LoginID;
                a.PhoneNumber = model.PhoneNumber;
                a.LocalityId = model.LocalityId;
                context.addressmasters.Add(a);
                var result = context.SaveChanges();
                if (result > 0)
                {
                    var addressdata = context.addressmasters.Where(z => z.LoginID == model.LoginID).OrderByDescending(x => x.AddressId).FirstOrDefault();
                    if (addressdata != null)
                    {

                        data.AddressId = addressdata.AddressId;                       
                        data.CompleteAddress = addressdata.CompleteAddress;                      
                        data.ZipCode = addressdata.ZipCode;
                        data.LandMark = addressdata.LandMark;
                        data.LoginID = (int)addressdata.LoginID;
                        data.PhoneNumber = addressdata.PhoneNumber;
                        data.LocalityId = (int)addressdata.LocalityId;
                    }
                }
            }
            return data;
        }

        public AddressModel UpdateAddress(AddressModel model)
        {

            AddressModel data = new AddressModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                addressmaster a = new addressmaster();
                a.AddressId = model.AddressId;               
                a.CompleteAddress = model.CompleteAddress;               
                a.ZipCode = model.ZipCode;
                a.LandMark = model.LandMark;
                a.LoginID = model.LoginID;
                a.PhoneNumber = model.PhoneNumber;
                a.LocalityId = model.LocalityId;
                context.Entry(a).State = System.Data.Entity.EntityState.Modified;
                var result = context.SaveChanges();
                if (result > 0)
                {
                    var addressdata = context.addressmasters.Where(x => x.AddressId == model.AddressId).FirstOrDefault();
                    if (addressdata != null)
                    {
                        data.AddressId = addressdata.AddressId;                       
                        data.CompleteAddress = addressdata.CompleteAddress;                       
                        data.ZipCode = addressdata.ZipCode;
                        data.LandMark = addressdata.LandMark;
                        data.LoginID = (int)addressdata.LoginID;
                        data.PhoneNumber = addressdata.PhoneNumber;
                        data.LocalityId = (int)addressdata.LocalityId;
                    }
                }
            }
            return data;
        }

        public IEnumerable<AddressModel> GetAllAddress(int loginId)
        {
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = (from add in context.addressmasters
                              join l in context.localitymasters on add.LocalityId equals l.LocalityId into j1
                              from j2 in j1.DefaultIfEmpty()
                              where add.LoginID == loginId
                              select new AddressModel()
                              {
                                  AddressId = add.AddressId,
                                  LoginID = (int)add.LoginID,
                                  CompleteAddress = add.CompleteAddress,   
                                  LandMark =add.LandMark,                              
                                  PhoneNumber = add.PhoneNumber,
                                  ZipCode = add.ZipCode,
                                  LocalityId = (int)add.LocalityId,
                                  LocalityName = j2.LocalityName
                              }).ToList();
                return result;
            }
        }

        public ResponseStatus DeleteAddress(int addressid)
        {
            ResponseStatus response = new ResponseStatus();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                addressmaster address = new addressmaster();
                address = context.addressmasters.Find(addressid);
                if (address != null)
                {
                    context.addressmasters.Remove(address);
                    context.SaveChanges();
                    response.isSuccess = true;
                    response.serverResponseTime = System.DateTime.Now;
                }
                else
                {
                    response.isSuccess = false;
                    response.serverResponseTime = System.DateTime.Now;
                }
            }
            return response;
        }
    }
}