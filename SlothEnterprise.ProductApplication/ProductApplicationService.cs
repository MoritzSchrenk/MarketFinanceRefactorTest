using System;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Company;
using SlothEnterprise.ProductApplication.Products;

namespace SlothEnterprise.ProductApplication
{
    public class ProductApplicationService
    {
        private readonly ISelectInvoiceService _selectInvoiceService;
        private readonly IConfidentialInvoiceService _confidentialInvoiceWebService;
        private readonly IBusinessLoansService _businessLoansService;

        public ProductApplicationService(ISelectInvoiceService selectInvoiceService, 
            IConfidentialInvoiceService confidentialInvoiceWebService, IBusinessLoansService businessLoansService)
        {
            _selectInvoiceService = selectInvoiceService;
            _confidentialInvoiceWebService = confidentialInvoiceWebService;
            _businessLoansService = businessLoansService;
        }

        public int SubmitApplicationFor(ISellerApplication application)
        {
            int result;

            switch (application.Product)
            {
                case SelectiveInvoiceDiscount sid:
                    result = _selectInvoiceService.SubmitApplicationFor(
                                application.CompanyData.Number.ToString(), sid.InvoiceAmount, sid.AdvancePercentage);
                    break;

                case ConfidentialInvoiceDiscount cid:
                    result = HandleConfidentialInvoiceDiscount(cid, application.CompanyData);
                    break;

                case BusinessLoans loans:
                    result = HandleBusinessLoan(loans, application.CompanyData);
                    break;

                default:
                    throw new ArgumentException("Unknown application type");
            }

            return result;
            
        }

        private int HandleConfidentialInvoiceDiscount(ConfidentialInvoiceDiscount cid, ICompanyData company)
        {
            int result;

            var companyDataRequest = company.ToCompanyDataRequest();

            var applicationResult = _confidentialInvoiceWebService.SubmitApplicationFor(
                                        companyDataRequest, cid.TotalLedgerNetworth, cid.AdvancePercentage, cid.VatRate);

            if (applicationResult.Success && applicationResult.ApplicationId.HasValue)
                result = applicationResult.ApplicationId.Value;
            else
                result = -1;

            return result;
        }

        private int HandleBusinessLoan(BusinessLoans loans, ICompanyData company)
        {
            int result;

            var companyDataRequest = company.ToCompanyDataRequest();

            var loansRequest = new LoansRequest
            {
                InterestRatePerAnnum = loans.InterestRatePerAnnum,
                LoanAmount = loans.LoanAmount
            };

            var applicationResult = _businessLoansService.SubmitApplicationFor(companyDataRequest, loansRequest);

            if (applicationResult.Success && applicationResult.ApplicationId.HasValue)
                result = applicationResult.ApplicationId.Value;
            else
                result = -1;

            return result;
        }
    }
}
