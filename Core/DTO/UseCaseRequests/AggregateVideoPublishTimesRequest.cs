namespace Core.DTO.UseCaseRequests
{
    public class AggregateVideoPublishTimesRequest
    {
        public string ChannelID { get; private set; }

        public AggregateVideoPublishTimesRequest(string channelId)
        {
            ChannelID = channelId;
        }
    }
}