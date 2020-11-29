using System.Threading.Tasks;
using Core.DTO.UseCaseRequests;
using Core.Interfaces.Services;
using Core.Interfaces.UseCases;
using Core.UseCases;
using Moq;
using NUnit.Framework;

namespace Core.Test
{
    public class ExtractYouTubeChannelIDUseCaseTests
    {
        private IExtractYouTubeChannelIDUseCase _useCase;

        [SetUp]
        public void Setup()
        {
            // mock YouTubeDataService
            var youTubeDataServiceMock = new Mock<IYouTubeDataService>();
            youTubeDataServiceMock.Setup(foo => foo.GetChannelIdFromUsername("xyz")).Returns(Task.FromResult("abc"));

            _useCase = new ExtractYouTubeChannelIDUseCase(youTubeDataServiceMock.Object);
        }

        [Test]
        public async Task TestNullURL()
        {
            // null
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest(null));
            Assert.IsNull(result.ChannelId);
        }

        [Test]
        public async Task TestEmptyURL()
        {
            // empty string
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest(""));
            Assert.IsNull(result.ChannelId);
        }

        [Test]
        public async Task TestWhitespaceURL()
        {
            // whitespace
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest(" "));
            Assert.IsNull(result.ChannelId);
        }

        [Test]
        public async Task TestNonValidURL()
        {
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest("abc"));
            Assert.IsNull(result.ChannelId);
        }

        [Test]
        public async Task TestUsernameURL()
        {
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest("https://youtube.com/user/xyz"));
            Assert.AreEqual(result.ChannelId, "abc");
        }

        [Test]
        public async Task TestUsernameURLWithQueryParameter()
        {
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest("https://youtube.com/user/xyz?abc=123"));
            Assert.AreEqual(result.ChannelId, "abc");
        }

        [Test]
        public async Task TestChannelURL()
        {
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest("https://youtube.com/channel/abc"));
            Assert.AreEqual(result.ChannelId, "abc");
        }

        [Test]
        public async Task TestChannelURLWithQueryParameter()
        {
            var result = await _useCase.Handle(new ExtractYouTubeChannelIdRequest("https://youtube.com/channel/abc?test=test"));
            Assert.AreEqual(result.ChannelId, "abc");
        }
    }
}