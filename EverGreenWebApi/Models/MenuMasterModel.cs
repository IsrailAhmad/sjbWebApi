namespace EverGreenWebApi.Repository
{
    public class MenuMasterModel
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }  
        public decimal MenuPrice { get; set; }     
        public string ImageUrl { get; set; }
    }
}