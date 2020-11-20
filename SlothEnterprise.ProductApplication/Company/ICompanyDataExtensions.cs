using SlothEnterprise.External;
using SlothEnterprise.ProductApplication.Applications;
using System;
using System.Collections.Generic;
using System.Text;

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
