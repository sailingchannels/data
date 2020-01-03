using System.Collections.Generic;
using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Core.Interfaces.UseCases;
using Core.UseCases;
using Moq;
using NUnit.Framework;

namespace Core.Test
{
    public class IdentifySailingChannelsUseCaseTests
    {
        private IExtractYouTubeChannelIDUseCase _extractUseCase;
        private IIdentifySailingChannelsUseCase _identifyUseCase;

        [SetUp]
        public void Setup()
        {
            // mock YouTubeDataService
            var youTubeDataServiceMock = new Mock<IYouTubeDataService>();
            youTubeDataServiceMock.Setup(foo => foo.GetChannelIDFromUsername("xyz")).Returns(Task.FromResult("abc"));
            youTubeDataServiceMock.Setup(foo => foo.GetSnippets(It.IsAny<List<string>>()))
                .Returns(Task.FromResult(new List<YouTubeChannel>()
            {
                new YouTubeChannel()
                {
                    ID = "test",
                    Title = "Test",
                    Description = "This is a random youtube description",
                    Thumbnail = "https://thumbnails.com"
                }
            }));

            _extractUseCase = new ExtractYouTubeChannelIDUseCase(youTubeDataServiceMock.Object);

            // sailing terms repository
            var sailingTermsRepositoryMock = new Mock<ISailingTermRepository>();
            sailingTermsRepositoryMock.Setup(foo => foo.GetAll())
                .Returns(Task.FromResult(new List<SailingTerm>()
            {
                new SailingTerm()
                {
                    ID = "sail"
                }
            }));

            // channel repository
            var channelRepositoryMock = new Mock<IChannelRepository>();
            channelRepositoryMock.Setup(foo => foo.GetAll(It.IsAny<List<string>>()))
                .Returns(Task.FromResult(new List<Channel>()
            {
                new Channel()
                {
                    ID = "test2",
                    Title = "SY Test",
                    Description = "This is a sailboat test description"
                }
            }));

            _identifyUseCase = new IdentifySailingChannelsUseCase(
                youTubeDataServiceMock.Object,
                _extractUseCase,
                sailingTermsRepositoryMock.Object,
                channelRepositoryMock.Object
            );
        }

        [Test]
        public async Task TestIdentifyingDatabaseChannelAsSailingRelated()
        {
            // null
            var result = await _identifyUseCase.Handle(new IdentifySailingChannelsRequest(new List<string>()
            {
                "https://youtube.com/channel/test",
                "https://youtube.com/channel/test2"
            }));

            Assert.AreEqual(result.IdentifiedChannels.Count, 2);

            Assert.AreEqual(result.IdentifiedChannels[0].ChannelId, "test2");
            Assert.IsTrue(result.IdentifiedChannels[0].IsSailingChannel);
            Assert.AreEqual(result.IdentifiedChannels[0].Source, "db");


            Assert.IsFalse(result.IdentifiedChannels[1].IsSailingChannel);
            Assert.AreEqual(result.IdentifiedChannels[1].Source, "yt");
        }

        [Test]
        public async Task TestIdentifyingRejectedChannels()
        {
            // null
            var result = await _identifyUseCase.Handle(new IdentifySailingChannelsRequest(new List<string>()
            {
                "https://youtube.com/channel/test",
                "https://youtube.com/channel/test2",
                "https://google.de"
            }));

            Assert.AreEqual(result.IdentifiedChannels.Count, 2);
            Assert.AreEqual(result.Rejected.Count, 1);
            Assert.AreEqual(result.Rejected[0], "https://google.de");
        }
    }
}