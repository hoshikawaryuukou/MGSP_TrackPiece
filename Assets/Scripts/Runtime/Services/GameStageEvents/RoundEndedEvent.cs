namespace MGSP.TrackPiece.Services
{
    public enum GameResult
    {
        PlayerWhite,
        PlayerBlack,
        Draw
    }

    public sealed class RoundEndedEvent : IGameStageEvent
    {
        public GameResult Result { get; }

        public RoundEndedEvent(GameResult result)
        {
            Result = result;
        }
    }
}
