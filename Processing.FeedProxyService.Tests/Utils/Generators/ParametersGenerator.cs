using Bogus;
using Moq;
using Processing.FeedProxyService.Model.Producer.Interfaces;
using System;
using System.Collections.Generic;

namespace Processing.FeedProxyService.Tests.Utils.Generators
{
    public class ParametersGenerator
    {
        public static IEnumerable<object[]> GetProducersCommon()
        {
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(1)) };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(2)) };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(3)) };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(5)) };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(10)) };

            List<Mock<IProducer>> GenerateMockProducers(int count)
            {
                var generator = new Faker<Mock<IProducer>>();

                var producerMocks = generator.Generate(count);

                producerMocks.ForEach(q => q.SetupGet(c => c.Environment).Returns("Mock"));

                return producerMocks;
            }
        }

        public static IEnumerable<object[]> ManyProducersNotConnected()
        {
            yield return new object[] { GenerateMockProducersWithNotConnected(2, 1), 1 };
            yield return new object[] { GenerateMockProducersWithNotConnected(3, 2), 1 };
            yield return new object[] { GenerateMockProducersWithNotConnected(5, 3), 2 };
            yield return new object[] { GenerateMockProducersWithNotConnected(8, 5), 3 };
            yield return new object[] { GenerateMockProducersWithNotConnected(10, 2), 8 };

            List<Mock<IProducer>> GenerateMockProducersWithNotConnected(int count, int notConnectedCount)
            {
                var generator = new Faker<Mock<IProducer>>();

                var producerMocks = generator.Generate(count);

                for (var i = 0; i < notConnectedCount; i++)
                {
                    var producerMock = producerMocks[i];
                    producerMock.Setup(c => c.Connect()).Throws<Exception>();
                }

                producerMocks.ForEach(q => q.SetupGet(c => c.Environment).Returns("Mock"));

                return producerMocks;
            }
        }

        public static IEnumerable<object[]> GetMessageFromConsumerParameters()
        {
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(1)), 5 };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(2)), 3 };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(3)), 12 };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(5)), 1 };
            yield return new object[] { new List<Mock<IProducer>>(GenerateMockProducers(10)), 7 };

            List<Mock<IProducer>> GenerateMockProducers(int count)
            {
                var generator = new Faker<Mock<IProducer>>();

                var producerMocks = generator.Generate(count);

                producerMocks.ForEach(q => q.SetupGet(c => c.Environment).Returns("Mock"));

                return producerMocks;
            }
        }
        public static IEnumerable<object[]> GetMessageFromConsumerNotConnectedProducersParameters()
        {
            yield return new object[] { GenerateMockProducersWithNotSended(2, 1), 4 };
            yield return new object[] { GenerateMockProducersWithNotSended(3, 2), 2 };
            yield return new object[] { GenerateMockProducersWithNotSended(5, 3), 1 };
            yield return new object[] { GenerateMockProducersWithNotSended(8, 5), 12 };
            yield return new object[] { GenerateMockProducersWithNotSended(10, 2), 5 };

            List<Mock<IProducer>> GenerateMockProducersWithNotSended(int count, int notSendedCount)
            {
                var generator = new Faker<Mock<IProducer>>();

                var producerMocks = generator.Generate(count);

                for (var i = 0; i < notSendedCount; i++)
                {
                    var producerMock = producerMocks[i];
                    producerMock.Setup(c => c.SendAsync(It.IsAny<byte[]>())).Throws<Exception>();
                }

                producerMocks.ForEach(q => q.SetupGet(c => c.Environment).Returns("Mock"));

                return producerMocks;
            }
        }
    }
}
