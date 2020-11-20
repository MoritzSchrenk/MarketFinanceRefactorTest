using System;

namespace SlothEnterprise.ProductApplication.Applications
{
    public interface ICompanyData
    {
        string Name { get; set; }
        int Number { get; set; }
        string DirectorName { get; set; }
        DateTime Founded { get; set; }
    }
}