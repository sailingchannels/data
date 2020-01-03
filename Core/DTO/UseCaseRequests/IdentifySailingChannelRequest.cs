namespace Core.DTO.UseCaseRequests
{
    public class IdentifySailingChannelRequest
    {
        public string PossibleYouTubeChannelURL { get; private set; }
        public string UserId { get; private set; }

        public IdentifySailingChannelRequest(string possibleYouTubeChannelURL, string userId)
        {
            PossibleYouTubeChannelURL = possibleYouTubeChannelURL;
            UserId = userId;
        }
    }
}
