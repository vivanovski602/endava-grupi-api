namespace endavaRestApi.Data
{
    public class ProductFilter
    {
       
            public string ProductCategory { get; set; }
            public string ProductBrand { get; set; }
            public decimal? PriceMin { get; set; }
            public decimal? PriceMax { get; set; }
            public string ProductSize { get; set; }
            public decimal? WeightMin { get; set; }
            public decimal? WeightMax { get; set; }
        
    }
}
