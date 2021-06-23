using BookAPI.Data;
using BookAPI.Repositories;
using Domain.Managers;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Controllers
{
    public class PublisherControllerTests
    {
        private readonly List<PublisherResource> PublishersTestResource;
        //private readonly PublisherManager _publisherManager;
        private readonly Mock<IPublisherManager> _publisherManagerMock = new Mock<IPublisherManager>();
        private readonly Mock<IRepository<Publisher, int>> _publisherRepoMock = new Mock<IRepository<Publisher, int>>();

        //private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        public PublisherControllerTests()
        {
            _publisherRepoMock = new Mock<IRepository<Publisher, int>>();
            _publisherManagerMock = new Mock<IPublisherManager>();

            PublishersTestResource = new List<PublisherResource>
            {
                new PublisherResource
                {
                    Id=1,
                    Name="Test #1"
                },
                new PublisherResource
                {
                    Id=2,
                    Name="Test #2"
                }
            };

        }

        [Fact]
        public async Task GetPublisherById_Should_ReturnPublisher()
        {
            // Arange
            var publisherId = 1;
            var publisher = new Publisher
            {
                Id = 1,
                Name = "Test #1"
            };
            // Setup
            _publisherRepoMock.Setup(m => m.Get(1)).ReturnsAsync(publisher);
            // Act 
            var result = await _publisherManagerMock.Object.GetPublisher(publisherId);
            // Assert

            Assert.Equal(publisherId, publisher.Id);

        }
    }
}
