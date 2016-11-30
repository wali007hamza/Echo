﻿using Echo;
using Moq;
using Samples.MultiDependency.Target;
using System;
using System.IO;
using IServiceProvider = Samples.MultiDependency.Target.IServiceProvider;

namespace Samples.MultiDependency
{
    internal class Program
    {
        private const string EchoFileName = "output.echo";

        private static void Main(string[] args)
        {
            //Record();
            Test();

            Console.ReadKey();
        }

        private static void Record()
        {
            // Arrange

            var billingMock = new Mock<IBilling>();
            billingMock
                .Setup(x => x.GetQuote(It.Is<QuoteRequest>(
                    quoteRequest => quoteRequest.Service == ServiceType.Entertainment)
                ))
                .Returns(new QuoteResponse()
                {
                    Price = 10.50,
                });

            billingMock
                .Setup(x => x.Charge(It.IsAny<PaymentRequest>()))
                .Returns(new PaymentResponse()
                {
                    Result = PaymentCode.Success,
                });

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(x => x.Provision(It.IsAny<ProvisioningRequest>()))
                .Returns(new ProvisioningResponse()
                {
                    ProvisionedServices = new[] { ServiceType.Entertainment, ServiceType.Laundry, },
                });

            // write all echoes to a file
            using (var output = new StreamWriter(EchoFileName))
            {
                // setup recording

                var writer = new EchoWriter(output);
                var recorder = new Recorder(writer);

                var recordedBilling = recorder.GetRecordingTarget<IBilling>(billingMock.Object);
                var recordedServiceProvider = recorder.GetRecordingTarget<IServiceProvider>(serviceProviderMock.Object);
                var actualEndpoint = new Endpoint(recordedBilling, recordedServiceProvider);
                var recordedEndpoint = recorder.GetRecordingTarget<IEndpoint>(actualEndpoint);

                // Act

                recordedEndpoint.Purchase(new PurchaseRequest()
                {
                    Customer = new User()
                    {
                        FullName = "John Smith",
                        Identifier = Guid.NewGuid(),
                    },
                    Payment = new CreditCardPaymentInstrument()
                    {
                        CardExpirationDate = DateTime.UtcNow.AddYears(1),
                        CardNumber = long.MaxValue,
                        CardOwner = "John Smith SR",
                        CardProvider = CreditCardProvider.Visa,
                    },
                    ServiceType = ServiceType.Entertainment,
                });
            }
        }

        private static void Test()
        {
            using (var streamReader = new StreamReader(EchoFileName))
            {
                // Arrange

                // setup an echo player
                var reader = new EchoReader(streamReader);
                var player = new Player(reader);

                // obtain external dependencies from the player
                var billing = player.GetReplayingTarget<IBilling>();
                var serviceProvider = player.GetReplayingTarget<IServiceProvider>();
                var testValues = player.GetEntryValues();

                // this is the the instance that is getting tested
                // we inject external dependencies provided by the player
                var endpointUnderTest = new Endpoint(billing, serviceProvider);

                // Act

                // call method you'd like to test with values provided by the player
                endpointUnderTest.Purchase(testValues.GetValue<PurchaseRequest>());
            }
        }
    }
}