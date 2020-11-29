namespace Core.DTO.UseCaseRequests
{
    public class IdentifySailingChannelRequest
    {
        public string PossibleYouTubeChannelURL { get; private set; }

        public IdentifySailingChannelRequest(string possibleYouTubeChannelURL)
        {
            PossibleYouTubeChannelURL = possibleYouTubeChannelURL;
        }
    }
}
