namespace Core.DTO.UseCaseRequests
{
    public class TopicDetailRequest
    {
        public string TopicID { get; set; }

        public TopicDetailRequest(string topicId)
        {
            TopicID = topicId;
        }
    }
}
