using NUnit.Framework;
using System.Collections.Generic;
using TDDCabInvoiceGeneratorService;
namespace CabInvoiceGeneratorServiceNUnitTesting
{
    public class Tests
    {
        InvoiceGenerator invoiceGenerator;
        RideRepository rideRepository;
        [SetUp]
        public void Setup()
        {
            invoiceGenerator = new InvoiceGenerator(InvoiceGenerator.ServiceType.NORMAL_RIDE);
            rideRepository = new RideRepository();
        }

        [TestCase(2, 4, 24)]
        [TestCase(0.1, 0.1, 5)]
        public void GivenTimeAndDistanceCalculateFare(double distance, double time, double output)
        {
            Ride ride = new Ride(distance, time);
            Assert.AreEqual(output, invoiceGenerator.returnTotalFareForSingleRide(ride));
        }
        [Test]
        public void GivenInvalidDistanceThrowException()
        {
            Ride ride = new Ride(-1, 1);
            InvoiceGeneratorException invoiceGeneratorException = Assert.Throws<InvoiceGeneratorException>(() => invoiceGenerator.returnTotalFareForSingleRide(ride));
            Assert.AreEqual(invoiceGeneratorException.type, InvoiceGeneratorException.ExceptionType.INVALID_DISTANCE);
        }
        [Test]
        public void GivenInvalidTimeThrowException()
        {
            Ride ride = new Ride(1, -1);
            InvoiceGeneratorException invoiceGeneratorException2 = Assert.Throws<InvoiceGeneratorException>(() => invoiceGenerator.returnTotalFareForSingleRide(ride));
            Assert.AreEqual(invoiceGeneratorException2.type, InvoiceGeneratorException.ExceptionType.INVALID_TIME);
        }
        [Test]
        public void GivenListOfRidesGenerateInvoice()
        {
            Ride ride1 = new Ride(1, 1);
            Ride ride2 = new Ride(2, 1);
            Ride ride3 = new Ride(1, 1);
            List<Ride> rides = new List<Ride>();
            rides.Add(ride1);
            rides.Add(ride2);
            rides.Add(ride3);
            Assert.AreEqual(43.0d, invoiceGenerator.returnTotalFareForMultipleRides(rides));
            Assert.AreEqual(14.333333333333334d, invoiceGenerator.averagePerRide);
            Assert.AreEqual(3, invoiceGenerator.numberOfRides);
        }
        [Test]
        public void GivenValidUserIdGenerateInvoice()
        {
            Ride ride1 = new Ride(1, 1);
            Ride ride2 = new Ride(2, 1);
            Ride ride3 = new Ride(1, 1);
            rideRepository.AddToRideRepository("Dilip", ride1);
            rideRepository.AddToRideRepository("Dilip", ride2);
            rideRepository.AddToRideRepository("Dilip", ride3);
            Assert.AreEqual(32.0d, invoiceGenerator.returnTotalFareForMultipleRides(rideRepository.returnListByUserId("Dilip")));
            Assert.AreEqual(16.0d, invoiceGenerator.averagePerRide);
            Assert.AreEqual(2, invoiceGenerator.numberOfRides);
        }
        [Test]
        public void GivenInValidUserIdGenerateInvoice()
        {
            Ride ride1 = new Ride(1, 1);
            Ride ride2 = new Ride(2, 1);
            Ride ride3 = new Ride(1, 1);
            rideRepository.AddToRideRepository("Dilip", ride1);
            rideRepository.AddToRideRepository("Dilip", ride2);
            rideRepository.AddToRideRepository("Dilip", ride3);
            var Exception = Assert.Throws<InvoiceGeneratorException>(() => invoiceGenerator.returnTotalFareForMultipleRides(rideRepository.returnListByUserId("Ranjit")));
            Assert.AreEqual(Exception.type, InvoiceGeneratorException.ExceptionType.INVALID_USER_ID);
        }
        [TestCase(2, 4, 38)]
        [TestCase(0.1, 0.1, 20)]
        public void GivenTimeAndDistance_calculatePremiumFare(double distance, double time, double output)
        {
            InvoiceGenerator invoiceGeneratorPremium = new InvoiceGenerator(InvoiceGenerator.ServiceType.PREMIUM_RIDE);
            Ride ride = new Ride(distance, time);
            Assert.AreEqual(output, invoiceGeneratorPremium.returnTotalFareForSingleRide(ride));
        }
    }
}