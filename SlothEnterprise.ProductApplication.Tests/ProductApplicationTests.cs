using Moq;
using SlothEnterprise.External;
using SlothEnterprise.External.V1;
using SlothEnterprise.ProductApplication.Applications;
using SlothEnterprise.ProductApplication.Products;
using Xunit;

namespace SlothEnterprise.ProductApplication.Tests
{
    public class ProductApplicationTests
    {
        Mock<IApplicationResult> MockApplicationResult;

        Mock<ISelectInvoiceService> MockSelectInvoiceService;
        Mock<IConfidentialInvoiceService> MockConfidentialInvoiceService;
        Mock<IBusinessLoansService> MockBusinessLoansService;

        ProductApplicationService Sut;


        int applicationId = 123;

        private void Setup()
        {
            MockApplicationResult = new Mock<IApplicationResult>();
            MockApplicationResult.Setup(m => m.Success).Returns(true);
            MockApplicationResult.Setup(m => m.ApplicationId).Returns(applicationId);

            MockSelectInvoiceService = new Mock<ISelectInvoiceService>();
            MockSelectInvoiceService.Setup(m => m.SubmitApplicationFor(It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<decimal>())).Returns(0);

            MockConfidentialInvoiceService = new Mock<IConfidentialInvoiceService>();
            MockConfidentialInvoiceService.Setup(m => m.SubmitApplicationFor(
                It.IsAny<CompanyDataRequest>(), It.IsAny<decimal>(), It.IsAny<decimal>(), It.IsAny<decimal>()))
                .Returns(MockApplicationResult.Object);

            MockBusinessLoansService = new Mock<IBusinessLoansService>();
            MockBusinessLoansService.Setup(m => m.SubmitApplicationFor(
                It.IsAny<CompanyDataRequest>(), It.IsAny<LoansRequest>()))
                .Returns(MockApplicationResult.Object);

            Sut = new ProductApplicationService(MockSelectInvoiceService.Object, MockConfidentialInvoiceService.Object, MockBusinessLoansService.Object);
        }




        [Fact]
        public void CanSubmitApplicationForSelectiveInvoiceDiscount()
        {
            Setup();

            var application = new SellerApplication
            {
                Product = new SelectiveInvoiceDiscount
                {
                    InvoiceAmount = 20M,
                    AdvancePercentage = 0.5M
                },
                CompanyData = new Company
                {
                    Number = 1
                }
            };

            var result = Sut.SubmitApplicationFor(application);

            MockSelectInvoiceService.Verify(m => m.SubmitApplicationFor(
                It.Is<string>(s => s.Equals("1")),
                It.Is<decimal>(d => d == 20M),
                It.Is<decimal>(d => d == 0.5M)
                ));
        }


        [Fact]
        public void CanSubmitApplicationForConfidentialInvoiceService()
        {
            Setup();

            var application = new SellerApplication
            {
                Product = new ConfidentialInvoiceDiscount
                {
                    Id = 1,
                    TotalLedgerNetworth = 100,
                    AdvancePercentage = 1
                },
                CompanyData = new Company
                {
                    Number = 2
                }
            };

            var result = Sut.SubmitApplicationFor(application);


            //Todo: should verify CompanyDataRequest contents
            MockConfidentialInvoiceService.Verify(m => m.SubmitApplicationFor(
                It.IsAny<CompanyDataRequest>(),
                It.Is<decimal>(d => d == 100),
                It.Is<decimal>(d => d == 1),
                It.Is<decimal>(d => d == VatRates.UkVatRate)
                ));

            Assert.Equal(applicationId, result);
        }

        [Fact]
        public void CanSubmitApplicationForBusinessLoan()
        {
            Setup();

            var application = new SellerApplication
            {
                Product = new BusinessLoans
                {
                    Id = 1,
                    LoanAmount = 200,
                    InterestRatePerAnnum = 1.5M

                },
                CompanyData = new Company
                {
                    Number = 3
                }
            };

            var result = Sut.SubmitApplicationFor(application);

            //Todo: should verify CompanyDataRequest and LoansRequest contents
            MockBusinessLoansService.Verify(m => m.SubmitApplicationFor(
                It.IsAny<CompanyDataRequest>(),
                It.IsAny<LoansRequest>()
                ));

            Assert.Equal(applicationId, result);
        }
    }
}
