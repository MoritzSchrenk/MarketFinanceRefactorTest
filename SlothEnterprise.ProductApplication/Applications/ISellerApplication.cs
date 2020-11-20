using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Applications
{
    public interface ISellerApplication
    {
        IProduct Product { get; set; }
        ICompany CompanyData { get; set; }
    }
}