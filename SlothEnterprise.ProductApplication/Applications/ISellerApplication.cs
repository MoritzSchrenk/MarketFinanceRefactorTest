using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication.Applications
{
    public interface ISellerApplication
    {
        /// <summary>
        /// The product being applied for
        /// </summary>
        IProduct Product { get; set; }
        /// <summary>
        /// The company receiving the application
        /// </summary>
        ICompanyData CompanyData { get; set; }
    }
}