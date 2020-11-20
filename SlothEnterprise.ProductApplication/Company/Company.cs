using System;

namespace SlothEnterprise.ProductApplication.Applications
{
    public class Company : ICompany
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string DirectorName { get; set; }
        public DateTime Founded { get; set; }
    }
}