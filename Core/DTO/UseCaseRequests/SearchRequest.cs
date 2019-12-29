namespace Core.DTO.UseCaseRequests
{
    public class SearchRequest
    {
        public string Query { get; set; }

        public SearchRequest(string query)
        {
            Query = query;
        }
    }
}
