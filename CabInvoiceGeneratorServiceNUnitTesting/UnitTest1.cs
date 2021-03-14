using NUnit.Framework;
using TDDCabInvoiceGeneratorService;
namespace CabInvoiceGeneratorServiceNUnitTesting
{
    public class Tests
    {
        InvoiceGenerator invoiceGenerator;
        [SetUp]
        public void Setup()
        {
            invoiceGenerator = new InvoiceGenerator();
        }

    }
}