namespace Core.DTO.UseCaseRequests
{
    public class TopicsOverviewRequest
    {
        public string Language { get; set; }

        public TopicsOverviewRequest(string language)
        {
            Language = language;
        }
    }
}
