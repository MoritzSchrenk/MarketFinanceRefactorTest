using SlothEnterprise.External;
using SlothEnterprise.ProductApplication.Applications;

namespace SlothEnterprise.ProductApplication.Company
{
    public static class ICompanyDataExtensions
    {
        public static CompanyDataRequest ToCompanyDataRequest(this ICompanyData company)
        {
            return new CompanyDataRequest
            {
                CompanyFounded = company.Founded,
                CompanyNumber = company.Number,
                CompanyName = company.Name,
                DirectorName = company.DirectorName
            };
        }
    }
}
