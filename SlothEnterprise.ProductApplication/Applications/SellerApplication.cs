using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Applications
{
    public class SellerApplication : ISellerApplication
    {
        public IProduct Product { get; set; }
        public ICompany CompanyData { get; set; }
    }
}