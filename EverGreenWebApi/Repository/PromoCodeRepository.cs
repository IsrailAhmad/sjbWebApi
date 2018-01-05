using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EverGreenWebApi.DBHelper;
using EverGreenWebApi.Interfaces;
using EverGreenWebApi.Models;

namespace EverGreenWebApi.Repository
{
    public class PromoCodeRepository : IPromoCodeRepository
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public PromoCodeModel ApplyPromoCode(int loginid, string promocode)
        {
            PromoCodeModel data = new PromoCodeModel();
            using (sjb_androidEntities context = new sjb_androidEntities())
            {
                var result = context.promocodemasters.Where(x => x.LoginId == loginid && x.PromoCode == promocode).FirstOrDefault();
                if (result.PromoCodeId > 0)
                {
                    data.PromoCodeId = result.PromoCodeId;
                    data.PromoCode = result.PromoCode;
                    data.Discount =(decimal) result.Discount;                  
                    data.LoginId = (int)result.LoginId;
                }
                return data;
            }
        }
    }
}